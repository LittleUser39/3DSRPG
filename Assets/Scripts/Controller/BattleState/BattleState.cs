using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleState : State
{
    protected BattleController owner;
    //battlecontroller에 전역변수들을 참조
    public AbilityMenuPanelController AbilityMenuPanelController { get { return owner.abilityMenuPanelController; } }
    public Turn turn { get { return owner.turn; } }
    public List<Unit> units { get { return owner.units; } }
    //아래 변수들은 battlecontroller가 가지고 있는 변수
    public CameraRig CameraRig { get { return owner.cameraRig; } }
    public Board board { get { return owner.board; } }
    public LevelData levelData { get { return owner.leveldata; } }
    public Transform tileselection { get { return owner.tileselection; } }
    public Point pos { get { return owner.pos; } set { owner.pos = value; } }
    public StatPanelController StatPanelController { get { return owner.StatPanelController; } }
    public HitSuccessIndicator HitSuccessIndicator { get { return owner.HitSuccessIndicator; } }
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

    protected virtual Unit GetUnit(Point p)
    {
        //선택된 타일의 정보를 참조
        Tile t = board.GetTile(p);

        //선택된 타일이 null이 아니라면
        //선택된 타일에 있는 유닛 정보를 반환
        GameObject content = t != null ? t.content : null;

        //선택된 타일에 있는 유닛 정보를 반환
        return content != null ? content.GetComponent<Unit>() : null;
    }
    
    protected virtual void RefreshPrimaryStatPanel(Point p)
    {
        Unit target = GetUnit(p);
        if(target!=null)
        {
            //대상의 정보를 표시
            StatPanelController.ShowPrimary(target.gameObject);
        }
        else
        {
            //해당 UI 숨기기
            StatPanelController.HidePrimary();
        }
    }

    protected virtual void RefreshSecondaryStatPanel(Point p)
    {
        Unit target = GetUnit(p);
        if (target != null)
        {
            //대상의 정보를 표시
            StatPanelController.ShowSecondary(target.gameObject);
        }
        else
        {
            //해당 UI 숨기기
            StatPanelController.HideSecondary();
        }
    }
}
