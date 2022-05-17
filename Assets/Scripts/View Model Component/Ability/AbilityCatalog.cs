using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//능력(스킬)목록 클래스
//게임 오브젝트의 특정 계층
//이 스크립트를 가진 게임오브젝트의 모든 자식은 능력범주로 간주
public class AbilityCatalog : MonoBehaviour
{
  public int CategoryCount()
    {
        return transform.childCount;
    }
    public GameObject GetCategory(int index)
    {
        if (index < 0 || index >= transform.childCount)
            return null;
        return transform.GetChild(index).gameObject;
    }
    public int AbilityCount(GameObject category)
    {
        return category != null ? category.transform.childCount : 0;
    }
    public Ability GetAbility(int categoryIndex,int abilityIndex)
    {
        GameObject category = GetCategory(categoryIndex);
        if (category == null || abilityIndex < 0 || abilityIndex >= category.transform.childCount)
            return null;
        return category.transform.GetChild(abilityIndex).GetComponent<Ability>();
    }
}
