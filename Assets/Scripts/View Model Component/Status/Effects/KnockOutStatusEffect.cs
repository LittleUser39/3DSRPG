using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//유닛이 죽은 상태(죽은 애니메이션도 재생)
//턴이 주어지지 않는다
public class KnockOutStatusEffect : StatusEffect
{
    Unit owner;
    Stats stats;
    private void Awake()
    {
        owner = GetComponentInParent<Unit>();
        stats = owner.GetComponent<Stats>();
    }
    private void OnEnable()
    {
        owner.transform.localScale = new Vector3(0.75f, 0.1f, 0.75f);
        this.AddObserver(OnTurnCheck, TurnOrderController.TurnCheckNotification, owner);
        this.AddObserver(OnStatCountWillChange, Stats.WillChangeNotification(StateTypes.CTR), stats);
    }
    private void OnDisable()
    {
        owner.transform.localScale = Vector3.one;
        this.RemoveObserver(OnTurnCheck, TurnOrderController.TurnCheckNotification, owner);
        this.RemoveObserver(OnStatCountWillChange, Stats.WillChangeNotification(StateTypes.CTR), stats);
    }
    void OnTurnCheck(object sender,object args)
    {
        BaseException exc = args as BaseException;
        if(exc.defaultToggle == true)
        {
            exc.FlipToggle();
        }
    }
    void OnStatCountWillChange(object sender,object args)
    {
        ValueChangeException exc = args as ValueChangeException;
        if(exc.toValue>exc.fromValue)
        {
            exc.FlipToggle();
        }
    }
}
