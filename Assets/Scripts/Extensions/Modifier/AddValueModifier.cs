using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//경험치,레벨등의 계산들을 할때 사용되는 클래스
public class AddValueModifier : ValueModifier
{
    public readonly float toAdd;
    public AddValueModifier(int sortOrder,float toAdd):base(sortOrder)
    {
        this.toAdd = toAdd;
    }

    //두 수치를 더한 값을 반환
    public override float Modify(float value)
    {
        return value + toAdd;
    }
}
