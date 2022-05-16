using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//경험치,레벨등을 계산하기 위해 사용하는 클래스
public class MultValueModifier : ValueModifier
{
    public readonly float toMultiply;
    public MultValueModifier(int sortOrder, float toMultiply) : base(sortOrder)
    {
        this.toMultiply = toMultiply;
    }
    public override float Modify(float fromValue,float toValue)
    {
        // 두개의 값을 곱한다.
        return toValue * toMultiply;
    }
}

