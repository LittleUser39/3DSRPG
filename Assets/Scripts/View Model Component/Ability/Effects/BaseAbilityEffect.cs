using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbilityEffect : MonoBehaviour
{
    // 델리게이트 키 리스트
    public const string GetAttackNotification = "DamageAbilityEffect.GetAttackNotification";
    public const string GetDefenseNotification = "DamageAbilityEffect.GetDefenseNotification";
    public const string GetPowerNotification = "DamageAbilityEffect.GetPowerNotification";
    public const string TweakDamageNotification = "DamageAbilityEffect.TweakDamageNotification";

    public const string MissedNotification = "DamageAbilityEffect.MissedNotification";
    public const string HitNotification = "DamageAbilityEffect.HitNotification";
    // 최대 최소 피해 범위
    protected const int minDamage = -999;
    protected const int maxDamage = 999;
    public abstract int Predict(Tile target);
    public void Apply(Tile target)
    {
        if (GetComponent<AbilityEffectTarget>().IsTarget(target) == false)
            return;
        if (GetComponent<HitRate>().RollForHit(target))
            this.PostNotification(HitNotification, OnApply(target));
        else
            this.PostNotification(MissedNotification);
    }
    public abstract int OnApply(Tile target);
    protected virtual int GetStat(Unit attacker, Unit target, string notification, int startValue)
    {
        var mods = new List<ValueModifier>();

        var info = new Info<Unit, Unit, List<ValueModifier>>(attacker, target, mods);

        this.PostNotification(notification, info);

        mods.Sort();

        float value = startValue;

        for (int i = 0; i < mods.Count; ++i)
        {
            value = mods[i].Modify(startValue, value);
        }
        int retValue = Mathf.FloorToInt(value);

        retValue = Mathf.Clamp(retValue, minDamage, maxDamage);

        return retValue;
    }
}
