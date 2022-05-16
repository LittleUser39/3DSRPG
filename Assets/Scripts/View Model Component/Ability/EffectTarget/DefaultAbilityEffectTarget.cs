using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//살아있는 대상에게만 사용할수 있음
public class DefaultAbilityEffectTarget : AbilityEffectTarget
{
    public override bool IsTarget(Tile tile)
    {
        //타일이 존재하는지, 유닛이 존재하는지 
        if (tile == null || tile.content == null)
            return false;

        //능력치가 있는 대상이고 대상이 살아있으면 true 반환
        Stats stats = tile.content.GetComponent<Stats>();
        return stats != null && stats[StateTypes.HP] > 0;
    }
}
