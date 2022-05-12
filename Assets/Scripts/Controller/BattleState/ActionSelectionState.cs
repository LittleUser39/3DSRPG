using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//스킬 공격의 경우 화이트 공격,블랙공격 두분류
//각 분류 별로 3개의 스킬을 가지고 있음
//todo 나중에 스킬에 관한것 여기다 바꾸면 될듯?
public class ActionSelectionState : BaseAbilityMenuState
{
    public static int category;

    //해당 카테고리의 공격 종류들
    string[] whiteMagicOption = new string[]{"Cure", "Raise", "Holy"};
    string[] BlackMagicOption = new string[] { "Fire", "Ice", "Lightning" };

    protected override void LoadMenu()
    {
        if (menuOption == null) menuOption = new List<string>(3);

        //해당 카테고리에 따라 공격 종류를 세팅
        if(category==0)
        {
            menuTitle = "White Magic";
            SetOptions(whiteMagicOption);
        }
        else
        {
            menuTitle = "Black Magic";
            SetOptions(BlackMagicOption);
        }
        AbilityMenuPanelController.Show(menuTitle, menuOption);
    }

    //버튼선택
    protected override void Confirm()
    {
        turn.hasUnitActed = true;
        if (turn.hasUnitMoved)
            turn.lockMove = true;
        owner.ChangeState<CommandSelectionState>();
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
