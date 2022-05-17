using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//소생 능력 효과
//언데드는 피해받고 다른것은 치유되고
//죽은 유닛은 부활한다
public class ReviveAbilityEffect : BaseAbilityEffect
{
    public float percent;
    public override int Predict(Tile target)
    {
        Stats stats = target.content.GetComponent<Stats>();
        return Mathf.FloorToInt(stats[StateTypes.MHP] *percent);
    }
    public override int OnApply(Tile target)
    {
        Stats stats = target.content.GetComponent<Stats>();
        int value = stats[StateTypes.HP] = Predict(target);
        return value;
    }
}
