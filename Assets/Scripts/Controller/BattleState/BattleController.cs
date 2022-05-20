﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : StateMachine
{
    public Tile currentTile { get { return board.GetTile(pos); } }
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
   
    //능력치 패널 관리
    public StatPanelController StatPanelController;

    //메뉴판 관리,턴정보,전체 유닛 담는 배열
    public AbilityMenuPanelController abilityMenuPanelController;
    
    //공격 맞을 확률을 보여주는 UI
    public HitSuccessIndicator HitSuccessIndicator;

    //유닛의 방향을 강조하는 오브젝트
    public FacingIndicator facingIndicator;

    //배틀 메세지를 띄워주는 ui
    public BattleMassegeController BattleMassegeController;

    //AI
    public ComputerPlayer cpu;
    private void Start()
    {
        ChangeState<InitBattleState>();
    }
}
