using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//쉬프트 연산 
[System.Flags]
//장착 슬롯의 종류들 
public enum EquipSlots
{
    None=0,
    Primary=1<<0,   //보통 무기
    Secondary=1<<1, //방패 or 보조무기
    Head=1<<2,      //헬멧,모자 등 머리
    Body=1<<3,      //갑옷
    Accessory=1<<4  //악세사리
}
