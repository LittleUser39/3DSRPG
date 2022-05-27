using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//마법 능력의 사용을 규제하는 구성요소
//유닛에 능력사용 비용이 없을때 CamPerform알림,Flip 예외 토글
//비용을 제거하기 위해 DidPerform 알림
public class AbilityMagicCost : MonoBehaviour
{
    public int amount;
    Ability owner;

    private void Awake()
    {
        owner = GetComponent<Ability>();
    }
    private void OnEnable()
    {
        this.AddObserver(OnCanPerformCheck, Ability.CanPerformCheck, owner);
        this.AddObserver(OnDidPerformNotification, Ability.DidPerformNotification, owner);
    }
    private void OnDisable()
    {
        this.RemoveObserver(OnCanPerformCheck, Ability.CanPerformCheck, owner);
        this.RemoveObserver(OnDidPerformNotification, Ability.DidPerformNotification, owner);
    }
    void OnCanPerformCheck(object sender,object args)
    {
        Stats stats = GetComponentInParent<Stats>();
        if(stats[StateTypes.MP] < amount)
        {
            BaseException exc = (BaseException)args;
            exc.FlipToggle();
        }
    }
    void OnDidPerformNotification(object sender,object args)
    {
        Stats stats = GetComponentInParent<Stats>();
        stats[StateTypes.MP] -= amount;
    }
}
