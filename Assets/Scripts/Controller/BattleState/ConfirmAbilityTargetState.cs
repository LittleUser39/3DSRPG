using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//피격 범위 내 타겟 리스트를 저장하고 지정된 타겟의 능력ui를 갱신하는 역활
public class ConfirmAbilityTargetState : BattleState
{
    List<Tile> tiles;
    AbilityArea area;
    int index = 0;
    AbilityEffectTarget[] targetsers;
    public override void Enter()
    {
        base.Enter();
        //시전자의 피격타입을 참조
        area = turn.ability.GetComponent<AbilityArea>();
        //피격범위내의 타일들을 참조
        tiles = area.GetTilesInArea(board,pos);
        //피격 범위 내타일들을 선택중 상태로 변경
        board.SelectTile(tiles);
        //피격 범위 내 타일에 있는 대상을 참조
        FindTarget();
        //시전자의 능력 UI를 갱신
        RefreshPrimaryStatPanel(turn.actor.tile.pos);
        //타겟 지정 타겟 능력UI 갱신
        //처음에 젤 처음 지정된 대상이 타겟이됨
        if(turn.targets.Count>0)
        {
            HitSuccessIndicator.Show();
            SetTarget(0);
        }
    }

    public override void Exit()
    {
        base.Exit();
        //선택된 타일 해제
        board.DeSelectTiles(tiles);
        //타겟과 나의 능력UI 해제
        StatPanelController.HidePrimary();
        StatPanelController.HideSecondary();

        HitSuccessIndicator.Hide();
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
            UpdateHitSuccessIndicator();
        }
    }
    void UpdateHitSuccessIndicator()
    {
        //타겟의 최종 능력치
        int chance = 0;
        //임의로 50
        int amount = 0;
        Tile target = turn.targets[index];

        for (int i = 0; i < targetsers.Length; ++i)
        {
            if (targetsers[i].IsTarget(target))
            {
                HitRate hitRate = targetsers[i].GetComponent<HitRate>();
                targetsers[i].IsTarget(target);

                BaseAbilityEffect effect = targetsers[i].GetComponent<BaseAbilityEffect>();
                amount = effect.Predict(target);
                break;
            }
        }
        //능력치 세팅
        //두개의 매개변수에 따라 UI의 FillAmount 값이 변경됨
        HitSuccessIndicator.SetStats(chance, amount);
    }
   
}
