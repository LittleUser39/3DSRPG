using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//능력치 리스트 열거형
//enum 정수 숫자 형식의 명명된 상수 집합에 의해 정의되는 값형식
//todo 나중에 여기좀 바꿔야 할듯
public enum StateTypes
{
    LVL, // Level 레벨
    EXP, // Experience 경험치
    HP,  // Hit Points 체력
    MHP, // Max Hit Points 전체 체력
    MP,  // Magic Points    마나
    MMP, // Max Magic Points    전체 마나
    ATK, // Physical Attack  물리공격력
    DEF, // Physical Defense    물리 방어력
    MAT, // Magic Attack    마법 공격력
    MDF, // Magic Defense   마법 방어력
    EVD, // Evade          방어확률
    RES, // Status Resistance 회피 확률
    SPD, // Speed   속도
    MOV, // Move Range  이동 범위
    JMP, // Jump Height 점프 범위
    CTR, // Counter-for turn order
    Count
}

