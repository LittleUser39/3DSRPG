using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//특정 적 들은 같은 스킬을 공유할것이다
//스킬을 다시 만들지 않고 이 공유레시피를 참조시킬수 있다 
public class AbilityCatalogRecipe : ScriptableObject
{
    [System.Serializable]
    public class Category
    {
        //해당 능력의 종류
        public string name;
        //그 능력의 종류에 있는 스킬의 목록을 저장하는 배열
        public string[] entries;

    }
    //해당 능력의 종류를 저장하는 배열 EX)WHITE,Black
    public Category[] categories;
}
