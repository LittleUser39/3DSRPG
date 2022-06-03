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
    private static DamageText _instance;
    bool hit;
    bool heal;
    int damage;
    string targetname;
    public static DamageText instance
    {
        get { return _instance; }
    }


    private void Awake()
    {
        _instance = this;
        ec = gameObject.AddComponent<EasingControl>();
        ec.duration = 1.5f;
        ec.equation = EasingEquations.EaseInQuad;
        ec.endBehaviour = EasingControl.EndBehaviour.Constant;
    }
   
    public void SetHit(bool hit)
    {
        this.hit = hit;
    }
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
    public void SetHeal(bool heal)
    {
        this.heal = heal;
    }
    public void SetTarget(string name)
    {
        targetname = name;
    }
    public void SetText()
    {
        Debug.Log(hit);
        if (hit && !heal)
        {
            label.text = string.Format("{0}가 {1}의 피해를 받음", targetname, damage);
        }
        else if (hit && heal)
        {
            label.text = string.Format("{0}의 체력이 {1} 회복됨", targetname, damage);
        }
        else if(!hit && !heal)
        {
            label.text = string.Format("{0}가 공격을 회피", targetname);
        }

        
    } 
    
    public void Display()
    {
        SetText();
        canvas.SetActive(true);
        StartCoroutine(Sequence());
        heal = false;
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
