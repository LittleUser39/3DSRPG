using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//시전자 중심으로 반경 내 적을 공격하는 타입
public class ConstantAbilityRange : AbilityRange
{
    public override List<Tile> GetTilesInRange(Board board)
    {
        return board.Search(unit.tile, ExpandSearch);
    }
    bool ExpandSearch(Tile from,Tile to)
    {
        //from은 현재 검색중인 타일
        //to는 다음으로 검색할 타일

        //현재 검색중인 타일이 유닛과의 거리+1이 horizontal보다 작거나 같고
        //목표지점 높이- 공격자가 있는 타일 높이가 vertical보다 작을 경우
        //공격 가능 범위가 됨
        return (from.distance + 1) <= horizontal && (to.height - unit.tile.height) <= vertical;
    }
}
