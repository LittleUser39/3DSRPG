using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//배틀 상태를 상속받은 추상화 클래스, 다른 클래스에서 상속받기 위한 용도로 만든 클래스
//메뉴판에 관련된 동작들을 정의
public abstract class BaseAbilityMenuState : BattleState
{
    protected string menuTitle;
    //해당 메뉴의 버튼 분류
    protected List<string> menuOption;
    public override void Enter()
    {
        base.Enter();
        SelectTile(turn.actor.tile.pos);

        //메뉴판을 호출
        LoadMenu();
    }
    public override void Exit()
    {
        base.Exit();

        //상태가 해제되면 메뉴판 숨김
        AbilityMenuPanelController.Hide();
    }
    protected override void OnFire(object Sender, InfoEventArgs<int> e)
    {
        //inputmanager에서 입력된 마우스의 번호가 e에 들어옴
        if (e.info == 0) Confirm();
        else Cancel();
    }
    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        //좌우 또는 상하로 메뉴판에 선택된 버튼을 변경
        //inputmanager에서 상화조우 입력에 따라 -1~1값을 e로 전달
        if (e.info.x > 0 || e.info.y < 0)
            AbilityMenuPanelController.Next();
        else
            AbilityMenuPanelController.Previous();
    }

    //해당 함수들을 자식에서 정의 하기 위해 추상화 클래스로 구현
    protected abstract void LoadMenu();
    protected abstract void Confirm();
    protected abstract void Cancel();
}
