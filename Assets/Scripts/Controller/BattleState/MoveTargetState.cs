using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//유닛을 선택해서 이동범위가 표시되는 상태
public class MoveTargetState : BattleState
{
    List<Tile> tiles;
    public override void Enter()
    {
        base.Enter();
        //사용자가 unit을 선택하면 movetargetstate상태가 되어
        //이동 가능한 타일들의 색상을 변경
        //턴 순서에 따라 자동으로 선택
        //turn 의 actor 정보를 담음
        Movement mover = turn.actor.GetComponent<Movement>();
        
        tiles = mover.GetTilesInRange(board);
        board.SelectTile(tiles);
        RefreshPrimaryStatPanel(pos);
        //if (driver.Current == Drivers.Computer)
          //  StartCoroutine(ComputerHighlightMoveTarget());
    }

    public override void Exit()
    {
        base.Exit();

        //MoveTargetState 상태가 종료되면
        //변경된 타일들의 색상을 원래대로 변경
        board.DeSelectTiles(tiles);
        tiles = null;
        StatPanelController.HidePrimary();
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
        RefreshPrimaryStatPanel(pos);
    }

    protected override void OnFire(object Sender, InfoEventArgs<int> e)
    {
        //클릭한 타일로 이동
        if (tiles.Contains(owner.currentTile))
            owner.ChangeState<MoveSequenceState>();
        //현재 상태를 취소하면 메뉴판이 열려있는 명령상태로 돌아감
        else
            owner.ChangeState<CommandSelectionState>();
    }
}
