using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//자신의 위치를 반환하는 클래스
public class SelfAbiltyRange : AbilityRange
{
    public override List<Tile> GetTilesInRange(Board board)
    {
        //자신의 위치를 반환
        //본인이 있는 위치는 탐색할 필요 없음
        //자기 자신에게 사용하는 스킬의 경우에 사용
        List<Tile> retValue = new List<Tile>(1);
        retValue.Add(unit.tile);
        return retValue;
    }
}
