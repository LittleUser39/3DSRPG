using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//추상 클래스
//아이템의 능력치를 더하거나 감소시킬 때 사용되는 클래스
//캐릭터 생성 삭제 OnApply 에서 능력치 적용등의 역활을 함
public abstract class Feature : MonoBehaviour
{
    protected GameObject _target { get; private set; }

    //캐릭터 생성
    public void Activate(GameObject target)
    {
        if(_target==null)
        {
            _target = target;
            OnApply();
        }
    }

    //캐릭터 삭제
    public void Deactivate()
    {
        if (_target != null)
        {
            OnRemove();
            _target = null;
        }
    }

    //해당 능력치 추가
    public void Apply(GameObject target)
    {
        _target = target;
        OnApply();
        _target = null;
    }

    //상속받은 애가 꼭 구현 해야함
    protected abstract void OnApply();
    //상속받은 애가 재정의 해서 사용할것임
    protected virtual void OnRemove() {}
}
