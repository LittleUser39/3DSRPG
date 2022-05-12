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
        CreateComnatants();

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
        //소모푼의 능력치 설정
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

    GameObject CreateHero()
    {

    }
}
