using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//능력치에 영향을 주는 스킬
public class HasteStatusEffect : StatusEffect
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

        //CTR은 턴속도에 영향
        //CRT을 두배 빠르게 충전시킴
        MultDeltaModifier m = new MultDeltaModifier(0, 2);
        exc.AddModifier(m);
    }
}
