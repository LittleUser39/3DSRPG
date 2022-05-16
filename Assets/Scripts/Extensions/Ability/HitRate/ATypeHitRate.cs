using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATypeHitRate : HitRate
{
    public override int Calculate(Unit attacker, Unit target)
    {
        //회피 할 수 없는 공격(최종 회피력 0)
        if (AutomaticHit(attacker, target)) return Final(0);
        //무조건 회피되는 공격(최종 회피력 100)
        if (AutomaticMiss(attacker, target)) return Final(100);
        //타겟의 회피능력치 참조
        int evade = GetEvade(target);
        //타겟과 공격자의 방향에 따라 타겟의 회피 능력치 적용
        evade = AdjustForRelativeFacing(attacker, target, evade);
        //공격자,타겟 그리고 타겟의 능력치로 회피력을 구함
        evade = AdjustForStatusEffects(attacker, target, evade);
        //타겟의 최종 회피력을 5와95 범위를 벗어나지 않는다.
        evade = Mathf.Clamp(evade, 5, 95);
        //100-타겟의 최종 회피력
        return Final(evade);
    }
    int GetEvade(Unit target)
    {
        //회피 능력치 반환
        //회피 능력치는 0또는 100범위를 벗어나지 않는다
        Stats stats = target.GetComponentInParent<Stats>();

        return Mathf.Clamp(stats[StateTypes.EVD], 0, 100);
    }
    int AdjustForRelativeFacing(Unit attacker, Unit target, int rate)
    {
        switch(attacker.GetFacing(target))
        {
            //공격자가 타겟 앞에 있으면
            //타겟의 회피 능력치가 그대로 적용
            case Facing.front:
                return rate;
                //공격자가 타겟 옆에 있으면
                //타겟의 회피능력치가 2배 낮아짐
            case Facing.side:
                return rate / 2;
                //공격자가 타겟 뒤에 있으면
                //타겟의 회피능력치가 4배 낮아짐
            default:
                return rate / 4;
        }
    }
}
