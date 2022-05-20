using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//어떤 능력을 누가 우엇에 사용할지 정하는 클래스?
public abstract class BaseAbilityPicker : MonoBehaviour
{
    protected Unit owner;
    protected AbilityCatalog ability;

    private void Start()
    {
        owner = GetComponentInParent<Unit>();
        ability = owner.GetComponentInChildren<AbilityCatalog>();
    }

    public abstract void Pick(PlanOfAttack plan);

    protected Ability Find(string abilityName)
    {
        for(int i=0; i<ability.transform.childCount;++i)
        {
            Transform category = ability.transform.GetChild(i);
            Transform child = category.FindChild(abilityName);
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
