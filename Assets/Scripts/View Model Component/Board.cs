using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

    //선택 미선택에 따른 타일 색상
    Color selectedTileColor = new Color(0, 1, 1, 1);
    Color defaultTileColor = new Color(1, 1, 1, 1);

    //현재 검사할 타일의 주변타일을 참조할때 사용하는 변수
    Point[] dirs = new Point[4]
    {
        new Point(0,1),
        new Point(0,-1),
        new Point(1,0),
        new Point(-1,0),
    };

    public void Load(LevelData data)
    {
        for(int i=0; i<data.tiles.Count;++i)
        {
            GameObject instance = Instantiate(tilePrefab) as GameObject;
            Tile t = instance.GetComponent<Tile>();
            t.Load(data.tiles[i]);
            tiles.Add(t.pos, t);
        }
    }

    public List<Tile>Search(Tile start,Func<Tile,Tile,bool>addTile)
    {
        List<Tile> retValue = new List<Tile>();
        retValue.Add(start);

        ClearSearch(); //검사전에 전체 타일의 prev,distance 초기화
        Queue<Tile> checkNext = new Queue<Tile>();
        Queue<Tile> checkNow = new Queue<Tile>();

        start.distance = 0;
        checkNow.Enqueue(start);

        //검사시작
        while(checkNow.Count > 0)
        {
            Tile t = checkNow.Dequeue();
            for(int i =0;i<4;++i)
            {
                Tile next = GetTile(t.pos + dirs[i]);
                if (next == null || next.distance <= t.distance + 1)
                    continue;

                //이동 가능 거리 내에 있는 타일인지 검사
                if(addTile(t,next))
                {
                    //가능한 타일이면 다음 검사 대기열에 추가
                    next.distance = t.distance + 1;
                    next.prev = t;
                    checkNext.Enqueue(next);
                    retValue.Add(next);
                }
            }
            //현재 검사할 대기열이 종료되면 다음 검사할 대기열로 교체
            //while이 계속 돌아감
            if (checkNow.Count == 0)
                SwapReference(ref checkNow, ref checkNext);
        }

        //검사범위 내 타일들을 반환
        return retValue;
    }

    //해당 좌표에 타일이 있는지 검사
    public Tile GetTile(Point p)
    {
        return tiles.ContainsKey(p) ? tiles[p] : null;
    }

    //검사하기 전에 타일들의 prev,distance 초기화
    void ClearSearch()
    {
        foreach(Tile t in tiles.Values)
        {
            t.prev = null;
            t.distance = int.MaxValue;
        }
    }

    //다음 대기열로 교체
    void SwapReference(ref Queue<Tile>a,ref Queue<Tile>b)
    {
        Queue<Tile> temp = a;
        a = b;
        b = temp;
    }

    public void SelectTile(List<Tile>tiles)
    {
        for(int i =tiles.Count-1;i>=0;--i)
        {
            Renderer tileRender = tiles[i].GetComponent<Renderer>();
            tileRender.material.SetColor("_Color", selectedTileColor);
        }
    }

    public void DeSelectTiles(List<Tile>tiles)
    {
        for(int i=tiles.Count-1;i>=0;--i)
        {
            Renderer tileRender = tiles[i].GetComponent<Renderer>();
            tileRender.material.SetColor("_Color", defaultTileColor);
        }
    }
}
