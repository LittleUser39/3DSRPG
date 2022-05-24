using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class DamageText : MonoBehaviour
{
    [SerializeField] Text label;
    [SerializeField] GameObject canvas;
    EasingControl ec;

    private void Awake()
    {
        ec = gameObject.AddComponent<EasingControl>();
        ec.duration = 1.5f;
        ec.equation = EasingEquations.EaseInQuad;
        ec.endBehaviour = EasingControl.EndBehaviour.Constant;
    }
   
    public void SetDamage(string target,int damge)
    {
        label.text = string.Format("{0}가{1}의 피해를 받음", target, damge);
    }
    public void Display()
    {
        canvas.SetActive(true);
        StartCoroutine(Sequence());
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
