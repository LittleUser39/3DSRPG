using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//대상을 어떤 방식으로 공격할지 결정하는 상태
public class CategorySelectionState : BaseAbilityMenuState
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
    protected override void LoadMenu()
    {
        if(menuOption==null)
        {
            menuTitle = "Action";
            menuOption = new List<string>(3);
            //공격 종류
            menuOption.Add("Attack");
            menuOption.Add("White Magic");
            menuOption.Add("Black Magic");
        }
        AbilityMenuPanelController.Show(menuTitle, menuOption);
    }

    protected override void Confirm()
    {
        //버튼을 선택했을 때의 처리
        switch(AbilityMenuPanelController.selection)
        {
            case 0:
                Attack();
                break;
            case 1:
                SetCategory(0);
                break;
            case 2:
                SetCategory(1);
                break;
        }
    }

    protected override void Cancel()
    {
        owner.ChangeState<CommandSelectionState>();
    }

    void Attack()
    {
        //공격 했음으로 체크
        turn.hasUnitActed = true;

        if(turn.hasUnitMoved)
        {
            //이미 이동을 한 상태이면 lockMove도 true 처리
            turn.lockMove = true;
        }

        //명령대기 상태로 돌아감
        owner.ChangeState<CommandSelectionState>();
    }

    //특수 공격을 했을때의 처리
   void SetCategory(int index)
    {
        ActionSelectionState.category = index;
        owner.ChangeState<ActionSelectionState>();
    }
}
