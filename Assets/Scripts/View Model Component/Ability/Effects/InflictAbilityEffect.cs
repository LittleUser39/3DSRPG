using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

//대상에게 걸린 효과의 지속시간을 정해주는 클래스
public class InflictAbilityEffect : BaseAbilityEffect
{
    public string statusName;
    public int duration;
    public override int Predict(Tile target)
    {
        return 0;
    }
    public override int OnApply(Tile target)
    {
        //Reflection의 기능
        //스트링값으로 클래스를 찾아냄
        Type statusType = Type.GetType(statusName);
    
        if(statusType==null||!statusType.IsSubclassOf(typeof(StatusEffect)))
        {
            Debug.LogError("Invalid Status Type");
            return 0;
        }

        MethodInfo mi = typeof(Status).GetMethod("Add");
        Type[] types = new Type[] { statusType, typeof(DurationStatusCondition) };

        MethodInfo constructed = mi.MakeGenericMethod(types);

        Status status = target.content.GetComponent<Status>();

        object retValue = constructed.Invoke(status, null);

        DurationStatusCondition condition = retValue as DurationStatusCondition;
        condition.duration = duration;
        return 0;
    }
}
