using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//STR이 높은 순서대로 유닛 정렬
//턴완료시 CTR 감소
//todo 이거 손봐야할듯
public class TurnOrderController : MonoBehaviour
{
    //턴비용(ex:공격만 하면 기본코스트 + 200 증가)
    #region
    const int turnActivation = 70;
    const int turnCost = 40;
    const int moveCost = 20;
    const int actionCost = 10;
    #endregion

    //델리게이트 키값들

    #region Notifications
    //라운드가 시작될때 사용하는 키
    public const string RoundBeganNotification = "TurnOrderController.roundBegan";
    //누구의 턴이 시작될지 체크할때 사용하는 키
    public const string TurnCheckNotification = "TurnOrderController.turnCheck";
    //특정 유닛의 턴이 완료 되었을때 사용하는 키
    public const string TurnComplatedNotification = "TurnOrderController.turnComplated";
    //라운드가 종료되었을때 사용하는 키
    public const string RoundEndedNotification = "TurnOrderController.roundEnded";
    public const string TurnBeganNotification = "TurnOrderController.TurnBeganNotification";
    #endregion

    public IEnumerator Round()
    {
        //BattleController 참조
        BattleController bc = GetComponent<BattleController>();
        while(true)
        {
            //turnordercontroller.roundbegab 키를 가진 델리게이트 호출
            this.PostNotification(RoundBeganNotification);

            //모든 유닛 정보를 list에 저장
            List<Unit> units = new List<Unit>(bc.units);
            for(int i=0; i<units.Count;++i)
            {
                Stats stats = units[i].GetComponent<Stats>();
                //매턴 마다 각 유닛이 SPD만큼 CTR이 증가
                //todo 나중에 이곳 손보면 될듯 AP로
                stats[StateTypes.AP] += stats[StateTypes.SPD];
                if(stats[StateTypes.AP] > 100)
                {
                    stats[StateTypes.AP] = 100;
                }
            }
            //CTR이 높은 순서대로 정렬
            units.Sort((a, b) => GetCounter(a).CompareTo(GetCounter(b)));

            for(int i=units.Count-1;i>=0;--i)
            {
                //해당 유닛의 CTR이 전보다 크거나 같은지 확인
                if(CanTakeTurn(units[i]))
                {
                    //CTR이 전보다 크거나 같으면 
                    //해당 유닛의 턴으로 변경
                    bc.turn.Change(units[i]);

                    //턴이 시작될 유닛이 결정되면 델리게이트 호출
                    units[i].PostNotification(TurnBeganNotification);
                    
                    //해당 유닛의 프레임이 종료 될때 까지 대기
                    //SelectUnitState의 ChangeCurrentUnit 코루틴 함수의
                    //owner.round.MoveNext 에서 종료됨
                    yield return units[i];

                    //기본 턴 코스트 40
                    int cost = turnCost;

                    //유닛이 이동을 했다면
                    if (bc.turn.hasUnitMoved)
                        //기본코스트에 20을 더함
                        cost += moveCost;
                    //유닛이 공격을 했다면
                    if (bc.turn.hasUnitActed)
                        //기본 코스트에 10을 더함
                        cost += actionCost;

                    Stats stats = units[i].GetComponent<Stats>();
                    // cost만큼 CTR 수치를 감소
                    // 행동 종료후 남은 AP를 계산하는 함수
                    stats.SetValue(StateTypes.AP, stats[StateTypes.AP] - cost, false);
                    
                    //TurnOrderController.turnComplate 키를 가진 델리게이트 호출
                    units[i].PostNotification(TurnComplatedNotification);
                }
            }
        }
    }
    bool CanTakeTurn(Unit target)
    {
        //유닛의 CTR수치가 1000보다 크거나 같으면 exc.toggle에 true 저장
        //1000보다 작으면 exc.toggle에 false 저장
        BaseException exc = new BaseException(GetCounter(target) >= turnActivation);
        
        //체크가 종료되면 turnchecknotification키로 된 델리게이트 호출(exc 전달)
        target.PostNotification(TurnCheckNotification, exc);
        //exc.toggle 반환
        return exc.toggle;
    }
    int GetCounter(Unit target)
    {
        //인덱서로 CTR수치를 반환
        return target.GetComponent<Stats>()[StateTypes.AP];
    }
}


// AP는 무조건로 시작
// 턴 시작시 마다 SPD를 가져와서 AP를 채움 EX) SPD = 100 AP = 0 -> AP = 100
// 전투가 시작됨 무조건 AP를 소모 TurnCost 50  AP = 50
// 선택지가 나뉨 1.대기,2.이동,3.공격 4. 스킬  AP 사용 
//      이동 COST 에 +20, 공격 +30, 이동 공격 +50
// 1. 대기시 그냥 지나감 그럼 다음 턴에 AP = 150 
// 2. 이동시 다음턴 AP = 120
// 3. 공격시 다음턴 AP = 130
// 4. 이동 공격시 다음턴 AP = 100