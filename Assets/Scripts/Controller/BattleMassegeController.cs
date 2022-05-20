using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//능력 이름을 보여주는 검은색 막대를 추가하는 클래스
//todo 나중에 바꿔줘야할듯
//UI요소를 페이드 인하고 잠시 화면에서 머물게 한 다음 다시 페이드 아웃
public class BattleMassegeController : MonoBehaviour
{
    [SerializeField] Text label;
    [SerializeField] GameObject canvas;
    [SerializeField] CanvasGroup group;
    EasingControl ec;

    private void Awake()
    {
        ec = gameObject.AddComponent<EasingControl>();
        ec.duration = 0.5f;
        ec.equation = EasingEquations.EaseInQuad;
        ec.endBehaviour = EasingControl.EndBehaviour.Constant;
        ec.updateEvent += OnUpdateEvent;
    }
    public void Display(string message)
    {
        group.alpha = 0;
        canvas.SetActive(true);
        label.text = message;
        StartCoroutine(Sequence());
    }
    void OnUpdateEvent(object sender,EventArgs e)
    {
        group.alpha = ec.currentValue;
    }
    IEnumerator Sequence()
    {
        ec.Play();

        while (ec.IsPlaying)
            yield return null;

        yield return new WaitForSeconds(1);

        ec.Reverse();

        while (ec.IsPlaying)
            yield return null;

        canvas.SetActive(false);
    }
}
