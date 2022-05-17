using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//어떤 스킬로 공격할지 결정하는 상태
//스킬 공격의 경우 화이트 공격,블랙공격 두분류
//각 분류 별로 3개의 스킬을 가지고 있음
//todo 나중에 스킬에 관한것 여기다 바꾸면 될듯?
public class ActionSelectionState : BaseAbilityMenuState
{
    public static int category;
    AbilityCatalog catalog;

    //해당 카테고리의 공격 종류들
    string[] whiteMagicOption = new string[]{"Cure", "Raise", "Holy"};
    string[] BlackMagicOption = new string[] { "Fire", "Ice", "Lightning" };

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
        //해당 턴의 대상의 능력 목록을 가져옴
        catalog = turn.actor.GetComponentInChildren<AbilityCatalog>();
        //게임오브젝트에 카테고리(능력 종류)를 가져와서 저장함
        GameObject container = catalog.GetCategory(category);
        menuTitle = container.name;

        int count = catalog.AbilityCount(container);
        if (menuOption == null)
        {
            menuOption = new List<string>(3);
        }
        else
        {
            menuOption.Clear();
        }
        
        bool[] locks = new bool[count];
        for(int i=0;i<count;++i)
        {
            //카테고리(스킬 종류)에 있는 능력을 가져와 저장
            Ability ability = catalog.GetAbility(category, i);
            //능력의 비용을 컴포넌트를 참고해서 저장
            AbilityMagicCost cost = ability.GetComponent<AbilityMagicCost>();
            if (cost)
                menuOption.Add(string.Format("{0}:{1}", ability.name, cost.amount));
            else
                menuOption.Add(ability.name);
            locks[i] = !ability.CanPerform();
        }

        ////해당 카테고리에 따라 공격 종류를 세팅
        //if (category == 0)
        //{
        //    menuTitle = "White Magic";
        //    SetOptions(whiteMagicOption);
        //}
        //else
        //{
        //    menuTitle = "Black Magic";
        //    SetOptions(BlackMagicOption);
        //}
        AbilityMenuPanelController.Show(menuTitle, menuOption);
        for (int i = 0; i < count; ++i)
            AbilityMenuPanelController.SetLocked(i, locks[i]);
    }

    //버튼선택
    protected override void Confirm()
    {
        //해당 턴의 기술을 선택하면
        //발동자의 상태를 기술을 사용하는 상태로 변경
        turn.ability = catalog.GetAbility(category, AbilityMenuPanelController.selection);
        owner.ChangeState<AbilityTargetState>();
        //turn.hasUnitActed = true;
        //if (turn.hasUnitMoved)
        //    turn.lockMove = true;
        //owner.ChangeState<CommandSelectionState>();
    }

    //취소하기
    protected override void Cancel()
    {
        owner.ChangeState<CategorySelectionState>();
    }

    //메뉴판 세팅
    void SetOptions(string[] options)
    {
        menuOption.Clear();
        for (int i = 0; i < options.Length; ++i)
            menuOption.Add(options[i]);
    }
}
