using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//전제 타일들의 좌표 정보를 관리하는 스크립트
//스크립팅이 가능한 오브젝트 생성
public class LevelData : ScriptableObject
{
    public List<Vector3> tiles;
}
