using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMenuPanelController : MonoBehaviour
{
    //애니메이션에서 사용
    const string ShowKey = "Show";
    const string HideKey = "Hide";

    //오브젝트 pool로 게임 오브젝트를 만들때 필요한 변수
    const string EntryPoolKey = "AbilityMenuPanel.Entry";
    const int MenuCount = 4;

    [SerializeField] GameObject entryPrefab; //버튼 오브젝트
    [SerializeField] Text titleLabel; //제목
    [SerializeField] Panel panel;   //abilityMenuPanel 오브젝트의 panel 컴포 
    [SerializeField] GameObject canvas;

    List<AbilityMenuEntry> menuEntries = new List<AbilityMenuEntry>(MenuCount);

    //현재 선택된 버튼의 menuEntries에서의 배열번호
    public int selection { get; private set; }

    private void Awake()
    {
        GameObjectPoolController.AddEntry(EntryPoolKey, entryPrefab, MenuCount, int.MaxValue);
    }
    private void Start()
    {
        panel.SetPosition(HideKey, false);

        canvas.SetActive(false);
    }
    
    bool SetSelection(int value)
    {
        //잠금상태이면 선택 불가
        if (menuEntries[value].isLocked) return false;

        //이전에 선택한 항목 선택 취소
        if(selection>=0&&selection<menuEntries.Count)
        {
            menuEntries[selection].IsSelected = false;
        }

        //selection을 방금 선택한 버튼의 번호로 설정
        selection = value;

        //방금 선택한 버튼을 선택중 상태로 변겅
        if(selection>=0&selection<menuEntries.Count)
        {
            menuEntries[selection].IsSelected = true;
        }

        return true;
    }

    public void SetLocked(int index,bool value)
    {
        if (index < 0 || index >= menuEntries.Count) return;
        menuEntries[index].isLocked = value;

        //버튼의 상태를 변경, 버튼이 잠금상태, 선택된 버튼과 잠금된 버튼이 동일한 버튼이면
        if(value&&selection==index)
        {
            //선택된 버튼을 다음 번호 버튼으로 변경
            Next();
        }
    }

    //메뉴판 보기
    public void Show(string title,List<string>option)
    {
        canvas.SetActive(true);
        Clear();

        //제목변경
        titleLabel.text = title;
    
        for(int i=0;i<option.Count;++i)
        {
            //obectpool로 만든 버튼 오브젝트 가져오기
            AbilityMenuEntry entry = Dequeue();

            //버튼 제목
            entry.Title = option[i];

            //메뉴판의 버튼리스트에 추가
            menuEntries.Add(entry);
        }
        //0번 버튼을 선택상태로 만듬
        SetSelection(0);

        //메뉴판 애니메이션 시작
        TogglePos(ShowKey);
    }

    //메뉴판 감추기
    public void Hide()
    {
        //애니메이션 재생
        Tweener t = TogglePos(HideKey);

        //메뉴판 감추기 애니가 완료되면
        t.completedEvent += delegate (object sender, System.EventArgs e)
          {
              //메뉴판 상태 초기화 및 ui 비활성화 시키기
              if (panel.CurrentPosition == panel[HideKey])
              {
                  Clear();
                  canvas.SetActive(false);
              }
          };
    }

    AbilityMenuEntry Dequeue()
    {
        //미리 만들어놓은 entryPrefab을 하나 꺼내옴
        Poolable p = GameObjectPoolController.Dequeue(EntryPoolKey);

        //abilitymenuentry를 참조시킴
        AbilityMenuEntry entry = p.GetComponent<AbilityMenuEntry>();

        //Hierarchy 뷰에서의 부모 지정,위치 초기화,활성화 시킴
        entry.transform.SetParent(panel.transform, false);
        entry.transform.localScale = Vector3.one;
        entry.gameObject.SetActive(true);
        entry.Reset();
        return entry;
    }

    void Enqueue(AbilityMenuEntry entry)
    {
        //버튼을 비활성화 시킴
        Poolable p = entry.GetComponent<Poolable>();
        GameObjectPoolController.Enqueue(p);
    }

    //메뉴판 상태를 초기화 시킴
    void Clear()
    {
        for(int i=menuEntries.Count-1;i>=0;--i)
        {
            Enqueue(menuEntries[i]);
        }
        menuEntries.Clear();
    }

    //ui애니메이션과 관련된 함수
    Tweener TogglePos(string pos)
    {
        Tweener t = panel.SetPosition(pos, true);
        t.duration = 0.5f;
        t.equation = EasingEquations.EaseOutQuad;
        return t;
    }

    //다음 번호 버튼 선택하기
    public void Next()
    {
        //락이 걸린애는 건너뛰어야 하니 for문
        for (int i=selection+1;i<selection+menuEntries.Count;++i)
        {
            int index = i % menuEntries.Count;
            if (SetSelection(index)) break;
        }
    }

    //이전 번호 버튼 선택하기
    public void Previous()
    {
        for(int i=selection-1+menuEntries.Count;i>selection;--i)
        {
            int index = i % menuEntries.Count;
            if (SetSelection(index))
                break;
        }
    }
}
