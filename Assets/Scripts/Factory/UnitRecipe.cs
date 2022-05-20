using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//팩토리가 런타임에 로드할수 있는 리소스 이름
//
public class UnitRecipe : ScriptableObject
{
    public string modle;            //모델 
    public string job;              //직업
    public string attack;           //공격
    public string abilityCatalog;   //능력 종류
    public Locomotions locomotions; //이동 방식 (열거형)
    public Alliances alliances;     //동맹
    public string strategy;         //AI 전략
}
