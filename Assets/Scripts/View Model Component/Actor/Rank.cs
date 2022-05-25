using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//레벨에 관한 클래스
//경험치에 따른 레벨을 구함, 레벨에 따름 필요경험치를 구하여 State.cs에게 값을 전달
//레벨또는 경험치를 변경시키라고 하는 클래스
//옵저버 패턴 적용
public class Rank : MonoBehaviour
{
    public const int minLevel = 1;
    public const int maxLevel = 99;
    public const int maxExperience = 999999;
    Stats stats;

    //레벨 정보
    public int LVL
    {
        get
        {
            return stats[StateTypes.LVL];
        }
    }
    //경험치 정보
    public int EXP
    {
        get 
        { 
            return stats[StateTypes.EXP]; 
        }
        set
        {
            stats[StateTypes.EXP] = value;
        }
    }

    public float LevelPercent
    {
        //최대 레벨까지 도달량
        get { return (float)(LVL - minLevel) / (float)(maxLevel - minLevel); }
    }

    private void Awake()
    {
        stats = GetComponent<Stats>();
    }

    //델리게이트 등록 및 해제
    //옵저버 패턴
    private void OnEnable()
    {
        this.AddObserver(OnExpWillChange, Stats.WillChangeNotification(StateTypes.EXP), stats);
        this.AddObserver(OnExpDidChange, Stats.DidChangeNotification(StateTypes.EXP), stats);
    }
    private void OnDisable()
    {
        this.RemoveObserver(OnExpWillChange, Stats.WillChangeNotification(StateTypes.EXP), stats);
        this.RemoveObserver(OnExpDidChange, Stats.DidChangeNotification(StateTypes.EXP), stats);
    }

    void OnExpWillChange(object sender,object args)
    {
        //as ~~타입으로서, 타입 변환이 가능하면 타입 변환(캐스팅)
        //Exception의 타입을 object 타입으로서 사용하는것
        ValueChangeException vce = args as ValueChangeException;

        //경험치를 보정
        //maxExperience 범위를 넘어가지 않도록 보정
        vce.AddModifier(new ClampValueModifier(int.MaxValue, EXP, maxExperience));
    }

    void OnExpDidChange(object sender,object args)
    {
        //레벨이 증가되었는지 체크
        //증가되었으면 레벨 수치를 증가
        stats.SetValue(StateTypes.LVL, LevelForExperience(EXP), false);
    }
    
    public static int ExperienceForLevel(int level)
    {
        //최대 레벨 도달량
        float levelPercent = Mathf.Clamp01((float)(level - minLevel) / (float)(maxLevel - minLevel));

        //999999 * levelPercent * LevelPercent = 해당레벨에 필요한 경험치
        //최대 레벨이 되면 999999가 됨
        return (int)EasingEquations.EaseInOutQuad(0, maxExperience, levelPercent);
    }

    //레벨 반환
    public static int LevelForExperience(int exp)
    {
        //exp에 따른 레벨 계산
        int lvl = maxLevel;
        for (; lvl >= minLevel; --lvl)
        {
            if (exp >= ExperienceForLevel(lvl))
            {
                break;
            }
        }
        return lvl;
    }
    
    //레벨과 경험치 값을 적용
    public void Init(int level)
    {
        stats.SetValue(StateTypes.LVL, level, false);
        stats.SetValue(StateTypes.EXP, ExperienceForLevel(level), false);
    }
}
