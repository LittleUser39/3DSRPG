using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//계산들을 정렬하고 계산해줌(계산기 같은것)
public class ValueChangeException : BaseException
{
    public readonly float fromValue;
    public readonly float toValue;
    public float delta { get { return toValue - fromValue; } }
    List<ValueModifier> modifiers;

    public ValueChangeException(float fromValue,float toValue):base(true)
    {
        //BaseException() 생성자 호출
        this.fromValue = fromValue;
        this.toValue = toValue;
    }

    public void AddModifier(ValueModifier m)
    {
        if(modifiers==null)
        {
            modifiers = new List<ValueModifier>();
        }

        //계산방식을 추가
        //valueModifier를 상속받은 것들 중에
        //m으로 돌어온 녀석을 추가
        //addvaluemodifier,multvaluemodifier 등등
        modifiers.Add(m);
    }

    public float GetModifiedValue()
    {
        float value = toValue;

        //계산할것이 없다면 toValue 그대로 반환
        if (modifiers == null) return value;

        //modifiers 정렬
        modifiers.Sort(Compare);
    
        for(int i=0;i<modifiers.Count;++i)
        {
            value = modifiers[i].Modify(fromValue,value);
        }

        return value;
    }

    //정렬 기준
    int Compare(ValueModifier x,ValueModifier y)
    {
        //modifier 의 sortorder 로 정렬
        //클수록 뒤로 보냄
        //곱하기는 더하기보다 먼전 계산되어야하므로 정렬
        return x.sortOrder.CompareTo(y.sortOrder);
    }
}
