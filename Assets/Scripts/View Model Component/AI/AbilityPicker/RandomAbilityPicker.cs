using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//랜덤으로 능력을 선택해주는 클래스
public class RandomAbilityPicker : BaseAbilityPicker
{
    public List<BaseAbilityPicker> pickers;

    public override void Pick(PlanOfAttack plan)
    {
        int index = Random.Range(0, pickers.Count);
        BaseAbilityPicker p = pickers[index];
        p.Pick(plan);
    }
}
