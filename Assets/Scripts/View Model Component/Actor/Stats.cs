using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//캐릭터의 능력치 리스트를 가짐
//변경된 능력치의 값을 적용시키는 코드
public class Stats : MonoBehaviour
{
    //인덱서 라는것
    //클래스나 구조체에서 특정값에 자연스러운 구문으로 접근하기 위한 기능
    //다른 클래스에 _data에 접근하면 stats[StatTypes.EXP]; 이런 형태로 접근
    public int this[StateTypes s]
    {
        get { return _data[(int)s]; }
        set { SetValue(s, value, true); }
    }
    int[] _data = new int[(int)StateTypes.Count];

    
    public void SetValue(StateTypes type,int value,bool allowExceptions)
    {
        //data[type] 값이 oldvalue 에 들어간다(기존 수치)
        int oldValue = this[type];

        //현재 수치와 변경될 수치가 같으면 리턴
        if (oldValue == value)
        {
            return;
        }
        //이게 true 이면 아마 랜덤으로 계산한다는 것일듯?
        if (allowExceptions)
        {
            //계산기를 만듬, 아직 어떤 계산이 될지 미정
            ValueChangeException exc = new ValueChangeException(oldValue, value);

            //Stats.{0}WillChange 키값을 가진 델리게이트 호출
            //이때 어떤 식으로 계산이 이뤄질지 결정 (곱하기,더하기 등)
            this.PostNotification(WillChangeNotification(type), exc);

            //등록된 계산 방식으로 최종값을 담음
            value = Mathf.FloorToInt(exc.GetModifiedValue());
           
            //BaseException.toggle 이 false 거나
            //value가 oldValue 값이 변경되지 않았다면
            if (exc.toggle == false || value == oldValue)
            {
                //무효처리
                return;
            }
        }

        //해당 능력치를 계산된 값으로 변경
        _data[(int)type] = value;

        //Stats.{0}WillChange 로 된 델리게이트들을 호출
        this.PostNotification(DidChangeNotification(type), oldValue);
    }

    //아래 두가지 타입의 델리게이트 키를 생성하는 코드
    public static string WillChangeNotification(StateTypes type)
    {
        if(!_willChangeNotifications.ContainsKey(type))
        {
            //없는키라면 추가
            _willChangeNotifications.Add(type, string.Format("Stats.{0}WillChange", type.ToString()));
        }
        return _willChangeNotifications[type];
    }
    public static string DidChangeNotification(StateTypes type)
    {
        if (!_didChangeNotifications.ContainsKey(type))
        {
            // _didChangeNotifications 에 없는 키라면 추가한다.
            _didChangeNotifications.Add(type, string.Format("Stats.{0}DidChange", type.ToString()));
        }

        return _didChangeNotifications[type];
    }

    static Dictionary<StateTypes, string> _willChangeNotifications = new Dictionary<StateTypes, string>();
    static Dictionary<StateTypes, string> _didChangeNotifications = new Dictionary<StateTypes, string>();

}