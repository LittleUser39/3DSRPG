using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 타겟을 처치하면 아군이 승리하는 조건인 클래스
public class DefeatTargetVictoryCondition : BaseVictoryCondition
{
    public Unit target;
    
    protected override void CheckForGameOver()
    {
        base.CheckForGameOver();
        if(Victor==Alliances.None&&IsDefeated(target))
        {
            Victor = Alliances.Hero;
        }
    }
}
