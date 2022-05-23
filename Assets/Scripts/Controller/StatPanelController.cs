using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatPanelController : MonoBehaviour
{
    //UI 애니메이션 키
    const string ShowKey = "Show";
    const string HideKey = "Hide";

    //본인 정보UI
    [SerializeField] StatPanel primaryPanel;

    //타겟 정보 UI
    [SerializeField] StatPanel secondaryPanel;

    //애니메이션 관련 클래스
    Tweener primatyTransition;
    Tweener secondaryTransition;

    private void Start()
    {
        if (primaryPanel.panel.CurrentPosition == null)
        {
            primaryPanel.panel.SetPosition(HideKey, false);
        }
        if (secondaryPanel.panel.CurrentPosition == null)
        {
            secondaryPanel.panel.SetPosition(HideKey, false);
        }
    }

    public void ShowPrimary(GameObject obj)
    {
        //유닛 정보 UI에 표시될 능력,이름 등 능력치
        primaryPanel.Display(obj);

        //유닛 정보 표시
        MovePanel(primaryPanel, ShowKey, ref primatyTransition);
    }
    public void HidePrimary()
    {
        //유닛 정보 UI 숨기기
        MovePanel(primaryPanel, HideKey, ref primatyTransition);
    }
    public void ShowSecondary(GameObject obj)
    {
        //타겟 정보 UI에 표시될 능력 이름 등 능력치
        secondaryPanel.Display(obj);

        MovePanel(secondaryPanel, ShowKey, ref secondaryTransition);
    }
    public void HideSecondary()
    {
        //타겟 정보 UI숨기기
        MovePanel(secondaryPanel, HideKey, ref secondaryTransition);
    }

    void MovePanel(StatPanel obj, string pos, ref Tweener t)
    {
        //UI를 표시하거나 감출때 애니메이션과 함께 처리
        Panel.Position target = obj.panel[pos];
        if (obj.panel.CurrentPosition != target)
        {
            if (t != null)
            {
                t.Stop();
            }
            t = obj.panel.SetPosition(pos, true);
            t.duration = 0.5f;
            t.equation = EasingEquations.EaseInOutQuad;
        }
    }
}

