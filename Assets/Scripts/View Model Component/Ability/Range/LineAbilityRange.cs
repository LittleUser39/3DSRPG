using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineAbilityRange : AbilityRange
{
    //방향 전환이 가능으로 변경
    public override bool directionOriented { get { return true; } }
    //공격 가능 타일 검색
    public override List<Tile> GetTilesInRange(Board board)
    {
        //공격자 위치
        Point startPos = unit.tile.pos;

        //목표지점 끝
        Point endPos;

        List<Tile> retValue = new List<Tile>();

        switch (unit.dir)
        {
            //방향을 구함
            //공격자가 바라보는 방향의 끝에 있는 타일 좌표를 구함
            case Directions.North://북
                endPos = new Point(startPos.x, board.max.y);
                break;
            case Directions.East: //동
                endPos = new Point(board.max.x, startPos.y);
                break;
            case Directions.South://남
                endPos = new Point(startPos.x, board.min.y);
                break;
            default ://서
                endPos = new Point(board.min.x, startPos.y);
                break;
        }
        //거리
        int dist = 0;

        //공격자의 위치에서 공격자가 바라보는 방향의 끝에 있는 타일 좌표까지 검색
        while(startPos != endPos)
        {
            if (startPos.x < endPos.x)
            {
                startPos.x++;
            }
            else if(startPos.x>endPos.x)
            {
                startPos.x--;
            }
            if (startPos.y < endPos.y)
            {
                startPos.y++;
            }
            else if(startPos.y>endPos.y)
            {
                startPos.y--;
            }

            //공격자의 위치에서 바라보는 방향에 있는 타일을 한칸씩 참조
            Tile t = board.GetTile(startPos);

            //타일이 공격 가능한 위치에 있는지 확인
            if(t!=null&&Mathf.Abs(t.height-unit.tile.height)<=vertical)
            {
                retValue.Add(t);
            }

            //거리 증가
            dist++;
            
            if(dist>=horizontal)
            {
                break;
            }    
        }
        //공격 범위내에 있는 타일 반환
        return retValue;
    }
}
