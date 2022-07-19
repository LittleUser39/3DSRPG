﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//어떤 능력을 누가 무엇에 사용할지 정하는 클래스?
public abstract class BaseAbilityPicker : MonoBehaviour
{
    protected Unit owner;
    protected AbilityCatalog ac;

    private void Start()
    {
        owner = GetComponentInParent<Unit>();
        ac = owner.GetComponentInChildren<AbilityCatalog>();
    }

    public abstract void Pick(PlanOfAttack plan);

    protected Ability Find(string abilityName)
    {
        for(int i = 0; i < ac.transform.childCount;++i)
        {
            Transform category = ac.transform.GetChild(i);
            Transform child = category.Find(abilityName);
            if(child!=null)
            {
                return child.GetComponent<Ability>();
            }
        }
        return null;
    }
    protected Ability Default()
    {
        return owner.GetComponentInChildren<Ability>();
    }
}
