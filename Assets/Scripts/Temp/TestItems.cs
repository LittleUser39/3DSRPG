using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItems : MonoBehaviour
{
    List<GameObject> inventory = new List<GameObject>();
    List<GameObject> combatants = new List<GameObject>();

    private void Start()
    {
        //inventory에 아이템 추가
        CreateItems();

        //comabtants에 전투 유닛을 추가
        CreateCombatants();

        StartCoroutine(SimulateBattle());
    }
    
    //옵저버 패턴
    //델리게이트 등록
    void OnEnable()
    {
        // Equipment.EquippedNotification 라는 키로 OnEquippedItem 을 등록한다.
        this.AddObserver(OnEquippedItem, Equipment.EquippedNotification);

        // Equipment.UnEquippedNotification 라는 키로 OnUnEquippedItem 을 등록한다.
        this.AddObserver(OnUnEquippedItem, Equipment.UnEquippedNotification);
    }

    //델리게이트 해제
    void OnDisable()
    {
        this.RemoveObserver(OnEquippedItem, Equipment.EquippedNotification);
        this.RemoveObserver(OnUnEquippedItem, Equipment.UnEquippedNotification);
    }

    //장착하기 매세지 띄우기
    void OnEquippedItem(object sender,object args)
    {
        Equipment eq = sender as Equipment;
        Equippable item = args as Equippable;
        inventory.Remove(item.gameObject); //인벤토리에서 나감
        string message = string.Format("{0}가 {1}를 장착", eq.name, item.name);
        Debug.Log(message);
    }
    void OnUnEquippedItem(object sender,object args)
    {
        Equipment eq = sender as Equipment;
        Equippable item = args as Equippable;
        inventory.Add(item.gameObject); //인벤토리에 들어옴
        string message = string.Format("{0}가 {1}를 장착해제", eq.name, item.name);
        Debug.Log(message);
    }
    GameObject CreateItem(string title,StateTypes type,int amount)
    {
        //게임 오브젝트 생성
        GameObject item = new GameObject(title);

        //능력치 설정
        StatModifierFeature smf = item.AddComponent<StatModifierFeature>();
        smf.type = type;
        smf.amount = amount;
        return item;
    }

    //소모성 아이템 생성
    GameObject CreateConumableItem(string title,StateTypes type,int amount)
    {
        //게임오브젝트 생성
        //소모품의 능력치 설정
        GameObject item = CreateItem(title, type, amount);
        item.AddComponent<Consumable>();
        return item;
    }

    //장착아이템 생성
    GameObject CreateEquippableItem(string title,StateTypes type,int amount,EquipSlots slot)
    {
        //게임 오브젝트 생성
        GameObject item = CreateItem(title, type, amount);
        Equippable equip = item.AddComponent<Equippable>();

        //슬롯설정
        equip.defaultSlots = slot;
        return item;
    }

    //영웅생성
    GameObject CreateHero()
    {
        //Hierarchy 뷰에 hero라는 게임오브젝트 생성
        //체력,공격,방어 관련 능력치 세팅
        GameObject actor = CreateActor("Hero");
        actor.AddComponent<Equipment>();
        return actor;
    }
    //유닛 능력치 설정
    GameObject CreateActor(string title)
    {
        GameObject actor = new GameObject(title);
        Stats s = actor.AddComponent<Stats>();
        s[StateTypes.HP] = s[StateTypes.MHP] = UnityEngine.Random.Range(500, 1000);
        s[StateTypes.ATK] = UnityEngine.Random.Range(30, 50);
        s[StateTypes.DEF] = UnityEngine.Random.Range(30, 50);
        return actor;
    }

    //인벤토리에 아이템을 추가
    void CreateItems()
    {
        inventory.Add(CreateConumableItem("Health Potion", StateTypes.HP, 300)); // 체력회복
        inventory.Add(CreateConumableItem("Bomb", StateTypes.HP, -150)); // 소모성 폭탄

        // 무기1
        inventory.Add(CreateEquippableItem("Sword", StateTypes.ATK, 10, EquipSlots.Primary));

        // 무기2
        inventory.Add(CreateEquippableItem("Broad Sword", StateTypes.ATK, 15, (EquipSlots.Primary | EquipSlots.Secondary)));

        // 방패
        inventory.Add(CreateEquippableItem("Shield", StateTypes.DEF, 10, EquipSlots.Secondary));
    }

    //몬스터 및 영웅 추가
    void CreateCombatants()
    {
        combatants.Add(CreateHero());
        combatants.Add(CreateActor("Monster"));
    }

    IEnumerator SimulateBattle()
    {
        //적 또는 영웅이 죽을때까지 반복
        while (VictoryCheck() == false)
        {
            //로그 출력
            LogCombatants();

            //영웅과 적 턴을 진행
            HeroTurn();
            EnemyTurn();
            yield return new WaitForSeconds(1);
        }
        LogCombatants();
        Debug.Log("전투종료");
    }

    //영웅의 턴
    void HeroTurn()
    {
        int rnd = UnityEngine.Random.Range(0, 2);
        switch(rnd)
        {
            case 0:
                //영웅이 몬스터를 공격
                Attack(combatants[0], combatants[1]);
                break;
            default:
                //소모성 아이템 사용 및 장착아이템 장착
                UseInventory();
                break;
        }
    }
    //적의 턴
    void EnemyTurn()
    {
        Attack(combatants[1], combatants[0]);
    }

    //공격하기
    void Attack(GameObject attacker,GameObject defender)
    {
        Stats s1 = attacker.GetComponent<Stats>();
        Stats s2 = defender.GetComponent<Stats>();

        //최종 피해 산출
        int damage = Mathf.FloorToInt((s1[StateTypes.ATK] * 4 - s2[StateTypes.DEF] * 2) * UnityEngine.Random.Range(0.9f, 1.1f));

        //피격자 체력 감소
        s2[StateTypes.HP] -= damage;

        string message = string.Format("{0}가{1}를 공격,{2}의 피해", attacker.name, defender.name, damage);
        Debug.Log(message);
    }
    void UseInventory()
    {
        //인벤토리 안에 있는 랜덤한 아이템을 item 변수에 저장
        int rnd = UnityEngine.Random.Range(0, inventory.Count);
        GameObject item = inventory[rnd];

        //아이템 컴포넌트가 소모성아이템이 아니였으면
        if(item.GetComponent<Consumable>()!=null)
        {
            //소모성 아이템 사용
            ConsumeItem(item);
        }
        else
        {
            //아이템 장착
            EquipItem(item);
        }
    }

    //소모성 아이템 사용
    void ConsumeItem(GameObject item)
    {
        inventory.Remove(item);

        StatModifierFeature smf = item.GetComponent<StatModifierFeature>();
        if(smf.amount>0)
        {
            //영웅에게 적용
            item.GetComponent<Consumable>().Consume(combatants[0]);
            Debug.Log("잘먹었습니다");
        }
        else
        {
            //몬스터에게 적용
            item.GetComponent<Consumable>().Consume(combatants[1]);
            Debug.Log("집어던짐");
        }
    }

    //아이템 장착하기
    void EquipItem(GameObject item)
    {
        Debug.Log("아이템 장착하기");
        Equippable toEquip = item.GetComponent<Equippable>();
        Equipment equipment = combatants[0].GetComponent<Equipment>();

        //아이템 장착
        equipment.Equip(toEquip, toEquip.defaultSlots);
    }

    //승리조건 체크
    //몬스터 또는 영웅의 체력 검사
    bool VictoryCheck()
    {
        for(int i=0;i<2;++i)
        {
            Stats s = combatants[i].GetComponent<Stats>();
            if (s[StateTypes.HP] <= 0)
                return true;
        }
        return false;
    }

    //전투 인원들 로그 출력
    void LogCombatants()
    {
        Debug.Log("===============");
        for(int i=0;i<2;++i)
        {
            LogToConsole(combatants[i]);
        }
        Debug.Log("================");
    }
    void LogToConsole(GameObject actor)
    {
        Stats s = actor.GetComponent<Stats>();

        string message = string.Format("이름:{0},체력:{1}/{2},공격:{3},방어:{4}", actor.name, s[StateTypes.HP], s[StateTypes.MHP], s[StateTypes.ATK], s[StateTypes.DEF]);
        Debug.Log(message);
    }
}
