using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//최대 체력의 10프로와 현재체력 중에 낮은 값으로 현재체력을 감소시키는 상태
public class PoisonStatusEffect : StatusEffect
{
    Unit owner;
    private void OnEnable()
    {
        owner = GetComponentInParent<Unit>();
        if (owner)
            this.AddObserver(OnNewTurn, TurnOrderController.TurnBeganNotification, owner);
    }
    private void OnDisable()
    {
        this.RemoveObserver(OnNewTurn, TurnOrderController.TurnBeganNotification, owner);
    }
    void OnNewTurn(object sender,object args)
    {
        Stats stats = GetComponentInParent<Stats>();
        int currentHP = stats[StateTypes.HP];
        int maxHP = stats[StateTypes.MHP];

        //현재 체력과 최대 체력*0.1중 낮은 값 저장
        int reduce = Mathf.Min(currentHP, Mathf.FloorToInt(maxHP * 0.1f));

        //현재 체력을 reduce로 변경
        stats.SetValue(StateTypes.HP, (currentHP - reduce), false);
    }
}
