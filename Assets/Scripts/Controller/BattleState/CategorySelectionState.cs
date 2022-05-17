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
        //메뉴 옵션이 null이 면
        if(menuOption==null)
        {
            //메뉴 옵션에 새로운 문자열 리스트 생성
            menuOption = new List<string>();
        }
        else
        {
            menuOption.Clear();
        }
        //제목은 action
        //Attack 옵션 추가
        menuTitle = "ACtion";
        menuOption.Add("Attack");

        //능력 목록을 해당 턴의 캐릭터 컴포넌트 참조
        //능력 전체를 이름으로 옵션추가
        AbilityCatalog catalog = turn.actor.GetComponentInChildren<AbilityCatalog>();
        for(int i=0; i< catalog.CategoryCount();++i)
        {
            menuOption.Add(catalog.GetCategory(i).name);
        }
        //능력 선택하는 UI 보이기
        AbilityMenuPanelController.Show(menuTitle, menuOption);
    }

    protected override void Confirm()
    {
        ////버튼을 선택했을 때의 처리
        //switch(AbilityMenuPanelController.selection)
        //{
        //    case 0:
        //        Attack();
        //        break;
        //    case 1:
        //        SetCategory(0);
        //        break;
        //    case 2:
        //        SetCategory(1);
        //        break;
        //}

        //변경
        //첫번째 버튼을 선택하면 공격
        //아니면 능력
        if (AbilityMenuPanelController.selection == 0)
            Attack();
        else
            SetCategory(AbilityMenuPanelController.selection - 1);
    }

    protected override void Cancel()
    {
        owner.ChangeState<CommandSelectionState>();
    }

    void Attack()
    {
        //turn.ability = turn.actor.GetComponentInChildren<AbilityRange>().gameObject;
        turn.ability = turn.actor.GetComponentInChildren<Ability>();
        owner.ChangeState<AbilityTargetState>();

        //abilitytargetstate으로 이동
        ////공격 했음으로 체크
        //turn.hasUnitActed = true;

        //if(turn.hasUnitMoved)
        //{
        //    //이미 이동을 한 상태이면 lockMove도 true 처리
        //    turn.lockMove = true;
        //}

        ////명령대기 상태로 돌아감
        //owner.ChangeState<CommandSelectionState>();
    }

    //특수 공격을 했을때의 처리
   void SetCategory(int index)
    {
        ActionSelectionState.category = index;
        owner.ChangeState<ActionSelectionState>();
    }
}
