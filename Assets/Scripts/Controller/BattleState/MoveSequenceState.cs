using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSequenceState : BattleState
{
    Animator animator;
    public override void Enter()
    {
        base.Enter();
        animator = turn.actor.GetComponent<Animator>();

        StartCoroutine("Sequence");
    }

    //이동 코루틴
    //이동이 완료되면 이동한 캐릭터라는 것을 체크
    IEnumerator Sequence()
    {
        Movement m = turn.actor.GetComponent<Movement>();
        animator.SetBool("Move", true);
        yield return StartCoroutine(m.Traverse(owner.currentTile));
        yield return new WaitForSeconds(1);
        animator.SetBool("Move", false);
        turn.hasUnitMoved = true;

        owner.ChangeState<CommandSelectionState>();
    }
}
