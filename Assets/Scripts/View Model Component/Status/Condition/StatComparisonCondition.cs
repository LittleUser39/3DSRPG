using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//상태이상에 걸려서 만약 죽으면 그것을 비교하는 클래스?
//hp 통계가 변경될때 마다 스크립트가 hp가 0인지 여부를 확인
public class StatComparisonCondition : StatusCondition
{
    public StateTypes type { get; private set; }
    public int value { get; private set; }
    public Func<bool> conditon { get; private set; }
    Stats stats;
    private void Awake()
    {
        stats = GetComponentInParent<Stats>();
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnStatChanged, Stats.DidChangeNotification(type), stats);
    }
    public void Init(StateTypes type,int value,Func<bool>condition)
    {
        this.type = type;
        this.value = value;
        this.conditon = conditon;
        this.AddObserver(OnStatChanged, Stats.DidChangeNotification(type), stats);
    }
    public bool EqualTo()
    {
        return stats[type] == value;
    }
    public bool LessThan()
    {
        return stats[type] < value;
    }
    public bool LessThanOrEqualTo()
    {
        return stats[type] <= value;
    }
    public bool GreaterThan()
    {
        return stats[type] > value;
    }
    public bool GreaterThanOnEqualTo()
    {
        return stats[type] >= value;
    }

    void OnStatChanged(object sender,object args)
    {
        if(conditon!=null&&!conditon())
        {
            Remove();
        }
    }
}
