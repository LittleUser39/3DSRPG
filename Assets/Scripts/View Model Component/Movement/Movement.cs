using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public int range{get { return stats[StateTypes.MOV]; } } //이동범위
    public int jumpHeight { get { return stats[StateTypes.JMP]; } }  //점프 높이
    protected Stats stats;
    protected Unit unit;    //이동하는 개체 (monster or hero)
    protected Transform jumper;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();

        //이거 있어야 프리팹들이 움직임
        jumper = transform.Find("Jumper");
    }
    private void Start()
    {
        stats = GetComponent<Stats>();
    }
    public virtual List<Tile>GetTilesInRange(Board board)
    {
        List<Tile> retValue = board.Search(unit.tile, ExpandSearch);
        Filter(retValue);

        //이동 범위 내 이동할수 있는 타일들을 반환
        return retValue;
    }

    //movement를 상속받는 클래스에서 해당 개체의 이동에 대해 강제적으로 정의하도록
    //추상화 함수로 만듬
    public abstract IEnumerator Traverse(Tile tile);

    //회전을 담당하는 함수
    protected virtual IEnumerator Turn(Directions dir)
    {
        //각도 회전 등을 반환
        TransformLocalEulerTweener t = (TransformLocalEulerTweener)transform.RotateToLocal
            (
                dir.ToEuler(),
                0.25f,
                EasingEquations.EaseInOutQuad
            );

        //북쪽과 서쪽 사이를 회전할 때 효율적인 방법으로 회전하는것처럼 보이도록
        //예외를 만들어야함(0과360도는 동일하게 봄)
        if(Mathf.Approximately(t.startTweenValue.y,0f)&&Mathf.Approximately(t.endTweenValue.y,270f))
        {
            t.startTweenValue = new Vector3(t.startTweenValue.x, 360f, t.startTweenValue.z);
        }
        else if(Mathf.Approximately(t.startTweenValue.y,270)&&Mathf.Approximately(t.endTweenValue.y,0))
        {
            t.endTweenValue = new Vector3(t.startTweenValue.x, 360f, t.startTweenValue.z);
        }
        unit.dir = dir;
        while (t != null) yield return null;
    }

    //이동범위 안의 타일인지 체크
    protected virtual bool ExpandSearch(Tile from,Tile to)
    {
        return (from.distance + 1) <= range;
    }

    //몬스터가 있거나 영웅이 있는 타일은 제외
    protected virtual void Filter(List<Tile>tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
            if (tiles[i].content != null)
                tiles.RemoveAt(i);
    }
}
