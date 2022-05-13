using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//메뉴창을 취소했을때 변경되는 상태
public class ExploreState : BattleState
{
    public override void Enter()
    {
        //해당 상태가 되면 초상화 정보를 출력
        base.Enter();
        StatPanelController.ShowPrimary(turn.actor.gameObject);
    }
    public override void Exit()
    {
        base.Exit();
        //상태가 해제되면 초상화 UI를 숨기기
        StatPanelController.HidePrimary();
    }
    //메뉴창을 취소 했을때 변경되는 상태
    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);

        //대상의 정보를 출력
        RefreshPrimaryStatPanel(pos);
    }

    //클릭으로 다시 메뉴판이 열려있는 CommandSelectionState 상태가 됨
    protected override void OnFire(object Sender, InfoEventArgs<int> e)
    {
        if (e.info == 0)
            owner.ChangeState<CommandSelectionState>();
    }
}
