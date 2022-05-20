using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanOfAttack
{
    //AI능력
    public Ability ability;
    //AI 공격대상
    public Targets targets;
    //AI 움직일 장소
    public Point moveLocation;
    //AI 공격할 장소
    public Point fireLocation;
    //AI 공격 범위
    public Directions attackDirection;
}
