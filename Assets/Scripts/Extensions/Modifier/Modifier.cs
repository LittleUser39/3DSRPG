using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//추상화 클래스
//경험치,레벨등의 수치들의 계산에 필요한 클래스
public abstract class Modifier
{
    //const와 비슷하지만 값설정없이 선언만 해도 사용가능
    public readonly int sortOrder;
    public Modifier(int sortOrder)
    {
        this.sortOrder = sortOrder;
    }
}
