using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSequenceState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine("Sequence");
    }

    //이동 코루틴
    //이동이 완료되면 이동한 캐릭터라는 것을 체크
    IEnumerator Sequence()
    {
        Movement m = turn.actor.GetComponent<Movement>();

        yield return StartCoroutine(m.Traverse(owner.currentTile));

        turn.hasUnitMoved = true;

        owner.ChangeState<CommandSelectionState>();
    }
}
