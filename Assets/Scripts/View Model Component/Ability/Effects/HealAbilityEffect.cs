using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbilityEffect : BaseAbilityEffect
{
    public override int Predict(Tile target)
    {
        Unit attacker = GetComponentInParent<Unit>();
        Unit defender = target.content.GetComponent<Unit>();
        return GetStat(attacker, defender, GetPowerNotification, 0);
    }
    public override int OnApply(Tile target)
    {
        Unit defender = target.content.GetComponent<Unit>();

        int value = Predict(target);

        value = Mathf.FloorToInt(value * UnityEngine.Random.Range(0.9f, 1.1f));

        value = Mathf.Clamp(value, minDamage, maxDamage);

        Stats stats = defender.GetComponent<Stats>();
        stats[StateTypes.HP] += value;
        return value;
    }
}
