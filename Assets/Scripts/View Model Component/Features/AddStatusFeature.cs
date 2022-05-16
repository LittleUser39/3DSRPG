using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//효과와 조건을 추가하거나 삭제하는 코드
public abstract class AddStatusFeature<T>:Feature where T:StatusEffect
{
    StatusCondition StatusCondition;
    protected override void OnApply()
    {
        //부모오브젝트의 status를 참조
        Status status = GetComponentInParent<Status>();

        //효과와 조건을 추가
        StatusCondition = status.Add<T, StatusCondition>();
    }

    protected override void OnRemove()
    {
        if(StatusCondition!=null)
        {
            //statusCondition를 제거
            StatusCondition.Remove();
        }
    }
}
