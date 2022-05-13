using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 특정 능력치를 감소시키거나 증가시키는 역활을 함
//Onapply와 OnRemove는 꼭 구현해야함
public class StatModifierFeature : Feature
{
    //능력치 종류 (enum)
    public StateTypes type;

    public int amount;
    Stats stats
    {
        get
        {
            return _target.GetComponentInParent<Stats>();
        }
    }

    protected override void OnApply()
    {
        //amount 만큼 해당 능력치 증가
        //인덱서로 특정 능력치를 증가
        stats[type] += amount;
    }
    protected override void OnRemove()
    {
        stats[type] -= amount;
    }
}
