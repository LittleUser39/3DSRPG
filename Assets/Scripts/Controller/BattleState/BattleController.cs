﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : StateMachine
{
    //임시코드
    public GameObject heroPrefab;
    public Unit currentUnit;
    public Tile currentTile { get { return board.GetTile(pos); } }

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

    private void Start()
    {
        ChangeState<InitBattleState>();
    }
}
