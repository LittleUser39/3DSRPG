using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//상태의 지속시간을 체크하는 상태컨디션
public class DurationStatusCondition : StatusCondition
{
    public int duration = 10;

    private void OnEnable()
    {
        //델리게이트 등록
        this.AddObserver(OnNewTurn, TurnOrderController.RoundBeganNotification);
    }
    private void OnDisable()
    {
        this.RemoveObserver(OnNewTurn, TurnOrderController.RoundBeganNotification);
    }

    void OnNewTurn(object sender,object args)
    {
        //지속시간 감소(턴)
        duration--;
        //0이 되면 해당 상태를 제거
        if (duration <= 0) Remove();
    }
}
