using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//경험치,레벨등 계산에 사용하기 위한 클래스
public class MaxValueModifier : ValueModifier
{
    public float max;
    public MaxValueModifier(int sortOrder,float max):base(sortOrder)
    { this.max = max; }

    public override float Modify(float fromValue,float toValue)
    {
        //두개의 값 중 큰값을 반환
        return Mathf.Max(fromValue, max);
    }
}
