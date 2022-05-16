using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityEffectTarget : MonoBehaviour
{
    //타겟을 할 수 있는 대상인지 체크하는 추상화 함수
    public abstract bool IsTarget(Tile tile);
}
