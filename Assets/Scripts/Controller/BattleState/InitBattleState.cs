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
        yield return null;

        //현재 상태를 movetargetstate로 변경
        owner.ChangeState<MoveTargetState>();
    }
}
