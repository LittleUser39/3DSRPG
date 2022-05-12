using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreState : BattleState
{
    //메뉴창을 취소 했을때 변경되는 상태
    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
    }

    //클릭으로 다시 메뉴판이 열려있는 CommandSelectionState 상태가 됨
    protected override void OnFire(object Sender, InfoEventArgs<int> e)
    {
        if (e.info == 0)
            owner.ChangeState<CommandSelectionState>();
    }
}
