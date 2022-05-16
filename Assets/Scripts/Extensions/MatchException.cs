using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//toggle을 통해서 해당 계산을 진행할지 결정하는데 사용하는 클래스
//AType,SType 에서 toggle이 false이면 타겟의 저항 또는 회피가 0또는 100이 되도록 만듬 
public class MatchException : BaseException
{
    public readonly Unit attacker;
    public readonly Unit target;

    //BaseException.toggle에 false로 저장
    public MatchException(Unit attacker, Unit target) : base(false)
    {
        this.attacker = attacker;
        this.target = target;
    }
}
