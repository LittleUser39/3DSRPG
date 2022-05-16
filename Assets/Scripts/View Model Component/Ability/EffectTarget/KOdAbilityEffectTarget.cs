using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//죽어있는 대상만 선택할수 있음
public class KOdAbilityEffectTarget : AbilityEffectTarget
{
    public override bool IsTarget(Tile tile)
    {
        
        if (tile == null || tile.content == null)
            return false;

        //능력치가 있는 대상이고 대상이 죽은상태이면 true 반환
        Stats stats = tile.content.GetComponent<Stats>();
        return stats != null && stats[StateTypes.HP] <= 0;
    }
}
