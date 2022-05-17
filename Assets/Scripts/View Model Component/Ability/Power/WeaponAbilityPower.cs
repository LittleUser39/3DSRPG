﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAbilityPower : BaseAbilityPower
{
    protected override int GetBaseAttack()
    {
        return GetComponentInParent<Stats>()[StateTypes.ATK];
    }
    protected override int GetBaseDefense(Unit target)
    {
        return target.GetComponent<Stats>()[StateTypes.DEF];
    }
    protected override int GetPower()
    {
        int power = PowerFromEquippedWeapon();

        return power > 0 ? power : UnarmedPower();
    }
    int PowerFromEquippedWeapon()
    {
        int power = 0;
        Equipment eq = GetComponentInParent<Equipment>();
        Equippable item = eq.GetItem(EquipSlots.Primary);
        StatModifierFeature[] features = item.GetComponentsInChildren<StatModifierFeature>();

        for(int i=0; i<features.Length;++i)
        {
            if (features[i].type == StateTypes.ATK)
                power += features[i].amount;
        }
        return power;
    }
    int UnarmedPower()
    {
        Job job = GetComponentInParent<Job>();
        for (int i = 0; i < Job.statOrder.Length; ++i)
        {
            if (Job.statOrder[i] == StateTypes.ATK)
            {
                return job.baseStats[i];
            }
        }
        return 0;
    }
    

}
