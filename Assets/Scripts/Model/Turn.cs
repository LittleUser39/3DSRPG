using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn
{
    //이번 턴에 움직이는 대상
    public Unit actor;
    //타겟리스트를 저장하는 변수
    public List<Tile> targets;
    //이동,공격 진행 여부
    public bool hasUnitMoved;
    public bool hasUnitActed;

    //최종적으로 이동을 완료했는지 체크하는 변수
    public bool lockMove;

    //능력 메뉴를 통해 어떤능려깅 선택되었는지 참조
    public Ability ability;
    //시작 타일
    Tile startTile;
    //시작 방향
    Directions startDir;
    public PlanOfAttack plan;

    public void Change(Unit current)
    {
        //행동하는 캐릭터
        actor = current;

        //공격,이동을 아직 안했음으로 변경
        hasUnitMoved = false;
        hasUnitActed = false;
        lockMove = false;

        //시작 타일, 시작방향 설정
        startTile = actor.tile;
        startDir = actor.dir;
        plan = null;
    }

    //이동이 취소됨
    public void UndoMove()
    {
        hasUnitMoved = false;
        actor.Place(startTile);
        actor.dir = startDir;
        actor.Match();
    }
}
