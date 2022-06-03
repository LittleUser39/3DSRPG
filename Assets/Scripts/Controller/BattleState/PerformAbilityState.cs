using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//최종적으로 공격할 대상을 지정하면 변경되는 상태
//공격할 대상을 지정하면 유닛이 행동하는 상태 클래스
public class PerformAbilityState : BattleState
{
    Animator animator;
    bool hit;
    public override void Enter()
    {
        base.Enter();

        //해당 턴의 유닛의 애니메이션을 가져옴
        animator = turn.actor.GetComponent<Animator>();

        turn.hasUnitActed = true;
        if(turn.hasUnitMoved)
        {
            turn.lockMove = true;
        }
        StartCoroutine(Animate());
    }
    IEnumerator Animate()
    {
        //나중에 여기서 애니메이션 재생해야함,
        if(animator!=null)
         animator.SetBool("Attack", true);
        
        yield return new WaitForSeconds(1f);
        
        if(animator!=null)
           animator.SetBool("Attack", false);
        
        ApplyAbility();

        //여기서 데미지 나오는거 플레이
       DamageText.instance.Display();
       
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

        //애니메이션 초기화
        animator = null;
    }
   
    void ApplyAbility()
    {
        //능력 확인시 ability 클래스로 이동해서 perform 함수 실행
        turn.ability.Perform(turn.targets);
    }
    bool UnitHasControl()
    {
        //녹다운 하고있는 상태가 아닌경우
        return turn.actor.GetComponentInChildren<KnockOutStatusEffect>() == null;
    }
}
