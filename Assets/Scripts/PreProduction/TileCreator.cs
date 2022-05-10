using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//파일을 저장하거나,복사,삭제,이동 관련기능
using System.IO;
public class TileCreator : MonoBehaviour
{
    //tile 프리팹을 담는 변수
    [SerializeField] GameObject tileViewPrefab;
    
    //tileselection 담는변수(현재 위치)
    [SerializeField] GameObject tileSelectionPreab;
    
    //필드의 범위
    [SerializeField] int width = 10;
    [SerializeField] int depth = 10;
    [SerializeField] int height = 5;

    //tile을 배치할 위치 정보를 담음
    //유니티 에디테에서 사용자가 정의하는 좌표
    [SerializeField] Point pos;

    //불러오기로 사용할 levelData를 담는 변수
    //leveldata는 scriptableobject 상속
    //vector 타입의 배열 변수가 존재
    [SerializeField] LevelData levelData;

    //타일 배치 정보를 담는 Dictionary 배열
    //Dictionary는 키와 값이 있는 배열 <point(키),tile(값)>을 가지는 배열 생성
    Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

    //타일을 배치할 위치를 표시하는 tilesecetion 의 현재 좌표 반환
    Transform marker
    {
        get
        {
            if(_marker == null)
            {
                GameObject instance = Instantiate(tileSelectionPreab) as GameObject;
                _marker = instance.transform;
            }
            return _marker;
        }
    }
    Transform _marker;

    //단일로 타일을 추가할 때 처음 호출되는 함수
    public void Grow()
    {
        GrowSingle(pos);
    }
   
    //단일로 타일을 제거할때 처음 호출되는 함수
    public void Shrink()
    {
        ShrinkSingle(pos);
    }

    //랜덤 범위 내 다수의 타일을 생성 처음 호출되는 함수
    public void GrowArea()
    {
        Rect r = RandomRect();
        GrowRect(r);
    }

    public void ShrinkArea()
    {
        Rect r = RandomRect();
        ShrinkRect(r);
    }

    //tile sectionprefab의 위치를 변경시 호출
    public void UpdateMarker()
    {
        Tile t = tiles.ContainsKey(pos) ? tiles[pos] : null;
        marker.localPosition = t != null ? t.center : new Vector3(pos.x, 0, pos.y);
    }

    //필드 전부를 지워줌
    public void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; --i)
        {
            //destroy와 같이 오브젝트 삭제 약간지연있음 , lmmdiate 즉시 삭제 에셋을 삭제할려면 이것을 사용 - 편집코드
            //Destroy(transform.GetChild(i).gameObject);
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        tiles.Clear();
    }

    //현재 배치 정보를 저장
    public void Save()
    {
        //저장할 경로를 string 타입으로 저장
        string filePath = Application.dataPath + "/Resources/Levels";

        //디렉토리의 filepath 경로가 있는지 확인
        if (!Directory.Exists(filePath))
            CreateSaveDirectory();

        LevelData board = ScriptableObject.CreateInstance<LevelData>();
        board.tiles = new List<Vector3>(tiles.Count);

        //tiles의 타일의 좌표값을 leveldata board에 저장
        foreach (Tile t in tiles.Values)
            board.tiles.Add(new Vector3(t.pos.x, t.height, t.pos.y));

        string fileName = string.Format("Assets/Resources/Levels/{1}.asset", filePath, name);
        AssetDatabase.CreateAsset(board, fileName);
    }

    //leveldata의 좌표로 타일들을 배치
    public void Load()
    {
        Clear();
        if (levelData == null)
            return;

        foreach(Vector3 v in levelData.tiles)
        {
            Tile t = Create();
            t.Load(v);
            tiles.Add(t.pos, t);
        }
    }

    Rect RandomRect()
    {
        int x = UnityEngine.Random.Range(0, width);
        int y = UnityEngine.Random.Range(0, depth);
        int w = UnityEngine.Random.Range(0, width - x + 1);
        int h = UnityEngine.Random.Range(0, depth - y + 1);
        return new Rect(x, y, w, h);
    }

    void GrowRect(Rect rect)
    {
        for(int y=(int)rect.yMin;y<(int)rect.yMax;y++)
        {
            for(int x=(int)rect.xMin;x<(int)rect.xMax;x++)
            {
                Point p = new Point(x, y);
                GrowSingle(p);
            }
        }
    }
    void ShrinkRect(Rect rect)
    {
        for (int y = (int)rect.yMin; y < (int)rect.yMax; y++)
        {
            for (int x = (int)rect.xMin; x < (int)rect.xMax; x++)
            {
                Point p = new Point(x, y);
                ShrinkSingle(p);
            }
        }
    }

    //타일을 생성
    Tile Create()
    {
        GameObject instance = Instantiate(tileViewPrefab) as GameObject;
        instance.transform.parent = transform;
        return instance.GetComponent<Tile>();
    }

    //tiles 배열에 p좌표의 타일이 있는지 확인
    //있으면 해당 타일 반환
    //없으면 생성한 뒤 생성한 타일 반환
    Tile GetOrCreate(Point p)
    {
        if (tiles.ContainsKey(p))
            return tiles[p];

        Tile t = Create();

        t.Load(p, 0);
        tiles.Add(p, t);

        return t;
    }

    //타일의 높이를 결정
    void GrowSingle(Point p)
    {
        Tile t = GetOrCreate(p);
        if (t.height < height)
            t.Grow();
    }

    //타일을 감축시키거나 삭제
    void ShrinkSingle(Point p)
    {
        if (!tiles.ContainsKey(p))
            return;
        Tile t = tiles[p];
        t.Shrink();

        if(t.height<=0)
        {
            tiles.Remove(p);
            DestroyImmediate(t.gameObject);
        }
    }

    //경로(폴더)를 생성
    void CreateSaveDirectory()
    {
        string filePath = Application.dataPath + "/Resources";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Asset/Resources", "Levels");
        AssetDatabase.Refresh();
    }
}
