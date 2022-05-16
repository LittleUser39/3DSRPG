using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//피격 범위 내 타겟 리스트를 저장하고 지정된 타겟의 능력ui를 갱신하는 역활
public class ConfirmAbilityTargetState : BattleState
{
    List<Tile> tiles;
    AbilityArea area;
    int index = 0;

    public override void Enter()
    {
        base.Enter();

        area = turn.ability.GetComponent<AbilityArea>();

        tiles = area.GetTilesInArea(board,pos);

        board.SelectTile(tiles);

        FindTarget();

        RefreshPrimaryStatPanel(turn.actor.tile.pos);

        SetTarget(0);
    }

    public override void Exit()
    {
        base.Exit();

        board.DeSelectTiles(tiles);

        StatPanelController.HidePrimary();
        StatPanelController.HideSecondary();
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        //방향키로 타겟 변경 가능
        if(e.info.y>0||e.info.x>0)
        {
            SetTarget(index + 1);
        }
        else
        {
            SetTarget(index - 1);
        }
    }
    protected override void OnFire(object Sender, InfoEventArgs<int> e)
    {
        if(e.info==0)
        {
            if(turn.targets.Count>0)
            {
                owner.ChangeState<PerformAbilityState>();
            }
        }
        else
        {
            owner.ChangeState<AbilityTargetState>();
        }
    }
    void FindTarget()
    {
        turn.targets = new List<Tile>();

        AbilityEffectTarget[] targets = turn.ability.GetComponentsInChildren<AbilityEffectTarget>();

        for(int i=0;i<tiles.Count;++i)
        {
            if(IsTarget(tiles[i],targets))
            {
                turn.targets.Add(tiles[i]);
            }
        }
    }
    bool IsTarget(Tile tile, AbilityEffectTarget[] targets)
    {
        for(int i=0;i<targets.Length;++i)
        {
            if(targets[i].IsTarget(tile))
            {
                return true;
            }
        }
        return false;
    }
    void SetTarget(int target)
    {
        index = target;

        if(index<0)
        {
            index = turn.targets.Count - 1;
        }
        if(index>=turn.targets.Count)
        {
            index = 0;
        }
        if(turn.targets.Count>0)
        {
            RefreshPrimaryStatPanel(turn.targets[index].pos);
        }
    }
}
