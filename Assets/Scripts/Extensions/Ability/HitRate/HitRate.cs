using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//공격자 타겟자,타겟자의 능력을 토대로 델리게이트 함수를 호출해서
//계산하고 다른 클래스로 그 값을 전달하는 역활
//공격이 맞을 확률을 계산하는 클래스인듯
public abstract class HitRate : MonoBehaviour
{
    //델리게이트 키 리스트
    public const string AutomaticHitCheckNotification = "HitRate.AutomaticHitCheckNotification";
    public const string AutomaticMissCheckNotification = "GitRate.AutomaticMissCheckNotification";
    public const string StatusCheckNotification = "HitRate.StatusCheckNotification";
    public abstract int Calculate(Unit attacker, Unit target);

    
    protected virtual bool AutomaticHit(Unit attacker,Unit target)
    {
        //공격자와 타겟을 저장
        MatchException exc = new MatchException(attacker, target);
        //automatichitcheck키로 된 함수를 호출 (exc 전달)
        this.PostNotification(AutomaticHitCheckNotification, exc);
        //exc에 저장된 toggle 값을 전달
        return exc.toggle;
    }
    protected virtual bool AutomaticMiss(Unit attacker,Unit target)
    {
        //공격자와 타겟을 저장
        MatchException exc = new MatchException(attacker, target);
        //automaticmiss 키로 된 함수를 호출(exc 전달)
        this.PostNotification(AutomaticMissCheckNotification, exc);
        //exc에 저장된 toggle 값을 전달
        return exc.toggle;
    }
    protected virtual int AdjustForStatusEffects(Unit attacker,Unit target,int rate)
    {
        //공격자,타겟,타겟의 능력(회피,저항 등)을 info 클래스의 전역 변수에 저장
        Info<Unit, Unit, int> args = new Info<Unit, Unit, int>(attacker, target, rate);
        //statuscheck 키로 된 함수를 호출 (args 전달)
        this.PostNotification(StatusCheckNotification, args);
        //타겟 정보를 반환
        return args.arg2;
    }
    //타겟의 최종능력치 계산
    protected virtual int Final(int evade)
    {
        //100-타겟의 능력치 반환
        return 100 - evade;
    }
}
