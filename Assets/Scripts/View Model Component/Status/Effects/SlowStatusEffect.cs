using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowStatusEffect : StatusEffect
{
    Stats myStats;
    private void OnEnable()
    {
        myStats = GetComponent<Stats>();

        if (myStats)
            this.AddObserver(OnCounterWillChange, Stats.WillChangeNotification(StateTypes.AP), myStats);
    }
    private void OnDisable()
    {
        this.RemoveObserver(OnCounterWillChange, Stats.WillChangeNotification(StateTypes.AP), myStats);
    }
    void OnCounterWillChange(object sender,object args)
    {
        ValueChangeException exc = args as ValueChangeException;
        //CTR이 증가되어야 할 값이 반으로 줄어듬
        MultDeltaModifier m = new MultDeltaModifier(0, 0.5f);
        exc.AddModifier(m);
    }
}
