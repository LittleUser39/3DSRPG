using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : StateMachine
{
    //임시코드
    public GameObject heroPrefab;
    public Tile currentTile { get { return board.GetTile(pos); } }
    public StatPanelController StatPanelController;

    //메뉴판 관리,턴정보,전체 유닛 담는 배열
    public AbilityMenuPanelController abilityMenuPanelController;
    public Turn turn = new Turn();
    public List<Unit> units = new List<Unit>();

    //메인 카메라 컴포넌트
    //카메라가 tileselection을 따라가게 
    public CameraRig cameraRig;

    //leveldata 의 정보를 토대로 타일맵 불러오는 클래스
    public Board board;

    //타일맵에 타일들의 좌표정보가 저장된 leveldata
    public LevelData leveldata;

    //선택된 타일의 인디게이터
    public Transform tileselection;

    //tileselection의 좌표를 표시
    public Point pos;

    //턴의 프레임을 관리하는 코루틴
    public IEnumerator round;
    private void Start()
    {
        ChangeState<InitBattleState>();
    }
}
