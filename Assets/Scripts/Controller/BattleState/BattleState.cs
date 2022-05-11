using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleState : State
{
    protected BattleController owner;

    //아래 변수들은 battlecontroller가 가지고 있는 변수
    public CameraRig CameraRig { get { return owner.cameraRig; } }
    public Board board { get { return owner.board; } }
    public LevelData levelData { get { return owner.leveldata; } }
    public Transform tileselection { get { return owner.tileselection; } }
    public Point pos { get { return owner.pos; } set { owner.pos = value; } }

    protected virtual void Awake()
    {
        owner = GetComponent<BattleController>();
    }

    //inputcontroller의 moveevent와 fireevent 핸들러에 함수를 등록
    //addlisteners는 해당 state 상태로 변경시 호출
    protected override void AddListeners()
    {
        InputController.moveEvent += OnMove;
        InputController.fireEvent += OnFire;
    }

    protected override void RemoveListeners()
    {
        InputController.moveEvent -= OnMove;
        InputController.fireEvent -= OnFire;
    }
    protected virtual void OnMove(object sender,InfoEventArgs<Point>e)
    {
      
    }

    protected virtual void OnFire(object Sender, InfoEventArgs<int>e)
    {
      
    }
    protected virtual void SelectTile(Point p)
    {
        if (pos == p || !board.tiles.ContainsKey(p))
            return;
        pos = p;
        tileselection.localPosition = board.tiles[p].center;
    }
}
