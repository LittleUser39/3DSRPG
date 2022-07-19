using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//전투에 들어가는 상태
public class InitBattleState : BattleState
{
   
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Init());
    }
    IEnumerator Init()
    {
        

        //타일맵 로드
        //레벨 데이터를 로드
        board.Load(levelData);
        
        //이렇게 해서 스테이지 마지막에 경험치를 주는 방식으로 하면 될듯

        //현재 선택된 타일인디게이터(게임오브젝트)의 좌표를 설정
        Point p = new Point((int)levelData.tiles[0].x, (int)levelData.tiles[0].z);
        SelectTile(p);
        

        //현재 상태를 movetargetstate로 변경
        owner.ChangeState<MoveTargetState>();

        // 여기서 몬스터랑 영웅 추가 해주면 될듯?
        SpwanUnits();
        
        // 임시 코드(영웅을 소환)
        // SpawnTestUnits();

        //임시 코드(승리조건)
        if(levelData.ToString() == "Boss")
            AddVictoryCondition();
        AddVictoryCondition2();
        //라운드 코루틴을 등록
        owner.round = owner.gameObject.AddComponent<TurnOrderController>().Round();
        
        //한프레임 쉰다
        yield return null;

        // 현재 상태를 SelectUnitState로 변경한다.
        owner.ChangeState<CutSceneState>();
    }

    //승리조건을 추가하는 함수
    //todo 나중에 교체 예정
    private void AddVictoryCondition()
    {
        //BC에 승리조건 컴포넌트 추가
        DefeatTargetVictoryCondition vc = owner.gameObject.AddComponent<DefeatTargetVictoryCondition>();
        //적군 하나 지정해서 타겟으로 설정
        Unit enemy = units[units.Count - 1];
        vc.target = enemy;
        //일정 체력 이하로 설정(10) 체력 이하로 가면 승리하도록
        Health health = enemy.GetComponent<Health>();
        health.minHP = 50;
    }
    void AddVictoryCondition2()
    {
        DefeatAllEnemiesVictoryCondition vc = owner.gameObject.AddComponent<DefeatAllEnemiesVictoryCondition>();
    }
    
    //영웅과 몬스터를 각각 만들어서 배치해주는 함수
    void SpwanUnits()
    {
        
        GameObject heroContainer = GameObject.FindGameObjectWithTag("Container");
        GameObject unitContainer = new GameObject("Units");
        unitContainer.transform.SetParent(owner.transform);
        //heroContainer.transform.SetParent(owner.transform);
        

        List<GameObject> hero = new List<GameObject>();
        List<GameObject> monster = AddStageMonster(levelData.name);

        List<Tile> locations = new List<Tile>(board.tiles.Values);
        
        // 영웅컨테이너가 비어 있으면 영웅 추가
        if (heroContainer.transform.childCount == 0)
        {
            hero = AddHero();
            DontDestroyOnLoad(heroContainer);
        }
        // 비어있지 않으면 hero 리스트에 추가
       else
        {
            for (int i = 0; i < heroContainer.transform.childCount; ++i)
            {
                hero.Add(heroContainer.transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < hero.Count; ++i)
        {

            GameObject instance = hero[i];
            //자식이 영웅 수보다 작으면(처음 만들때)
            if (heroContainer.transform.childCount < hero.Count)
            {
                instance.transform.SetParent(heroContainer.transform);
            }
            //랜덤값으로 생성될 타일 위치 저장
            int random = UnityEngine.Random.Range(0, locations.Count);
            Tile randomTile = locations[random];
            locations.RemoveAt(random);

            //유닛을 랜덤값 타일에 배치
            Unit unit = instance.GetComponent<Unit>();
            unit.Place(randomTile);
            //유닛의 바라보는 방향 랜덤으로 생성
            unit.dir = (Directions)UnityEngine.Random.Range(0, 4);
            unit.Match();
            
            // 다음 전투로 넘어갈때 AP 초기화가 안되더라 - 완료 - 
            Stats stats = unit.GetComponent<Stats>();
            stats.SetValue(StateTypes.AP, 0, false);
            
            //유닛을 추가
            units.Add(unit);
        }
        for (int i = 0; i < monster.Count; ++i)
        {

            GameObject instance = monster[i];
            instance.transform.SetParent(unitContainer.transform);

            //랜덤값으로 생성될 타일 위치 저장
            int random = UnityEngine.Random.Range(0, locations.Count);
            Tile randomTile = locations[random];
            locations.RemoveAt(random);

            //유닛을 랜덤값 타일에 배치
            Unit unit = instance.GetComponent<Unit>();
            unit.Place(randomTile);
            //유닛의 바라보는 방향 랜덤으로 생성
            unit.dir = (Directions)UnityEngine.Random.Range(0, 4);
            unit.Match();
            
            
            //유닛을 추가
            units.Add(unit);
        }

        
        monster.Clear();
        //첫번째 유닛의 위치를 선택
        SelectTile(units[0].tile.pos);
    }

    //테스트 유닛 생성 함수
    void SpawnTestUnits()
    {
        //string 배열을 만들어서 unitrecipe의 이름을 배열에 저장
        string[] recipes = new string[]
        {
            "Warrior",
            "Rogue",
            "Wizard",
            "Enemy Warrior",
            "Enemy Rogue",
            "Enemy Wizard"
        };
        //유닛을 배틀 컨트롤러 자식으로 만듬

        GameObject unitContainer = new GameObject("Units");
        unitContainer.transform.SetParent(owner.transform);

        List<Tile> locations = new List<Tile>(board.tiles.Values);
        for(int i=0;i<recipes.Length;++i)
        {
            //랜덤으로 레벨값 생성
            
            int level = 1;//UnityEngine.Random.Range(1,4);
            
            //팩토리에서 이름에 따라 유닛생성
            GameObject instance = UnitFactory.Create(recipes[i], level);
            instance.transform.SetParent(unitContainer.transform);
           
            //랜덤값으로 생성될 타일 위치 저장
            int random = UnityEngine.Random.Range(0, locations.Count);
            Tile randomTile = locations[random];
            locations.RemoveAt(random);

            //유닛을 랜덤값 타일에 배치
            Unit unit = instance.GetComponent<Unit>();
            unit.Place(randomTile);
            //유닛의 바라보는 방향 랜덤으로 생성
            unit.dir = (Directions)UnityEngine.Random.Range(0, 4);
            unit.Match();

            //유닛을 추가
            units.Add(unit);
        }

        //첫번째 유닛의 위치를 선택
        SelectTile(units[0].tile.pos);
    }

    //스테이지 별로 몬스터 추가하는 함수
    List<GameObject> AddStageMonster(string levelData)
    {
        string[] recipes =new string[3];
        string stage = levelData;
        int level = 1;
        List<GameObject> instance = new List<GameObject>();

        //여기서 추가하면 됨
        switch (stage)
        { 
         case "Stage 1":
                recipes = new string [] { "Enemy Warrior", "Enemy Rogue", "Enemy Wizard"};
                owner.stageEXP = 10000;
                break;

         case "Stage 2":
                recipes = new string[] { "Enemy Warrior", "Enemy Warrior", "Enemy Wizard" };
                owner.stageEXP = 20000;
                break;
        }
        
        for(int i =0;i<recipes.Length; ++ i)
        {
            GameObject monster = UnitFactory.Create(recipes[i], level);
            instance.Add(monster);
        }

        return instance;
    }

    List<GameObject> AddHero()
    {
        string[] recipes = new string[]
        {
            "Warrior",
            "Rogue",
            "Wizard",
        };

        int level = 1;
        
        List<GameObject> instance = new List<GameObject>();

        for (int i = 0; i < recipes.Length; ++i)
        {
            GameObject hero = UnitFactory.Create(recipes[i], level);
            instance.Add(hero); 
        }

        return instance;
    }


}



//레벨설정 EX)
//Rank rank=instance.AddComponent<Rank>();
//rank.Init(10);


// 지금 문제가 제일 처음에 전투하는 것은 그냥 새로 만들면 되는데 (데이터를)
// 두번째 전투 부터는 이제 프리팹은 새로 만든다 치는데
// 전투를 했던 나의 유닛들의 데이터를 덮어씌워야함
// *컨테이너에 보관후 그 컨테이너를 부수지 않고 계속 이용하는 방식으로 구현*


// 스테이지 나눠야함 스테이지 별로 함수를 만들지 스트링 가져와서 만들지 선택
// 몬스터 만드는건 그냥 데이터 만들고 프리팹 설정하고 하면되서 데이터 영역에서 만들기만 하면 됨
// 레시피에 추가만 해주면 됨
// 
//
