using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public const float stepHeight = 0.25f;  //같은 위치에 2개 이상의 타일을 배치 해서 층을 높이거나 낮출때 y값 변화량 Grow 한번에 0.25만큼 스캐일 변경
    public Point pos;   //타일의 좌표값을 가지고 있는 변수
    public int height;  //타일의 현재 층수를 저장하는 변수
    public GameObject content; //해당 타일에 있는 게임오브젝트(유닛)
    //경로찾기에 사용할 전역 변수 - inspector뷰에서 숨긴다
    [HideInInspector] public Tile prev;
    [HideInInspector] public int distance;
    public Vector3 center   //해당 타일의 중심점을 반환
    {
        get
        {
            return new Vector3(pos.x, height * stepHeight, pos.y);
        }
    }
    //타일이 생성 되거나, 감소된후 로컬 포지션,로컬 스케일 값을 변경, 타일의 크기를 직접적으로 변경해주는 함수
    void Match()
    {
        transform.localPosition = new Vector3(pos.x, height * stepHeight / 2f, pos.y);
        transform.localScale = new Vector3(1, height * stepHeight, 1);
    }

    public void Grow()
    {
        height++;
        Match();
    }
    public void Shrink()
    {
        height--;
        Match();
    }
    //타일을 생성할때 호출하는 함수 타일의 높이와 좌표값을 저장한 뒤에 타일의 좌표와 스케일 값을 조정하는 match를 호출
    public void Load(Point p,int h)
    {
        pos = p;
        height = h;
        Match();
    }
    public void Load(Vector3 v)
    {
        Load(new Point((int)v.x, (int)v.z),(int)v.y);
    }


}
