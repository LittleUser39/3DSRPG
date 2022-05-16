using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;

public static class JobParser
{
    //상단 메뉴에 parseJobs라는 메뉴 생성
    //이 아래의 함수가 해당 버튼을 눌렀을 때 실행되는 함수
   [MenuItem("Pre Production/Parse Jobs")]
   public static void Parse()
    {
        //경로 생성
        CreateDirectories();
        //유닛의 시작 능력치 설정
        ParseStartingStats();
        //유닛의 성장 능력치를 설정
        ParseGrowthStats();

        //만들어진 애셋들을 유니티 에디터에 저장
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    static void CreateDirectories()
    {
        //Assets/Resources 경로에 Jobs 폴더가 있는지 확인
        if(!AssetDatabase.IsValidFolder("Assets/Resources/Jobs"))
        {
            //없으면 해당 경로에 Jobs 폴더를 생성
            AssetDatabase.CreateFolder("Assets/Resources", "Jobs");
        }
    }

    //영웅의 처음 시작 능력치를 저장
    static void ParseStartingStats()
    {
        //JobStartingStats.csv 파일이 저장된 경로를 저장
        string readPath = string.Format("{0}/Settings/JobStartingStats.csv", Application.dataPath);

        //JobStartingStats.csv 파일을 열고 모든 줄을 readText에 저장
        string[] readText = File.ReadAllLines(readPath);

        for(int i=1;i<readText.Length;++i)
        {
            //행 단위로 PartsStartingStarts 함수에 보냄
            PartsStartingStats(readText[i]);
        }
    }

    //그러면 여기서 처리
    static void PartsStartingStats(string line)
    {
        //,쉼표 기준으로 배열에 담는다
        string[] elements = line.Split(',');

        //elements[0]는 직업 이름이 들어감
        //해당 경로에 프리팹을 만들고 참조
        GameObject obj = GetOrCreate(elements[0]);
        Job job = obj.GetComponent<Job>();

        //+1 한 이유는 처음에는 영웅의 이름이 들어가기 때문에 해줌
        for (int i = 1; i < Job.statOrder.Length + 1; ++i)
        {
            //해당값을 32비트 부호있는 정수로 변환
            //baseStats에 해당 능력치 값을 넣는다
            job.baseStats[i - 1] = Convert.ToInt32(elements[i]);
        }
        //회피에 대한 능력치를 설정
        StatModifierFeature evade = GetFeature(obj, StateTypes.EVD);
        evade.amount = Convert.ToInt32(elements[8]);
        //저항에 대한 능력치를 설정
        StatModifierFeature res = GetFeature(obj, StateTypes.RES);
        res.amount = Convert.ToInt32(elements[9]);
        //이동에 대한 능력치 설정
        StatModifierFeature move = GetFeature(obj, StateTypes.MOV);

        //StatModifierFeature.type 이 Move인 녀석에게
        //amount(값)를 설정
        //이게 지금 scv에 있는 10번째 있는 값을 move값에 저장해주는 행동인거 같음
        move.amount = Convert.ToInt32(elements[10]);

        //StatModifierFeature.type이 jump인 녀석에게 amount 설정
        StatModifierFeature jump = GetFeature(obj, StateTypes.JMP);
        jump.amount = Convert.ToInt32(elements[11]);
    }

    //영웅 레벨업 성장 능력치 저장
    static void ParseGrowthStats()
    {
        //jobGrowthstats.csv 파일이 저장된 경로를 저장
        string readPath = string.Format("{0}/Settings/JobGrowthStats.csv", Application.dataPath);

        //scv 파일을 열고 모든 줄을 readtext에 저장
        string[] readText = File.ReadAllLines(readPath);
    
        for(int i=1;i<readText.Length;++i)
        {
            //행단위로 ParseGrowthStats 함수에 보냄
            ParseGrowthStats(readText[i]);
        }
    }
    static void ParseGrowthStats(string line)
    {
        //,쉼표 기준으로 배열에 담음
        string[] elements = line.Split(',');

        //elements[0]는 직업 이름이 들어감
        //해당 경로에 프리팹을 만들고 참조
        GameObject obj = GetOrCreate(elements[0]);

        Job job = obj.GetComponent<Job>();
        for (int i = 1; i < elements.Length; ++i)
            job.growStats[i - 1] = Convert.ToSingle(elements[i]);
    }

    static StatModifierFeature GetFeature(GameObject obj,StateTypes type)
    {
        //obj에 추가된 모든 statmodifierfeature 컴포넌트 참조
        StatModifierFeature[] smf = obj.GetComponents<StatModifierFeature>();
        for(int i=0;i<smf.Length;++i)
        {
            //type이 동일한 것을 찾고 반환
            if (smf[i].type == type)
                return smf[i];
        }

        //해당 오브젝트에 StatModifierFeature가 없을경우
        //추가후
        StatModifierFeature feature = obj.AddComponent<StatModifierFeature>();

        //type을 설정하고 반환
        feature.type = type;
        return feature;
    }

    static GameObject GetOrCreate(string jobName)
    {
        //파일 경로를 생성
        string fullPath = string.Format("Assets/Resources/Jobs/{0}.Prefab", jobName);

        //해당 경로의 오브젝트를 obj에 담음
        GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(fullPath);
        if(obj==null)
        {
            //없으면 생성
            obj = Create(fullPath);
        }
        return obj;
    }

    //만든다
    static GameObject Create(string fullPath)
    {
        GameObject instance = new GameObject("temp");

        //job 컴포넌트 추가
        instance.AddComponent<Job>();

        //해당경로에 temp 프리팹을 만듬
        //프리팸의 이름은 fullPath에 입력된 이름으로 저장
        GameObject prefab = PrefabUtility.CreatePrefab(fullPath, instance);

        //생성된 temp 게임오브젝트는 제거
        GameObject.DestroyImmediate(instance);
        return prefab;
    }
}
