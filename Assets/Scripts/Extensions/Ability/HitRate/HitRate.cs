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
    public const string AutomaticMissCheckNotification = "HitRate.AutomaticMissCheckNotification";
    public const string StatusCheckNotification = "HitRate.StatusCheckNotification";
   
    protected Unit attacker;

    //공격자는 이미 자신의 정보를 가지고 있다
    //tile에 해당 유닛의 정보가 저장되어 있다
    //  public abstract int Calculate(Unit attacker, Unit target);
    public abstract int Calculate(Tile target);
    
    //각도에 따른 명중률
    public virtual bool IsAngleBased { get { return true; } }

    //명중하는 것을 랜덤으로 정해주는 함수인것 같음
    public virtual bool RollForHit(Tile target)
    {
        //롤을 랜덤으로 정하고 찬스는 타겟의 능력치를 가져와서 
        //롤이 낮으면 true 높으면 fasle
        int roll = UnityEngine.Random.Range(0, 101);
        int chance = Calculate(target);
        return roll<=chance;
    }
    protected virtual void Start()
    {
        attacker = GetComponentInParent<Unit>();
    }
    protected virtual bool AutomaticHit(Unit target)
    {
        //공격자와 타겟을 저장
        MatchException exc = new MatchException(attacker, target);
        //automatichitcheck키로 된 함수를 호출 (exc 전달)
        this.PostNotification(AutomaticHitCheckNotification, exc);
        //exc에 저장된 toggle 값을 전달
        return exc.toggle;
    }
    protected virtual bool AutomaticMiss(Unit target)
    {
        //공격자와 타겟을 저장
        MatchException exc = new MatchException(attacker, target);
        //automaticmiss 키로 된 함수를 호출(exc 전달)
        this.PostNotification(AutomaticMissCheckNotification, exc);
        //exc에 저장된 toggle 값을 전달
        return exc.toggle;
    }
    protected virtual int AdjustForStatusEffects(Unit target,int rate)
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
