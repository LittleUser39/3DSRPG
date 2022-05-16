using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//기존 값 변경량을 계산후 tomultiply 값만큼 변경량을 증가 시켜줌
public class MultDeltaModifier : ValueModifier
{
    public readonly float toMultiply;

    //생성자
    public MultDeltaModifier(int sortOrder,float toMultiply):base(sortOrder)
    {
        this.toMultiply = toMultiply;
    }

    public override float Modify(float fromValue, float toValue)
    {
        //delta = 기존값 - 계산된값
        float delta = toValue - fromValue;

        //fromValue = 기존값 + delta * toMultiply
        return fromValue + delta * toMultiply;
    }
}
