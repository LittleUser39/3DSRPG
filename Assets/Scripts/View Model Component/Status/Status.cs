using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//StatusEffect는 상태이상 효과
//statusCondition은 상태가 지속되기 위한 조건
public class Status : MonoBehaviour
{
    //키값
    public const string AddedNotification = "Status.AddedNotification";
    public const string RemovedNotification = "Status.RemovedNotification";

    //T는 StatusEffect(효과),U는 StatusCondition(조건)을 매개변수로 받으며
    //StatusCondition 타입을 반환
    public U Add<T, U>() where T:StatusEffect where U:StatusCondition
    {
        //자식 오브젝트에 부착된 StatusEffect 타입을 참조
        T effect = GetComponentInChildren<T>();

        //effect 가 null
        if(effect==null)
        {
            //effect에 T를 추가
            effect = gameObject.AddChildComponent<T>();
            //AddedNotification키로 등록된 델리게이트 호출
            this.PostNotification(AddedNotification, effect);
        }

        return effect.gameObject.AddChildComponent<U>();
    }
    public void Remove(StatusCondition target)
    {
        //target의 부모오브젝트에서 statuseffect를 참조
        StatusEffect effect = target.GetComponentInParent<StatusEffect>();

        //statusEffect의 부모오브젝트와 부모자식 관계 해제
        target.transform.SetParent(null);
        //target.gameobject 삭제
        Destroy(target.gameObject);

        //effect의 자식에게서 statuscondition을 참조
        StatusCondition condition = effect.GetComponentInChildren<StatusCondition>();
        if(condition==null)
        {
            //effect의 부모오브젝트와 부모관계 해제
            effect.transform.SetParent(null);
            //effect의 삭제
            Destroy(effect.gameObject);
            //removedNotification 키로 등록된 델리게이트 호출
            this.PostNotification(RemovedNotification,effect);
        }
    }
}
