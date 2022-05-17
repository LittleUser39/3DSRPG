using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STypeHitRate : HitRate
{
    public override int Calculate(Tile target)
    {
        Unit defender = target.content.GetComponent<Unit>();
        if (AutomaticMiss(defender)) return Final(100);
        if (AutomaticHit(defender)) return Final(0);

        // 타겟의 저항능력치 참조
        int res = GetResistance(defender);

        // 공격자, 타겟 그리고 타겟의 능력치로 저항력을 구한다.
        res = AdjustForStatusEffects(defender, res);

        // 타겟과 공격자의 방향에 따라 타겟의 저항 능력치 적용
        res = AdjustForRelativeFacing(defender, res);

        // 타겟의 최종 저항능력치는 0 또는 100을 벗어날 수 없다.
        res = Mathf.Clamp(res, 0, 100);

        // 100 - 타겟의 최종 저항력 반환
        return Final(res);
    }
    int GetResistance(Unit target)
    {
        Stats s = target.GetComponentInParent<Stats>();

        // 타겟의 저항능력 반환
        return s[StateTypes.RES];
    }
    int AdjustForRelativeFacing(Unit target, int rate)
    {
        switch (attacker.GetFacing(target))
        {
            // 공격자가 타겟 앞에 있으면
            // 타겟의 저항 능력치가 그대로 적용된다.
            case Facing.front:
                return rate;

            // 공격자가 타겟 옆에 있으면
            // 타겟의 저항능력치가 10 낮아진다.
            case Facing.side:
                return rate - 10;

            // 공격자가 타겟 뒤에 있으면 
            // 타겟의 저항능력치가 20 낮아진다.
            default:
                return rate - 20;
        }
    }
}

