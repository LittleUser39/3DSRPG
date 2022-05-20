using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOption 
{
    //한쌍의 데이터를 보유하기 위해 mark라는 클래스 생성
    //개별 목록을 만드는것보다 오류가 덜 발생
   class Mark
    {
        public Tile Tile;
        public bool isMatch;

        public Mark(Tile tile,bool isMatch)
        {
            this.Tile = tile;
            this.isMatch = isMatch;
        }
    }

    //공격 타겟
    public Tile target;
    public Directions direction;
    public List<Tile> areaTargets = new List<Tile>();

    //이동 대상 위치 중 하나가 실제로 이동하기에 좋은지 여부 추적
    public bool isCasterMatch;
    
    //공격하기에 가장 좋은 점수를 제공하는 타일
    public Tile bestMoveTile { get; private set; }
    public int bestAngleBasedScore { get; private set; }
    
    
    List<Mark> marks = new List<Mark>();
    List<Tile> moveTargets = new List<Tile>();
    
    //공격 범위에 있는 위치 목록을 작성하는 함수
    public void AddMoveTarget(Tile tile)
    {
        //시전자에게 부정적인 영향을 줄 수 있는 타일로 이동하는 것을 허용하지 않는다
        if(!isCasterMatch&&areaTargets.Contains(tile))
        {
            return;
        }
        moveTargets.Add(tile);
    }

    //위에서 정의한 클래스의 인스턴스를 만들고 목록에 추가하는 함수
    public void AddMark(Tile tile,bool isMatch)
    {
        marks.Add(new Mark(tile, isMatch));
    }

    //턴 동안 사용할 수 있는 다양한 옵숀을 정렬 할 수 있는 점수를 제공
    //점수를 계산하기 전에 능력을 사용하기 위해 이동할 최상의 타일도 찾는다
    //두번째 요소 (공격 각도)에 따라 좋은 점수를 받는다
    public int GetScore(Unit caster,Ability ability)
    {
        GetBestMoveTarget(caster, ability);
        if(bestMoveTile==null)
        {
            return 0;
        }

        int score = 0;
        for(int i=0;i<marks.Count; ++i)
        {
            if(marks[i].isMatch)
            {
                score++;
            }
            else
            {
                score--;
            }
        }
        if(isCasterMatch&&areaTargets.Contains(bestMoveTile))
        {
            score++;
        }
        return score;
    }

    //이동할수 있는 모든 타일을 반복하고 그곳으로 유닛을 이동
    //시전자와 대상사이의 각도를 계산 하여 점수를 생성
    //점수가 이전보다 높으면 최고 옵션 목록을 재설정
    //점수가 가장 높은 타일에서 무작위로 선택
    //각도가 관련이 없으면 임의의 타일을 단순히 반환
    void GetBestMoveTarget(Unit caster,Ability ability)
    {
        if(moveTargets.Count==0)
        {
            return;
        }
        if(IsAbilityAngleBased(ability))
        {
            bestAngleBasedScore = int.MinValue;
            Tile startTile = caster.tile;
            Directions startDirection = caster.dir;
            caster.dir = direction;

            List<Tile> bestOption = new List<Tile>();
            for(int i=0;i<moveTargets.Count;++i)
            {
                caster.Place(moveTargets[i]);
                int score = GetAngleBasedScore(caster);
                if(score>bestAngleBasedScore)
                {
                    bestAngleBasedScore = score;
                    bestOption.Clear();
                }
                if(score==bestAngleBasedScore)
                {
                    bestOption.Add(moveTargets[i]);
                }
            }
            caster.Place(startTile);
            caster.dir = startDirection;

            FilterBestMoves(bestOption);
            bestMoveTile = bestOption[UnityEngine.Random.Range(0, bestOption.Count)];
        }
        else
        {
            bestMoveTile = moveTargets[UnityEngine.Random.Range(0, moveTargets.Count)];
        }
    }
    
    //적중률이 공격자와 방어자 사이의 각도에 따라 결정되는지 여부를 결정
     bool IsAbilityAngleBased(Ability ability)
    {
        bool isAngleBased = false;
        for(int i=0;i<ability.transform.childCount;++i)
        {
            HitRate hr = ability.transform.GetChild(i).GetComponent<HitRate>();
            if(hr.IsAngleBased)
            {
                isAngleBased = true;
                break;
            }
        }
        return isAngleBased;
    }

    //시전자에서 각 마크 위치까지의 각도를 얻는다
    //마크가 의도한 일치 여부에 따라 점수가 증가하거나 감소
   int GetAngleBasedScore(Unit caster)
    {
        int score = 0;
        for(int i=0;i<marks.Count;++i)
        {
            int value = marks[i].isMatch ? 1 : -1;
            int multiplier = MultiplierForAngle(caster, marks[i].Tile);
            score += value * multiplier;
        }
        return score;
    }

    //이동 하기에 가장 좋은 장소의 목록을 작성한후
    //나머지는 잠재적으로 추가한다
    void FilterBestMoves(List<Tile> list)
    {
        if (!isCasterMatch)
        {
            return;
        }
        bool canTargetSelf = false;
        for (int i = 0; i < list.Count; ++i)
        {
            if (areaTargets.Contains(list[i]))
            {
                canTargetSelf = true;
                break;
            }
        }
        if (canTargetSelf)
        {
            for (int i = list.Count - 1; i >= 0; --i)
            {
                if (!areaTargets.Contains(list[i]))
                {
                    list.RemoveAt(i);
                }
            }
        }
    }

    //공격 각도에 따라 이동 대상 옵션에 점수를 매기는데 도움이 된다
    int MultiplierForAngle(Unit caster,Tile tile)
    {
        if(tile.content==null)
        {
            return 0;
        }
        Unit defender = tile.content.GetComponentInChildren<Unit>();
        if(defender==null)
        {
            return 0;
        }
        Facing facing = caster.GetFacing(defender);
        if(facing==Facing.back)
        {
            return 90;
        }
        if(facing==Facing.side)
        {
            return 75;
        }
        return 50;
    }
}
