using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//언제든지 이를 복사하고 수정하여 다시 처음부터 시작할수 있다
//에셋을 생성
public class YourClassAsset
{
    // 유니티 상단 메뉴의 아래 경로에 
    // 아이템(버튼)을 추가시킨다.
    [MenuItem("Assets/Create/Conversation Data")]
    public static void CreateConversationData()
    {
        // 버튼이 눌리면 ScriptableObjectUtility.CreateAsset<T>를
        // 호출한다.
        // 영웅 데이터를 생성
        ScriptableObjectUtility.CreateAsset<ConversationData>();
    }

    [MenuItem("Assets/Create/Unit Recipe")]
    public static void CreateUnitRecipe()
    {
        //유닛 레시피 생성
        ScriptableObjectUtility.CreateAsset<UnitRecipe>();
    }
    
    [MenuItem("Assets/Create/Ability Catalog Recipe")]
    public static void CreateAbilityCatalogRecipe()
    {
        //능력 목록 레시피 생성
        ScriptableObjectUtility.CreateAsset<AbilityCatalogRecipe>();
    }

}

