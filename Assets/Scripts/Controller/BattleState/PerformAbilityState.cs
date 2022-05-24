﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//최종적으로 공격할 대상을 지정하면 변경되는 상태
//공격할 대상을 지정하면 유닛이 행동하는 상태 클래스
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
        //todo 나중에 여기서 애니메이션 재생해야함,
        yield return null;
        ApplyAbility();

        //여기서 데미지 나오는거 플레이
        owner.damageText.Display();
       
        //전투가 끝남
        if(IsBattleOver())
        {
            owner.ChangeState<CutSceneState>();
        }
        //아직 움직일 유닛이 있을때
        else if(!UnitHasControl())
        {
            owner.ChangeState<SelectUnitState>();
        }
        //해당 유닛이 움직였음
        else if(turn.hasUnitMoved)
        {
            owner.ChangeState<EndFacingState>();
        }
        //오른쪽 마우스 클릭(취소)
        else
        {
            owner.ChangeState<CommandSelectionState>();
        }
    }
   
    void ApplyAbility()
    {
        //능력 확인시 ability 클래스로 이동해서 perform 함수 실행
        turn.ability.Perform(turn.targets);
        //변경전
        //BaseAbilityEffect[] effects = turn.ability.GetComponentsInChildren<BaseAbilityEffect>();
        //for (int i = 0; i < turn.targets.Count; ++i)
        //{
        //    Tile target = turn.targets[i];
        //    for (int j = 0; j < effects.Length; ++j)
        //    {
        //        BaseAbilityEffect effect = effects[j];
        //        AbilityEffectTarget targeter = effect.GetComponent<AbilityEffectTarget>();
        //        if(targeter.IsTarget(target))
        //        {
        //            HitRate rate = effect.GetComponent<HitRate>();
        //            int chance = rate.Calculate(target);
        //            if(UnityEngine.Random.Range(0,101)>chance)
        //            {
        //                //Miss
        //                continue;
        //            }
        //            effect.Apply(target);
        //        }
        //    }
        //}
    }
    bool UnitHasControl()
    {
        //녹다운 하고있는 상태가 아닌경우
        return turn.actor.GetComponentInChildren<KnockOutStatusEffect>() == null;
    }
}
