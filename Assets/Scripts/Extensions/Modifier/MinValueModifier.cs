using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//경험치,레벨등을 계산하기 위해 사용하는 클래스
public class MinValueModifier : ValueModifier
{
    public float min;
    public MinValueModifier(int sortOrder, float min) : base(sortOrder)
    {
        this.min = min;
    }
 
    public override float Modify(float formValue,float toValue)
    {
        // 두 개의 값중 작은 값을 반환
        return Mathf.Min(min, toValue);
    }
}

