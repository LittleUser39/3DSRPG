using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationController : MonoBehaviour
{
    //이벤트 핸들러
    public static event EventHandler completeEvent;

    const string ShowTop = "Show Top";
    const string ShowBottom = "Show Bottom";
    const string HideTop = "Hide Top";
    const string HideBotton = "Hide Bottom";

    [SerializeField] ConversationPanel leftPanel;
    [SerializeField] ConversationPanel rightPanel;

    Canvas canvas;

    IEnumerator conversation;

    Tweener transition;

    private void Start()
    {
        canvas = GetComponentInChildren<Canvas>();

        if (leftPanel.panel.CurrentPosition == null)
            leftPanel.panel.SetPosition(HideBotton, false);
        if (rightPanel.panel.CurrentPosition == null)
            rightPanel.panel.SetPosition(HideBotton, false);

        canvas.gameObject.SetActive(false);
    }

    public void Show(ConversationData data)
    {
        canvas.gameObject.SetActive(true);
        conversation = Sequence(data);
        conversation.MoveNext();    //코루틴 시작
    }

    public void Next()
    {
        if (conversation == null || transition != null)
            return;

        conversation.MoveNext(); //코루틴 시작
    }

    IEnumerator Sequence(ConversationData data)
    {
        for(int i=0;i<data.list.Count;++i)
        {
            SpeakerData sd = data.list[i];

            ConversationPanel currentPanel
                = (sd.anchor == TextAnchor.UpperLeft || sd.anchor == TextAnchor.MiddleLeft || sd.anchor == TextAnchor.LowerLeft) ? leftPanel : rightPanel;


            IEnumerator presenter = currentPanel.Display(sd);

            presenter.MoveNext();

            string show, hide;

            // 어떤 키값으로 UI 연출을 할 것인지 체크
            if (sd.anchor == TextAnchor.UpperLeft || sd.anchor == TextAnchor.UpperCenter || sd.anchor == TextAnchor.UpperRight)
            {
                show = ShowTop;
                hide = HideTop;
            }
            else
            {
                show = ShowBottom;
                hide = HideBotton;
            }
            currentPanel.panel.SetPosition(hide, false);
            MovePanel(currentPanel, show);

            yield return null;


            while (presenter.MoveNext())
                yield return null;

            MovePanel(currentPanel, hide);

            transition.completedEvent += delegate (object sender, EventArgs e)
              {
                  conversation.MoveNext();
              };
            yield return null;
        }
        canvas.gameObject.SetActive(false);

        if (completeEvent != null)
            completeEvent(this, EventArgs.Empty);
    }
    void MovePanel(ConversationPanel obj,string pos)
    {
        transition = obj.panel.SetPosition(pos, true);
        transition.duration = 0.5f;
        transition.equation = EasingEquations.EaseOutQuad;
    }
}
