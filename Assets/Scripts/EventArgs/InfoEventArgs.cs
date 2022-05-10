using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//제너릭 c++의 템플릿과 비슷하다
//클래스의 이름을 정의할때 데이터 타입을 확정하지 않고 만들어 사용시에 특정 타입을 지정하게 만드는것
public class InfoEventArgs<T> : EventArgs
{
    public T info;

    public InfoEventArgs()
    {
        info = default(T);
        Debug.Log(info.ToString());
    }

    public InfoEventArgs(T info)
    {
        this.info = info;
    }
}
