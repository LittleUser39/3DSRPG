using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//유닛을 선택했을때 (Move,Action,Wait)중 고르는 상태
//
public class CommandSelectionState : BaseAbilityMenuState
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
    protected override void Cancel()
    {
        if (turn.hasUnitMoved && !turn.lockMove)
        {
            turn.UndoMove();
            AbilityMenuPanelController.SetLocked(0, false);
            SelectTile(turn.actor.tile.pos);
        }
        else
        {
            owner.ChangeState<ExploreState>();
        }
    }

    protected override void Confirm()
    {
        switch(AbilityMenuPanelController.selection)
        {
            case 0://Move
                owner.ChangeState<MoveTargetState>();
                break;
            case 1://Action
                owner.ChangeState<CategorySelectionState>();
                break;
            case 2://Wait
                owner.ChangeState<SelectUnitState>();
                break;
        }
    }

    //메뉴를 연다
    protected override void LoadMenu()
    {
        //메뉴의 옵션 세팅
        //todo여기서 메뉴창 버튼 세팅하면 될듯
        if(menuOption==null)
        {
            menuTitle = "Commands";
            menuOption = new List<string>(3);
            menuOption.Add("Move");
            menuOption.Add("Action");
            menuOption.Add("Wait");
        }

        //메뉴창을 연다
        AbilityMenuPanelController.Show(menuTitle, menuOption);

        //해당 캐릭터가 이번턴에서 이미 이동했는지,공격했는지 체크
        //버튼을 잠금상태로 만듬
        //한번의 턴 == 1번의 공격 and 1번이동
        //todo AP로 바꾸면 한턴에 여러번 공격할수 있기 때문에 손봐야할듯
        //이건 살짝 생각해 봐야할듯
        AbilityMenuPanelController.SetLocked(0, turn.hasUnitMoved);
        AbilityMenuPanelController.SetLocked(1, turn.hasUnitActed);
    }

}
