using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//적군을 전부 처치했을때 아군이 승리하는 조건 클래스
public class DefeatAllEnemiesVictoryCondition : BaseVictoryCondition
{
    protected override void CheckForGameOver()
    {
        base.CheckForGameOver();
        if(Victor==Alliances.None&&PartyDefeated(Alliances.Enemy))
        {
            Victor = Alliances.Hero;
        }

    }
}
