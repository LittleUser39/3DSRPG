using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//경험치,레벨등의 수치들을 계산하는데 사용하는 클래스
public abstract class ValueModifier : Modifier
{
    //:base는 부모의 생성자를 호출하는 코드
    //생성자 호출시 매개변수로 받은 sortorder를 부모의 생성자 매개변수로 보낸다는 의미
    public ValueModifier(int sortOrder) : base(sortOrder) { }
    //추상화 메소드 이 클래스를 상속받은 자식클래스에서 반드시 선언해줘야하는 함수
    public abstract float Modify(float value,float toValue);
}
