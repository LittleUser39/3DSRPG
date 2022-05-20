using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//AI의 공격 패턴을 정하는 클래스
public class AttackPattern : MonoBehaviour
{
    public List<BaseAbilityPicker> pickers;
    int index;

    public void Pick(PlanOfAttack plan)
    {
        pickers[index].Pick(plan);
        index++;
        if (index >= pickers.Count)
            index = 0;
    }
}
