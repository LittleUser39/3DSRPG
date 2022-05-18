using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        board.Load(levelData);

        //현재 선택된 타일인디게이터(게임오브젝트)의 좌표를 설정
        Point p = new Point((int)levelData.tiles[0].x, (int)levelData.tiles[0].z);
        SelectTile(p);
        

        //현재 상태를 movetargetstate로 변경
        owner.ChangeState<MoveTargetState>();
        
        // 임시 코드(영웅을 소환)
        SpawnTestUnits();

        //라운드 코루틴을 등록
        owner.round = owner.gameObject.AddComponent<TurnOrderController>().Round();
        
        //한프레임 쉰다
        yield return null;

        // 현재 상태를 SelectUnitState로 변경한다.
        owner.ChangeState<CutSceneState>();
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
        List<Tile> locations = new List<Tile>(board.tiles.Values);
        for(int i=0;i<recipes.Length;++i)
        {
            //랜덤으로 레벨값 생성
            //일단 1렙으로 설정
            int level = 1;//UnityEngine.Random.Range(9, 12);
            //팩토리에서 이름에 따라 유닛생성
            GameObject instance = UnitFactory.Create(recipes[i], level);

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
}


//레벨설정 EX)
//Rank rank=instance.AddComponent<Rank>();
//rank.Init(10);



