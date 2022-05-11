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

        yield return null;

        // 현재 상태를 SelectUnitState로 변경한다.
        owner.ChangeState<CutSceneState>();
    }

// 임시 함수
    void SpawnTestUnits()
    {
        System.Type[] components
            = new System.Type[]
            { typeof(WalkMoveMent), typeof(FlyMoveMent), typeof(TeleportMovement) };


        for (int i = 0; i < 3; ++i)
        {
            // 영웅을 생성하고
            GameObject instance = Instantiate(owner.heroPrefab) as GameObject;

            // 해당 영웅의 시작 좌표를 부여하고.
            Point p = new Point((int)levelData.tiles[i].x, (int)levelData.tiles[i].z);
            Unit unit = instance.GetComponent<Unit>();
            unit.Place(board.GetTile(p));
            unit.Match();

            // 영웅의 이동 방식을 넣고
            Movement m = instance.AddComponent(components[i]) as Movement;

            // 이동범위와 점프 높이를 설정한다.
            m.range = 5;
            m.jumpHeight = 1;
        }
    }
}



