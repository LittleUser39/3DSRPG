using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitBattleState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Init());
    }
    IEnumerator Init()
    {
        //타일맵 로드
        board.Load(levelData);

        //현재 선택된 타일인디게이터(게임오브젝트)의 좌표를 설정
        Point p = new Point((int)levelData.tiles[0].x, (int)levelData.tiles[0].z);
        SelectTile(p);
        

        //현재 상태를 movetargetstate로 변경
        owner.ChangeState<MoveTargetState>();
        
        // 임시 코드(영웅을 소환)
        SpawnTestUnits();

        //라운드 코루틴을 등록
        owner.round = owner.gameObject.AddComponent<TurnOrderController>().Round();
        
        //한프레임 쉰다
        yield return null;

        // 현재 상태를 SelectUnitState로 변경한다.
        owner.ChangeState<CutSceneState>();
    }

// 임시 함수
    void SpawnTestUnits()
    {
        string[] jobs = new string[] { "Rogue", "Warrior", "Wizard" };

        for(int i=0;i<jobs.Length;++i)
        {
            GameObject instance = Instantiate(owner.heroPrefab) as GameObject;

            //heroprefab에 stats 컴포넌트 추가
            Stats s = instance.AddComponent<Stats>();

            //레벨 1
            s[StateTypes.LVL] = 1;

            //Resources/Jobs 폴더에 있는 프리팹을 로드
            GameObject jobPrefab = Resources.Load<GameObject>("Jobs/" + jobs[i]);

            //Hierarchy 뷰에 생성
            GameObject jobInstance = Instantiate(jobPrefab) as GameObject;

            //heroPrefab의 자식오브젝트로 생성
            jobInstance.transform.SetParent(instance.transform);

            //job에 job컴포넌트 가져옴
            Job job = jobInstance.GetComponent<Job>();
            
            //job 이라는 직업 생성 초기능력치 설정됨
            job.Empoly();

            //성장형 능력치 설정됨
            job.LoadDefaultStats();
            
            //시작위치(스폰위치)
            //todo 나중에 이거 바꿔주면 될듯
            Point p = new Point((int)levelData.tiles[i].x, (int)levelData.tiles[i].z);
            Unit unit = instance.GetComponent<Unit>();
            unit.Place(board.GetTile(p));
            unit.Match();

            //이동 방법 걷기
            instance.AddComponent<WalkMoveMent>();
            units.Add(unit);
        }
    }
}


//레벨설정 EX)
//Rank rank=instance.AddComponent<Rank>();
//rank.Init(10);



