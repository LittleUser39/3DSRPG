using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//제너릭 타입 함수로 T0,T1,T2에 전달받은 대상을 저장하는 용도의 클래스
//공격자 타겟 타겟의 능력치를 저장하고 있다
public class Info<T0>
{
    // arg0 에 T0 저장
    public T0 arg0;
    public Info(T0 arg0)
    {
        this.arg0 = arg0;
    }
}
public class Info<T0, T1> : Info<T0>
{
    // arg1 에 T1 저장
    public T1 arg1;
    public Info(T0 arg0, T1 arg1) : base(arg0)
    {
        this.arg1 = arg1;
    }
}
public class Info<T0, T1, T2> : Info<T0, T1>
{
    // arg2 에 T2 저장
    public T2 arg2;
    public Info(T0 arg0, T1 arg1, T2 arg2) : base(arg0, arg1)
    {
        this.arg2 = arg2;
    }
}

