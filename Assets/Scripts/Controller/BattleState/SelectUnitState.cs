using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//유닛을 선택하기 전의 상태
public class SelectUnitState : BattleState
{
    //턴순서대로 유닛이 자동으로 선택되는 방식으로 변경
    //todo 여기서 AP순서대로 턴을 옮기도록 해야할듯?
    
    int index = -1;

    public override void Enter()
    {
        base.Enter();
        StartCoroutine("ChangeCurrentUnit");
    }
    public override void Exit()
    {
        base.Exit();
        StatPanelController.HidePrimary();
    }
    IEnumerator ChangeCurrentUnit()
    {
        index = (index + 1) % units.Count;
        turn.Change(units[index]);
        RefreshPrimaryStatPanel(pos);
        yield return null;
        owner.ChangeState<CommandSelectionState>();
    }
   

}

