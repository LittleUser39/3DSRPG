using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//능력이 실제로 사용 할수 있는지 확인
//사용할 수 없으면 UI에서 비활성화
public class Ability : MonoBehaviour
{
    public const string CanPerformCheck = "Ability.CanPerformCheck";
    public const string FailedNotification = "Ability.FailedNotification";
    public const string DidPerformNotification = "Ability.DidPerformNotification";

    public bool CanPerform()
    {
        BaseException exc = new BaseException(true);
        this.PostNotification(CanPerformCheck, exc);
        return exc.toggle;
    }
    public void Perform(List<Tile> targets)
    {
        if(!CanPerform())
        {
            this.PostNotification(FailedNotification);
            return;
        }

        for(int i =0;i<targets.Count;++i)
        {
            Perform(targets[i]);
        }
        this.PostNotification(DidPerformNotification);
    }
    void Perform(Tile target)
    {
        for(int i=0;i<transform.childCount;++i)
        {
            Transform child = transform.GetChild(i);
            BaseAbilityEffect effect = child.GetComponent<BaseAbilityEffect>();
            effect.Apply(target);
        }
    }
    public bool IsTarget(Tile tile)
    {
        Transform obj = transform;
        for (int i = 0; i < obj.childCount; ++i)
        {
            AbilityEffectTarget targeter = obj.GetChild(i).GetComponent<AbilityEffectTarget>();
            if (targeter.IsTarget(tile))
                return true;
        }
        return false;
    }
}
