using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAbilityArea : AbilityArea
{
    //1인 타겟
    public override List<Tile> GetTilesInArea(Board board, Point pos)
    {
        //피격 범위 내 리스트를 저장하는 배열
        List<Tile> retValue = new List<Tile>();

        //pos 좌표에 있는 타일 정보를 참조
        Tile tile = board.GetTile(pos);

        //해당 좌표에 타일이 있으면
        if (tile != null)
            //타일을 retValue 리스트에 추가
            retValue.Add(tile);

        //리스트 반환
        return retValue;
    }
}
