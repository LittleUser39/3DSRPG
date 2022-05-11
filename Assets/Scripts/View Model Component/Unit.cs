using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
   public Tile tile { get; protected set; }
    public Directions dir;
    //unit이 머물고 있는 타일을 변경할때 호출
    //이동 죽음등의 이유로 함수가 호출 될것같음
    public void Place(Tile target)
    {
        //이전에 선택한 tile이 null로 초기화 안되었다면
        if (tile != null && tile.content == gameObject)
            tile.content = null;

        tile = target;

        if (target != null)
            target.content = gameObject;
    }

    //해당 게임 오브젝트의 positon과 eulerangles 값을 변경
    public void Match()
    {
        transform.localPosition = tile.center;

        //vector3(x,y,z)로 rotation을 구하는것
        transform.localEulerAngles = dir.ToEuler();
    }
}
