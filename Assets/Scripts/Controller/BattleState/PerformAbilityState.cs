using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//최종적으로 공격할 대상을 지정하면 변경되는 상태
public class PerformAbilityState : BattleState
{
    public override void Enter()
    {
        base.Enter();

        turn.hasUnitActed = true;
        if(turn.hasUnitMoved)
        {
            turn.lockMove = true;
        }
        StartCoroutine(Animate());
    }
    IEnumerator Animate()
    {
        yield return null;

        TempoaryAttackExample();

        if(turn.hasUnitMoved)
        {
            owner.ChangeState<EndFacingState>();
        }
        else
        {
            owner.ChangeState<CommandSelectionState>();
        }
    }
    void TempoaryAttackExample()
    {
        for(int i=0;i<turn.targets.Count;++i)
        {
            GameObject obj = turn.targets[i].content;

            Stats stats = obj != null ? obj.GetComponent<Stats>() : null;
        
            if(stats!=null)
            {
                stats[StateTypes.HP] -= 50;
                if(stats[StateTypes.HP]<=0)
                {
                    Debug.Log("죽음", obj);
                }
            }
                        
        }
    }
    void ApplyAbility()
    {
        BaseAbilityEffect[] effects = turn.ability.GetComponentsInChildren<BaseAbilityEffect>();
        for (int i = 0; i < turn.targets.Count; ++i)
        {
            Tile target = turn.targets[i];
            for (int j = 0; j < effects.Length; ++j)
            {
                BaseAbilityEffect effect = effects[j];
                AbilityEffectTarget targeter = effect.GetComponent<AbilityEffectTarget>();
                if(targeter.IsTarget(target))
                {
                    HitRate rate = effect.GetComponent<HitRate>();
                    int chance = rate.Calculate(target);
                    if(UnityEngine.Random.Range(0,101)>chance)
                    {
                        //Miss
                        continue;
                    }
                    effect.Apply(target);
                }
            }
        }
    }
}
