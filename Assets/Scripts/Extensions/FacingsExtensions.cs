using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FacingsExtensions 
{
public static Facing GetFacing(this Unit attacker,Unit target)
    {
        //타겟이 바라보는 방향
        Vector2 targetDirection = target.dir.GetNormal();

        //공격자 기준으로 타겟이 있는 방향
        Vector2 approachDirection = ((Vector2)(target.tile.pos - attacker.tile.pos)).normalized;
        //두 벡터의 내적
        //반환값이 +1이면 같은방향
        //-1이면 다른방향
        //0이면 180도
        float dot = Vector2.Dot(approachDirection, targetDirection);
        //공격자가 타겟 뒤에 있다.
        if (dot >= 0.45f) return Facing.back;
        //공격자가 타겟 앞에 있다
        if (dot <= -0.45f) return Facing.front;
        //공격자가 타겟 옆에 있다.
        return Facing.side;
    }
}
