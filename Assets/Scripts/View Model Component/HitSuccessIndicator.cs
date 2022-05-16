using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//공격,피격 연출 관련 UI 애니메이션을 동작시키는 클래스
public class HitSuccessIndicator : MonoBehaviour
{
    const string ShowKey = "Show";
    const string HideKey = "Hide";
    [SerializeField] Canvas Canvas;
    [SerializeField] Panel Panel;
    [SerializeField] Image arrow;
    [SerializeField] Text label;
    Tweener transition;
    private void Start()
    {
        Panel.SetPosition(HideKey, false);
        Canvas.gameObject.SetActive(false);
    }

    public void SetStats(int chance,int amount)
    {
        arrow.fillAmount = (chance / 100f);
        label.text = string.Format("{0}%{1}pt(s)", chance, amount);
    }
    public void Show()
    {
        Canvas.gameObject.SetActive(true);
        SetPanelPos(ShowKey);
    }
    public void Hide()
    {
        SetPanelPos(HideKey);
        transition.easingControl.completedEvent += delegate (object sender, System.EventArgs e)
          {
              Canvas.gameObject.SetActive(false);
          };
    }
    void SetPanelPos(string pos)
    {
        //해당 개체가 애니메이션이 재생중이면 앞에서 재생하던것을 멈춤
        if(transition!=null&&transition.easingControl.IsPlaying)
        {
            transition.easingControl.Stop();
        }
        //애니메이션 동작(pos에 저장된 위치로)
        transition = Panel.SetPosition(pos, true);
        transition.easingControl.duration = 0.5f;
        transition.easingControl.equation = EasingEquations.EaseInOutQuad;
    }
}
