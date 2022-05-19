using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//기본적인 승리 조건 클래스
//다양한 승리 조건을 만들수 있어 추상클래스로 구현
public abstract class BaseVictoryCondition : MonoBehaviour
{
    Alliances victor = Alliances.None;
    public Alliances Victor
    {
        get { return victor; }
        protected set { victor = value; }
    }
    //유닛 목록과 같은 게임내의 오브젝트를 참조할수 있음
    protected BattleController bc;
    private void Awake()
    {
        bc = GetComponent<BattleController>();
    }
    //유닛들의 hp가 변경하기만 해도 알림이 감
   protected virtual void OnEnable()
    {
        this.AddObserver(OnHPDidChangeNotification, Stats.DidChangeNotification(StateTypes.HP));
    }
    protected virtual void OnDisEnable()
    {
        this.RemoveObserver(OnHPDidChangeNotification, Stats.DidChangeNotification(StateTypes.HP));
    }
    protected virtual void OnHPDidChangeNotification(object sender,object args)
    {
        CheckForGameOver();
    }

    //특정유닛(유닛 하나)이 죽었는 지 확인 하는 함수
    protected virtual bool IsDefeated(Unit unit)
    {
        Health health = unit.GetComponent<Health>();
        if(health)
        {
            return health.minHP == health.HP;
        }

        Stats stats = GetComponent<Stats>();
        return stats[StateTypes.HP] == 0;
    }

    //유닛의 파티가 전멸했는지 확인하는 함수
    //매개변수로 파티를 받아 그파티에 있는 유닛들이 전멸했는지 확인하는 함수
    protected virtual bool PartyDefeated(Alliances type)
    {
        for(int i=0;i<bc.units.Count;++i)
        {
            //배틀에 있는 유닛들의 동맹컴포넌트 참조
            Alliance a = bc.units[i].GetComponent<Alliance>();
            
            //없으면 넘어감
            if(a == null)
            {
                continue;
            }

            //만약 유닛의 파티가 현재 매개변수의 파티와 같고 그 파티에 있는 유닛이 전멸하지 않았으면
            //false 반환
            if(a.type==type && !IsDefeated(bc.units[i]))
            {
                return false;
            }
        }
        //전멸시 true  반환
        return true;
    }
    //아군이 게임오버 되었는지 확인하는 함수
    protected virtual void CheckForGameOver()
    {
        if(PartyDefeated(Alliances.Hero))
        {
            //Enemy의 승리
            Victor = Alliances.Enemy;
        }
    }

}
