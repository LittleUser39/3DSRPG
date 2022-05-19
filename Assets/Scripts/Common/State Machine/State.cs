using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//다른 클래스에서 상속받기 위해 만든 클래스 골격같은 존재로 추상화 클래스 이다.
public abstract class State : MonoBehaviour
{
    //상태가 시작시 호출
    public virtual void Enter()
    {
        AddListeners();
    }

    //상태가 종료될때 호출
    public virtual void Exit()
    {
        RemoveListeners();
    }

    //안전하게 listener을 제거하기 위한 용도
    protected virtual void OnDestroy()
    {
        RemoveListeners();
    }

    //이벤트 핸들러에 이벤트를 추가
    protected virtual void AddListeners()
    {

    }

    //이벤트 핸들러에 이벤트를 제거
    protected virtual void RemoveListeners()
    {

    }
}
