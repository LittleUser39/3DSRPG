using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//전투가 끝났을때 나오는 상태 클래스
//나중에 전투가 끝났을때 경험치나 아이템등을 얻은 것을 띄워주어야함
//팀을 설정하거나 다른 스테이지를 선택하는 게임의 다른 장면으로 이동해야함
public class EndBattleState : BattleState
{
    
    public override void Enter()
    {
        base.Enter();

        ExperienceManger.AwardExperience(stageEXP, units);
        ExperienceManger.LogParty(units);
        for(int i=0;i < units.Count;++i)
        {
            units[i].GetComponent<Animator>().SetBool("Die", false);
        }
        //전투가 끝난후 전투한 유닛들 넣어둔 리스트 클리어
        //이거 넣어서 오류 수정함 비워줘야했음
        units.Clear();
       
        //이거 바꿔야함
       Application.LoadLevel(0);
    }
}
