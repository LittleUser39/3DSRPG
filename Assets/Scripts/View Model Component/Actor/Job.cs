using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job : MonoBehaviour
{
    //읽기 전용 아래 baseStas또는 growStats의 각배열에 능력치를 확인
    //CSV 테이블의 능력치 순서와 동일해야함
    //CSV에 있는 숫자를 가지고 해당 능력치로 설정한 것 같음
    public static readonly StateTypes[] statOrder = new StateTypes[]
    {
        StateTypes.MHP,
        StateTypes.MMP,
        StateTypes.ATK,
        StateTypes.DEF,
        StateTypes.MAT,
        StateTypes.MDF,
        StateTypes.SPD
    };

    //시작 능력치
    public int[] baseStats = new int[statOrder.Length];

    //성장 능력치
    public float[] growStats = new float[statOrder.Length];

    Stats stats;

    private void OnDestroy()
    {
        this.RemoveObserver(OnLvlChangeNotification, Stats.DidChangeNotification(StateTypes.LVL),stats);
    }

    //영웅을 고용하는 함수
    public void Empoly()
    {
        stats = gameObject.GetComponentInParent<Stats>();

        this.AddObserver(OnLvlChangeNotification, Stats.DidChangeNotification(StateTypes.LVL), stats);
        //해당 오브젝트의 자식들이 가지고 있는 feature 를 참조
        //feature 를 상속받은 대상도 포함
        
        Feature[] features = GetComponentsInChildren<Feature>();

        for(int i=0;i<features.Length;++i)
        {
            features[i].Activate(gameObject);
        }
    }

    //영웅을 해제 하는 함수
    public void UnEmpoly()
    {
        Feature[] features = GetComponentsInChildren<Feature>();
        for(int i=0;i<features.Length;++i)
        {
            features[i].Deactivate();
        }

        this.RemoveObserver(OnLvlChangeNotification, Stats.DidChangeNotification(StateTypes.LVL), stats);
        stats = null;
    }

    public void LoadDefaultStats()
    {
        //statOrder는 레벨에 따라 변경되는 능력치 리스트
        //읽기 전용 , 레벨에 따라 변경하는 능력치를 시작값으로 설정
        for(int i=0; i<statOrder.Length;++i)
        {
            StateTypes type = statOrder[i];
            stats.SetValue(type, baseStats[i], false);
        }

        //체력과 MP를 MAX값으로 설정
        //HP,MP는 현재 체력으로 테이블에서 따로 관리 안함
        stats.SetValue(StateTypes.HP, stats[StateTypes.MHP], false);
        stats.SetValue(StateTypes.MP, stats[StateTypes.MMP], false);
    }

    //레벨이 변경될때 호출, 델리게이트로 호출
    protected virtual void OnLvlChangeNotification(object sender,object args)
    {
        int oldValue = (int)args;
        int newValue = stats[StateTypes.LVL];
        for (int i= oldValue; i < newValue; ++i)
            LevelUp();
    }

    //레벨업
    void LevelUp()
    {
        //레벨업에 따른 능력치 증가
        for(int i=0;i<statOrder.Length;++i)
        {
            //statOrder에 있는 변수들을 type에 넣어서
            StateTypes type = statOrder[i];

            //whole에 성장 능력치 값 참조하여 저장
            int whole = Mathf.FloorToInt(growStats[i]);
            
            //소수점 값만 참조
            float fraction = growStats[i] - whole;

            //현재 능력치 값
            int value = stats[type];

            //현재능력치에 성장능력치 증가
            value += whole;

            //0~1사이의 랜덤값이 1-fraction보다 크면
            if(UnityEngine.Random.value>(1f-fraction))
            {
                //능력치 1증가
                value++;
            }

            //변경된 값을 능력치에 적용시킴
            stats.SetValue(type, value, false);
        }

        //레벨업이 되면 현재 HP와 MP는 MAX
        stats.SetValue(StateTypes.HP, stats[StateTypes.MHP], false);
        stats.SetValue(StateTypes.MP, stats[StateTypes.MMP], false);
    }

}
