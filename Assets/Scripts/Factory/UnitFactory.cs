using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//정적 클래스 인스턴스 데이터를 보유할 필요가 없다
//작업을 수행하는 모든것은 레시피 , 매개변수로 전달됨
//유닛의 설정을 변경하려면 팩토리 클래스만 수정하면 된다.
public static class UnitFactory
{
    //유닛의 레시피를 만드는 함수
    public static GameObject Create(string name,int level)
    {
        UnitRecipe recipe = Resources.Load<UnitRecipe>("Unit Recipes/" + name);
        if(recipe==null) 
        {
            Debug.LogError("No Unit Recipe for name:" + name);
            return null;
        }
        return Create(recipe, level);
    }

    //레시피를 받아 유닛을 생성하는 함수
    public static GameObject Create(UnitRecipe recipe,int level)
    {
        GameObject obj = InstantiatePrefab("Units/" + recipe.modle);
        obj.name = recipe.name;
        obj.AddComponent<Unit>();
        AddStats(obj);
        AddLocomotion(obj, recipe.locomotions);
        obj.AddComponent<Status>();
        obj.AddComponent<Equipment>();
        AddJob(obj, recipe.job);
        AddRank(obj, level);
        obj.AddComponent<Health>();
        obj.AddComponent<Mana>();
        AddAttack(obj, recipe.attack);
        AddAbilityCatalog(obj, recipe.abilityCatalog);
        AddAlliance(obj, recipe.alliances);
        AddAttackPattern(obj, recipe.strategy);
        return obj;
    }

    //매개 변수의 이름에 따라 리소스에서 게임오브젝트를 로드할려고 시도한다
    static GameObject InstantiatePrefab(string name)
    {
        GameObject prefab = Resources.Load<GameObject>(name);
        if (prefab == null)
        {
            Debug.LogError("No Prefab for name:" + name);
            return new GameObject(name);
        }
        GameObject instance = GameObject.Instantiate(prefab);
        instance.name = instance.name.Replace("(Clone)", "");
        return instance;
    }

    //처음 스탯을 추가하는 함수
    static void AddStats(GameObject obj)
    {
        Stats stats = obj.AddComponent<Stats>();
        stats.SetValue(StateTypes.LVL, 1, false);
    }

    //처음 직업을 추가하는 함수
    static void AddJob(GameObject obj,string name)
    {
        GameObject instance = InstantiatePrefab("Jobs/" + name);
        instance.transform.SetParent(obj.transform);
        Job job = instance.GetComponent<Job>();
        job.Empoly();
        job.LoadDefaultStats();
    }

    //이동 타입을 추가하는 함수
    static void AddLocomotion(GameObject obj,Locomotions type)
    {
        //열거형 Locomotions 에 있는 변수에 따라 컴포넌트 추가
        switch(type)
        {
            case Locomotions.Walk:
                obj.AddComponent<WalkMoveMent>();
                break;
            case Locomotions.Fly:
                obj.AddComponent<FlyMoveMent>();
                break;
            case Locomotions.Teleport:
                obj.AddComponent<TeleportMovement>();
                break;
        }
    }

    //동맹 설정 추가하는 함수
    static void AddAlliance(GameObject obj, Alliances type)
    {
        //열거행 Alliances에 있는 변수로 추가
        Alliance alliance = obj.AddComponent<Alliance>();
        alliance.type = type;
    }

    //레벨을 추가하는 함수
    static void AddRank(GameObject obj,int level)
    {
        Rank rank = obj.AddComponent<Rank>();
        rank.Init(level);
    }

    //스킬을 추가하는 함수
    static void AddAttack(GameObject obj,string name)
    {
        GameObject instance = InstantiatePrefab("Abilities/" + name);
        instance.transform.SetParent(obj.transform);
    }

    //능력에 대한 스킬 목록 추가
    static void AddAbilityCatalog(GameObject obj,string name)
    {
        //능력 카테고리를 obj의 자식오브젝트로 설정
        //catalog 컴포넌트 추가
        GameObject main = new GameObject("Ability Catalog");
        main.transform.SetParent(obj.transform);
        main.AddComponent<AbilityCatalog>();

        //리소스에서 스킬 가져오기
        //레시피 없으면 그냥 리턴
        AbilityCatalogRecipe recipe = Resources.Load<AbilityCatalogRecipe>("Ability Catalog Recipe/" + name);
        if(recipe==null)
        {
            Debug.LogError("No Ability Catalog Recipe Found:" + name);
            return;
        }

        //레시피에 있는 카테고리에 있는 스킬들 추가
        for(int i=0;i<recipe.categories.Length;++i)
        {
            GameObject category = new GameObject(recipe.categories[i].name);
            category.transform.SetParent(main.transform);
            for(int j=0;j<recipe.categories[i].entries.Length;++j)
            {
                string abilityName = string.Format("Abilities/{0}/{1}", recipe.categories[i].name, recipe.categories[i].entries[j]);
                GameObject ability = InstantiatePrefab(abilityName);
                //ability.name = recipe.categories[i].entries[j];
                ability.transform.SetParent(category.transform);
            }
        }
        
    }

    //유닛에 공격패턴 추가해주는 함수
    //조종을 컴퓨터가 한다면 패턴을 넣어줌
    static void AddAttackPattern(GameObject obj,string name)
    {
        Driver driver = obj.AddComponent<Driver>();
        if(string.IsNullOrEmpty(name))
        {
            driver.normal = Drivers.Human;
        }
        else
        {
            driver.normal = Drivers.Computer;
            GameObject instance = InstantiatePrefab("Attack Pattern/" + name);
            instance.transform.SetParent(obj.transform);
        }
    }
}
