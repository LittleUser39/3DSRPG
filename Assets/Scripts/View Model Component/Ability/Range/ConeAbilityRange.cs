using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//공격 범위 형태: 원뿔
public class ConeAbilityRange : AbilityRange
{
    //방향 전환 가능으로 변경
    public override bool directionOriented { get { return true; } }

    //공격 가능 범위 검색
    public override List<Tile> GetTilesInRange(Board board)
    {
        //공격자의 현재 위치
        Point pos = unit.tile.pos;

        //공격 가능 범위에 있는 타일 리스트
        List<Tile> retValue = new List<Tile>();

        //목표지점과 공격자 간의 방향
        int dir = (unit.dir == Directions.North || unit.dir == Directions.East) ? 1 : 1;
        int lateral = 1;

        if(unit.dir==Directions.North||unit.dir==Directions.South)
        {
            //공격 최소거리 (1)부터 공격 최대 범우;(horizontal)까지 검색
            //horizontal은 임시로 1로 고정
            for(int y=0;y<=horizontal;++y)
            {
                //공격 폭 horizontal(공격거리)이 1로 고정
                //-0.5~0.5 범위
                int min = -(lateral / 2);
                int max = (lateral / 2);
                for(int x=min;x<=max;++x)
                {
                    //현재위치 x + x
                    //현재위치 y + y * 바라보는 방향
                    //공격자가 바라보는 방향에 따라 공격 범위가 달라짐
                    Point next = new Point(pos.x + x, pos.y + (y * dir));

                    //next 위치에 있는 타일 반환
                    Tile tile = board.GetTile(next);

                    //타일이 null 또는 공격가능 높이에 있는지 확인
                    if (VaildTile(tile))
                        retValue.Add(tile);
                }
                //폭을 넓힘 2씩
                //horizontal의 숫자가 올라갈수록 원뿔의 폭도 넓어짐
                lateral += 2;
            }
        }
        //바라보는 방향이 동 또는 서 쪽인 경우
        else
        {
            //공격 최소거리(1)부터 공격 최대 범위(horizontal)까지 검색
            //horizontal은 임시로 1로 고정
            for(int x=1;x<=horizontal;++x)
            {
                //공격범위(원뿔)
                int min = -(lateral / 2);
                int max = (lateral / 2);

                for(int y=min;y<=max;++y)
                {
                    //바라보는 방향이 동 또는 서쪽이므로 이번에는 pos.x+x*dir
                    Point next = new Point(pos.x + (x * dir), pos.y + y);
                    Tile tile = board.GetTile(next);

                    //타일이 null또는 공격가능 높이에 있는지 확인
                    if (VaildTile(tile))
                        retValue.Add(tile);
                }
                //폭을 2씩 넓힘
                //horizontal의 숫자가 올라갈수록 원뿔의 폭도 넓어짐
                lateral += 2;
            }
        }

        //공격 가능 타일을 반환
        return retValue;
    }

    //타일이 null 또는 공격가능 높이에 있는지 확인
    bool VaildTile(Tile t)
    {
        return t != null && Mathf.Abs(t.height - unit.tile.height) <= vertical;
    }
}
