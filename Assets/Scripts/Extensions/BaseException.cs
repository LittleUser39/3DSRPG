using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//특정계산(경험치)을 진행OR중단 해서 무효처리 할지 여부 설정
//경험치를 랜덤으로 먹거나 못먹게 하는 클래스 나는 안쓸듯
public class BaseException
{
    public bool toggle { get; private set; }
    private bool defaultToggle;

    public BaseException(bool defaultToggle)
    {
        this.defaultToggle = defaultToggle;
        toggle = defaultToggle;
    }

    public void FlipToggle()
    {
        //toggle이 false가 되면
        //states.setvalue에서 계산을 중단해서 취소처리
        //todo 근데 나는 이거 안쓸꺼라 나중에 꺼버리면 될듯
        toggle = !defaultToggle;
    }
}
