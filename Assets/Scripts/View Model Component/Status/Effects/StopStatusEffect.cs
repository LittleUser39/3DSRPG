using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopStatusEffect : StatusEffect
{
    Stats myStats;
    private void OnEnable()
    {
        myStats = GetComponent<Stats>();
        if (myStats)
            this.AddObserver(OnCounterWillChange, Stats.WillChangeNotification(StateTypes.AP), myStats);
        this.AddObserver(OnAutomaticHitCheck, HitRate.AutomaticHitCheckNotification);
    }
    private void OnDisable()
    {
        this.RemoveObserver(OnCounterWillChange, Stats.WillChangeNotification(StateTypes.AP), myStats);
    }
    void OnCounterWillChange(object sender,object args)
    {
        ValueChangeException exc = args as ValueChangeException;
        //CTR 증가 계산을 처리하지 않게됨
        exc.FlipToggle();
    }
    void OnAutomaticHitCheck(object sender,object args)
    {
        //unit 클래스를 참조
        Unit owner = GetComponentInParent<Unit>();

        MatchException exc = args as MatchException;

        //타겟이 owner와 동일하면 exc의 값을 true 변경
        if (owner == exc.target)
            exc.FlipToggle();
    }
}
