using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecifyAbilityArea : AbilityArea
{
    //유니티 에디터에서 입력되는 스킬범위
    public int horizontal;
    public int vertical;

    //중심이 되는 타일
    Tile tile;

    //범위 스킬
    public override List<Tile> GetTilesInArea(Board board, Point pos)
    {
        //선택된 타일의 좌표를 토대로 타일 정보를 참조
        tile = board.GetTile(pos);

        //tile 중심으로
        return board.Search(tile, ExpandSearch);
    }

    bool ExpandSearch(Tile from,Tile to)
    {
        //중심 좌표로 부터
        //x,z 거리가 horizontal 거리내이고
        //y(높이)가 vertical 거리내에 있으면 true 반환
        return (from.distance + 1) <= horizontal && Mathf.Abs(to.height - tile.height) <= vertical;
    }
}
