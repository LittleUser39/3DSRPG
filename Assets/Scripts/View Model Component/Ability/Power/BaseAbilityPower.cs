using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbilityPower : MonoBehaviour
{
    protected abstract int GetBaseAttack();
    protected abstract int GetBaseDefense(Unit target);
    protected abstract int GetPower();
   
    // 델리게이트 등록
    void OnEnable()
    {
        this.AddObserver(OnGetBaseAttack, BaseAbilityEffect.GetAttackNotification);
        this.AddObserver(OnGetBaseDefense, BaseAbilityEffect.GetDefenseNotification);
        this.AddObserver(OnGetPower, BaseAbilityEffect.GetPowerNotification);
    }
    void OnDisable()
    {
        this.RemoveObserver(OnGetBaseAttack, BaseAbilityEffect.GetAttackNotification);
        this.RemoveObserver(OnGetBaseDefense, BaseAbilityEffect.GetDefenseNotification);
        this.RemoveObserver(OnGetPower, BaseAbilityEffect.GetPowerNotification);
    }
    void OnGetBaseAttack(object sender,object args)
    {
        if(IsMyEffect(sender))
        {
            var info = args as Info<Unit, Unit, List<ValueModifier>>;
            info.arg2.Add(new AddValueModifier(0, GetBaseAttack()));
        }
        //var info = args as Info<Unit, Unit, List<ValueModifier>>;
        //if (info.arg0 != GetComponentInParent<Unit>())
        //    return;
        //AddValueModifier mod = new AddValueModifier(0, GetBaseAttack());
        //info.arg2.Add(mod);
    }
    void OnGetBaseDefense(object sender,object args)
    {
        if (IsMyEffect(sender))
        {
            var info = args as Info<Unit, Unit, List<ValueModifier>>;
            info.arg2.Add(new AddValueModifier(0, GetBaseDefense(info.arg1)));
        }
        //var info = args as Info<Unit, Unit, List<ValueModifier>>;
        //if (info.arg0 != GetComponentInParent<Unit>())
        //    return;

        //AddValueModifier mod = new AddValueModifier(0, GetBaseDefense(info.arg1));
        //info.arg2.Add(mod);
    }
    void OnGetPower(object sender,object args)
    {
        if (IsMyEffect(sender))
        {
            var info = args as Info<Unit, Unit, List<ValueModifier>>;
            info.arg2.Add(new AddValueModifier(0, GetPower()));
        }
        //var info = args as Info<Unit, Unit, List<ValueModifier>>;
        //if(info.arg0 != GetComponentInParent<Unit>())
        //    return;

        //AddValueModifier mod = new AddValueModifier(0, GetPower());
        //info.arg2.Add(mod);
    }
    bool IsMyEffect(object sender)
    {
        MonoBehaviour obj = sender as MonoBehaviour;
        return (obj != null && obj.transform.parent == transform);
    }
}
