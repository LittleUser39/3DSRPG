using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//상태효과 - 실명같은거인듯
public class BlindStatusEffect : MonoBehaviour
{
    void OnEnable()
    {
        // HitRate.StatusCheckNotification 키로 OnHitRateStatusCheck 등록
        this.AddObserver(OnHitRateStatusCheck, HitRate.StatusCheckNotification);
    }

    void OnDisable()
    {
        this.RemoveObserver(OnHitRateStatusCheck, HitRate.StatusCheckNotification);
    }


    void OnHitRateStatusCheck(object sender, object args)
    {
        Info<Unit, Unit, int> info = args as Info<Unit, Unit, int>;

        // Unit 클래스를 참조합니다.
        Unit owner = GetComponentInParent<Unit>();

        // 자기 자신에게 사용하는 공격이라면 
        if (owner == info.arg0)
        {
            //  회피 또는 저항 능력 50 증가
            info.arg2 += 50;
        }

        // 다른 대상에게 사용하는 공격이라면
        else if (owner == info.arg1)
        {
            // 회피 또는 저항 능력 20 감소
            info.arg2 -= 20;
        }
    }
}

