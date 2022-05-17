using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//언데드를 대상으로 하는 스킬
public class UndeadAbilityEffectTarget : AbilityEffectTarget
{
    public bool toggle;

    public override bool IsTarget(Tile tile)
    {
        //타일이 없거나 대상이 없으면 false
        if (tile == null || tile.content == null)
            return false;

        //타일의 대상이 undead 컴포넌트가 있는지 확인
        //없으면 false
        bool hasComponent = tile.content.GetComponent<UnDead>() != null;
        if (hasComponent != toggle)
            return false;

        Stats stats = tile.content.GetComponent<Stats>();
        return stats != null && stats[StateTypes.HP] > 0;
    }
}
