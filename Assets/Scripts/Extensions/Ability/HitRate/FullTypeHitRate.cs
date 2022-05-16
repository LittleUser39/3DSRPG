using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//무적등의 상태가 아니면 무조건 적용되는 공격 스타일
public class FullTypeHitRate : HitRate
{
    public override int Calculate(Unit attacker, Unit target)
    {
        //AutomaticMiss에서 true가 반환 되지 않으면 무조건 적용되는 타입
        if (AutomaticMiss(attacker, target)) return Final(100);
        return Final(0);
    }
}
