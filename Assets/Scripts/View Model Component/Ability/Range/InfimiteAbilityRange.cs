using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//모든 영역을 대상을 목표로 하는 스킬의 경우 사용
public class InfimiteAbilityRange : AbilityRange
{
    public override List<Tile> GetTilesInRange(Board board)
    {
        //모든 tile들을 반환
        return new List<Tile>(board.tiles.Values);
    }
}
