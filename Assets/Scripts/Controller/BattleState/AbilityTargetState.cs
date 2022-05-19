using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//능력 메뉴에서 공격을 선택하면 변경되는 상태
//스킬을 선택했을때도 나옴 
//능력을 쓸 타겟을 정하는 상태
public class AbilityTargetState : BattleState
{
    List<Tile> tiles;
    AbilityRange ar;

    //ability targetstate 상태가 되었을때 호출
    public override void Enter()
    {
        base.Enter();

        //대상의 공격타입을 참조
        ar = turn.ability.GetComponent<AbilityRange>();

        //공격 범위 내 타일들 색상 변겅
        SelectTiles();

        //공격자 능력치 정보 ui 출력
        statPanelController.ShowPrimary(turn.actor.gameObject);
    
        //방향 전환이 가능하다면
        if(ar.directionOriented)
        {
            //피격자 능력치 정보 ui출력
            RefreshSecondaryStatPanel(pos);
        }

    }
    //ability targetstate 상태가 종료될때 호출
    public override void Exit()
    {
        base.Exit();
        //선택된 타일 해제
        board.DeSelectTiles(tiles);

        //유닛 능력치 정보 ui 숨기기
        statPanelController.HidePrimary();
        statPanelController.HideSecondary();
    }
    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        //방향전환이 가능하다면
        if(ar.directionOriented)
        {
            //대상이 바라보는 방향을 변경
            ChangeDirection(e.info);
        }
        else
        {
            //선택된 타일 인디게이터(게임오브젝트)의 위치를 변경
            SelectTile(e.info + pos);
            //타겟의 능력ui를 갱신
            RefreshSecondaryStatPanel(pos);
        }
    }

    protected override void OnFire(object Sender, InfoEventArgs<int> e)
    {   //왼쪽 마우스를 눌렀을때
        if (e.info == 0)
        {
            if (ar.directionOriented || tiles.Contains(board.GetTile(pos)))
            {
                //해당 타일이 탐색범위 내 존재하는 타일인지
                //방향 전환이 되는 공격 타입인지 확인

                //confirmAbilityTargetState로 변경
                owner.ChangeState<ConfirmAbilityTargetState>();
            }
        }
        //오른쪽 마우스를 눌렀을때
        else
        {
            //caregoryselectionstate로 돌아감
            owner.ChangeState<CategorySelectionState>();
        }
    }
    void ChangeDirection(Point p)
    {
        //바라보는 방향(절대값)
        Directions dir = p.GetDirections();

        //turn.actor.dir과 위방향이 다르다면
        if(turn.actor.dir!=dir)
        {
            //선택된 타일 해제
            board.DeSelectTiles(tiles);
            //turn.actor.dir 을 같도록
            turn.actor.dir = dir;
            //turn.actor의 position과 eulerangles 값을 변경
            turn.actor.Match();
            //공격 범위 내 타일들 색상 변경
            SelectTiles();
        }
    }
    void SelectTiles()
    {
        //공격 범위내 타일들 참조
        tiles = ar.GetTilesInRange(board);
        //공격 범위 내 타일들의 색상 변경
        board.SelectTile(tiles);
    }
       
}
