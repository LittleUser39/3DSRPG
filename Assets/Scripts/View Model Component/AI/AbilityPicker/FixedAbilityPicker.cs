using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//사용할려는 기술을 고르는 클래스
//사용할려는 기술을 가져와서 저장함
public class FixedAbilityPicker : BaseAbilityPicker
{
    public Targets targets;
    public string ability;

    public override void Pick(PlanOfAttack plan)
    {
        plan.targets = targets;
        plan.ability = Find(ability);
    
        if(plan.ability==null)
        {
            plan.ability = Default();
            plan.targets = Targets.Foe;
        }
    }

}
