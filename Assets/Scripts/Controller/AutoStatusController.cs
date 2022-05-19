using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//체력과 같은 능력치를 자동으로 확인해주는 클래스
//캐릭터가 죽는것을 컨트롤하는 클래스
public class AutoStatusController : MonoBehaviour
{
    private void OnEnable()
    {
        this.AddObserver(OnHpDidChangeNotification, Stats.DidChangeNotification(StateTypes.HP));
    }
    private void OnDisable()
    {
        this.RemoveObserver(OnHpDidChangeNotification, Stats.DidChangeNotification(StateTypes.HP));
    }
    void OnHpDidChangeNotification(object sender,object args)
    {
        Stats stats = sender as Stats;
        if(stats[StateTypes.HP] == 0)
        {
            Status status = stats.GetComponentInChildren<Status>();
            //<효과,조건>을 매개변수로 줌
            StatComparisonCondition c = status.Add<KnockOutStatusEffect, StatComparisonCondition>();
            c.Init(StateTypes.HP, 0, c.EqualTo);
        }
    }
}
