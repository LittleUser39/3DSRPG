using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    //현재 타입을 저장,불러올때 호출
    //currentstate의 속성
   public virtual State CurrentState
    {
        get { return _currentState; }
        set { Transition(value); }
    }
    protected State _currentState;
    protected bool _inTransition;

    //변경하려는 상태가 해당 게임오브젝트에 컴포넌트로 있는지 체크
    public virtual T GetState<T>()where T:State
    {
        T target = GetComponent<T>();

        //변경하려는 state가 게임오브젝트 없으면 추가
        if (target == null)
            target = gameObject.AddComponent<T>();

        return target;
    }

    //상태를 변경시킬때 호출
    public virtual void ChangeState<T>() where T : State
    {
        CurrentState = GetState<T>();
    }
    protected virtual void Transition(State value)
    {

        //현재상태와 변경하려는 상태가 같은지 확인, 상태가 변경중인지
        if (_currentState == value || _inTransition) return;
        _inTransition = true;

        //상태를 변경할 때 현재 상태의 state.exit호출
        if (_currentState != null) _currentState.Exit();

        //현재 상태 변경
        _currentState = value;

        //변경된 상태의 state.enter를 호출
        if (_currentState != null) _currentState.Enter();

        //변경이 완료되면 _intransition false로 변경
        _inTransition = false;
    }
}
