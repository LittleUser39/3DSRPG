using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//list<GameObject>와 동인한 Party라는 네임스페이스를 만듬
using Party = System.Collections.Generic.List<UnityEngine.GameObject>;
public class TestLevelGrowth : MonoBehaviour
{
    //델리게이트에 함수등록
    //옵저버 패턴
    private void OnEnable()
    {
        this.AddObserver(OnLevelChange, Stats.DidChangeNotification(StateTypes.LVL));
        this.AddObserver(OnExperienceException, Stats.WillChangeNotification(StateTypes.EXP));
    }

    //델리게이트에 함수해제
    private void OnDisable()
    {
        this.RemoveObserver(OnLevelChange, Stats.DidChangeNotification(StateTypes.LVL));
        this.RemoveObserver(OnExperienceException, Stats.WillChangeNotification(StateTypes.EXP));
    }

    private void Start()
    {
        //각 레벨별 필요 경험치 표시
        VerifyLevelToExperienceCalculations();

        //임의 환경(영웅,레벨,경험치 등 세팅)만듬
        VerifySharedExperienceDistribution();
    }

    void VerifyLevelToExperienceCalculations()
    {
        for(int i=1;i<100;++i)
        {
            //i레벨에서 필요한 경험치를 expLvl에 담는다
            int expLvl = Rank.ExperienceForLevel(i);

            //expLvl 따른 레벨(확인용도)
            int lvlExp = Rank.LevelForExperience(expLvl);

            if(lvlExp!=i)
            {
                //레벨과 경험치가 잘못되었을 경우
                //최대레벨을 초과했을때 등 버그 상황에서의 로그
                Debug.Log(string.Format("Mismatch on level:{0} with exp:{1} returned:{2}", i, expLvl, lvlExp));
            }
            else
            {
                //레벨과 해당레벨에서 필요한 경험치를 log에 남김
                Debug.Log(string.Format("Level:{0}=Exp{1}", lvlExp, expLvl));
            }

        }
    }
    void VerifySharedExperienceDistribution()
    {
        //영웅목록
        string[] names = new string[] { "주인공", "친구" };//이름 입력

        //party 는 List<GameObject>와 같은 의미
        Party heroes = new Party();

        for(int i=0;i<names.Length;++i)
        {
            GameObject actor = new GameObject(names[i]);
            actor.AddComponent<Stats>();
            Rank rank = actor.AddComponent<Rank>();

            //레벨을 1~5 중 랜덤값
            rank.Init((int)UnityEngine.Random.Range(1, 5));
            heroes.Add(actor);
        }
        Debug.Log("Before Adding Exp");
        
        //이름 레벨 경험치를 로그에 표시
        LogParty(heroes);
        Debug.Log("===================");

        //경험치 추가하기(레벨이 높을수록 낮은 경험치가 추가됨)
        //이게 경험치 추가
        //ExperienceManger.AwardExperience(1000, heroes);
        Debug.Log("After Adding EXP");

        LogParty(heroes);
    }

    //이름 레벨 경험치를 로그에 표시
    void LogParty(Party p)
    {
        for(int i=0; i<p.Count;++i)
        {
            GameObject actor = p[i];
            Rank rank = actor.GetComponent<Rank>();
            Debug.Log(string.Format("Name:{0} Level:{1} Exp:{2}", actor.name, rank.LVL, rank.EXP));
        }
    }

    void OnLevelChange(object sender,object args)
    {
        Stats stats = sender as Stats;
        Debug.Log(stats.name + "레벨업");
    }

    //여기서 경험치 2배 이벤트 하는거임
    //todo 나중에 바꿔야함
    void OnExperienceException(object sender,object args)
    {
        GameObject actor = (sender as Stats).gameObject;
        ValueChangeException vce = args as ValueChangeException;
        int roll = UnityEngine.Random.Range(0, 5);
    
        switch(roll)
        {
            case 0:
                // 경험치를 얻지 못하는 상황
                vce.FlipToggle();
                Debug.Log(string.Format("{0} would have received {1} experience, but we stopped it", actor.name, vce.delta));
                break;

            case 1:
                // 원래 경험치보다 훨씬 큰 경험치를 얻게되는 상황. 
                vce.AddModifier(new AddValueModifier(0, 1000));
                Debug.Log(string.Format("{0} would have received {1} experience, but we added 1000", actor.name, vce.delta));
                break;
            case 2:
                // 경험치를 2배 곱한다.
                vce.AddModifier(new MultValueModifier(0, 2f));
                Debug.Log(string.Format("{0} would have received {1} experience, but we multiplied by 2", actor.name, vce.delta));
                break;
            default:
                // 원래 경험치를 획득.
                Debug.Log(string.Format("{0} will receive {1} experience", actor.name, vce.delta));
                break;
        }
    }
}
