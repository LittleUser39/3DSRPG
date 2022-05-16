using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityArea : MonoBehaviour
{
    //피격 범위 내 있는 타일리스트
    public abstract List<Tile> GetTilesInArea(Board board, Point pos);
}
