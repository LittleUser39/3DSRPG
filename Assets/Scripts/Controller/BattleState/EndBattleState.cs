using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//전투가 끝났을때 나오는 상태 클래스
//todo 나중에 전투가 끝났을때 경험치나 아이템등을 얻은 것을 띄워주어야함
//todo 팀을 설정하거나 다른 스테이지를 선택하는 게임의 다른 장면으로 이동해야함
public class EndBattleState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        //이거 바꿔야함
        Application.LoadLevel(0);
    }
}
