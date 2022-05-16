using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//경험치,레벨등을 계산하는 데 사용하는 클래스
public class ClampValueModifier : ValueModifier
{
    public readonly float min;
    public readonly float max;
    public ClampValueModifier(int sortOrder,float min,float max):base(sortOrder)
    {
        this.min = min;
        this.max = max;
    }

    //value가 min또는 max 범위를 넘어가지 않게 함

    public override float Modify(float fromValue,float toValue)
    {
        return Mathf.Clamp(toValue, min, max);
    }
}
