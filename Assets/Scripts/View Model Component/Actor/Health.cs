using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//hit point 변경 알림이 실행될때 마다 ClampValueModifier를 사용하여 
//HP를 관리
public class Health : MonoBehaviour
{
    Stats stats;
  public int HP
    {
        get
        {
            return stats[StateTypes.HP];
        }
        set
        {
            stats[StateTypes.HP] = value;
        }
    }
    public int MHP
    {
        get { return stats[StateTypes.MHP]; }
        set { stats[StateTypes.MHP]= value; }
    }
    private void Awake()
    {
        stats = GetComponent<Stats>();
    }
    private void OnEnable()
    {
        this.AddObserver(OnWillChangeHP, Stats.WillChangeNotification(StateTypes.HP), stats);
        this.AddObserver(OnWillChangeMHP, Stats.DidChangeNotification(StateTypes.MHP), stats);
    }
    private void OnDisable()
    {
        this.RemoveObserver(OnWillChangeHP, Stats.WillChangeNotification(StateTypes.HP), stats);
        this.RemoveObserver(OnWillChangeMHP, Stats.DidChangeNotification(StateTypes.MHP), stats);
    }

    void OnWillChangeHP(object sender,object args)
    {
        ValueChangeException vce = args as ValueChangeException;
        vce.AddModifier(new ClampValueModifier(int.MaxValue, 0, stats[StateTypes.HP]));
    }
    void OnWillChangeMHP(object sender,object args)
    {
        int oldMHP = (int)args;
        if (MHP > oldMHP)
            HP += MHP - oldMHP;
        else
            HP = Mathf.Clamp(HP, 0, MHP);
    }
}
