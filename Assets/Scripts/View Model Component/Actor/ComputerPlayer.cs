using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//AI 전체 관리자 클래스
public class ComputerPlayer : MonoBehaviour
{
    BattleController bc;
    Alliance Alliance { get { return actor.GetComponent<Alliance>(); } }
    Unit nearestFoe;
    Unit actor { get { return bc.turn.actor; } }
    private void Awake()
    {
        bc = GetComponent<BattleController>();
    }
    public PlanOfAttack Evaluate()
    {
        //공격 계획 작성
        PlanOfAttack poa = new PlanOfAttack();

        //사용할 기능 결정
        AttackPattern pattern = actor.GetComponentInChildren<AttackPattern>();
        if(pattern)
        {
            pattern.Pick(poa);
        }
        else
        {
            DefaultAttackPattern(poa);
        }

        //능력을 맞추기위해 올바른 방향에 있어야한다
        //유닛의 방향이 정방향이 되도록
        //이동,캐스팅,회전시 잠재적으로 업데이트 한다
        if(IsPositionIndependent(poa))
        {
            PlanPositionIndependent(poa);
        }
        else if(IsDirectionIndependent(poa))
        {
            PlanDirectionIndependent(poa);
        }
        else
        {
            PlanDirectionDependent(poa);
        }

        //능력이 없을때는 그냥 가장 가까운 상대를 향해 이동한다
        if(poa.ability==null)
        {
            MoveTowardOpponent(poa);
        }
       
        //완료된 계획을 반환
        return poa;
    }

    void DefaultAttackPattern(PlanOfAttack poa)
    {
        //첫번째 공격 능력을 가져옴
        poa.ability = actor.GetComponentInChildren<Ability>();
        poa.targets = Targets.Foe;
    }
    //능력 사용시 위치를 확인하는 함수
    //능력의 범위안 까지 이동함
  bool IsPositionIndependent(PlanOfAttack poa)
    {
        AbilityRange range = poa.ability.GetComponent<AbilityRange>();
        return range.postitionOriented == false;
    }

    void PlanPositionIndependent(PlanOfAttack poa)
    {
        List<Tile> moveOptions = GetMoveOptions();
        Tile tile = moveOptions[Random.Range(0, moveOptions.Count)];
        poa.moveLocation = poa.fireLocation = tile.pos;
    }

    bool IsDirectionIndependent(PlanOfAttack poa)
    {
        AbilityRange range = poa.ability.GetComponent<AbilityRange>();
        return !range.directionOriented;
    }
    void PlanDirectionIndependent(PlanOfAttack poa)
    {
        Tile startTile = actor.tile;
        Dictionary<Tile, AttackOption> map = new Dictionary<Tile, AttackOption>();
        AbilityRange ar = poa.ability.GetComponent<AbilityRange>();
        List<Tile> moveOptions = GetMoveOptions();

        //해당 이동위치의 발사 범위 내의 모든 타일을 고려
        //해당 발사 위치의 모든 타일을 고려
        for(int i=0;i<moveOptions.Count;++i)
        {
            Tile moveTile = moveOptions[i];
            actor.Place(moveTile);
            List<Tile> fireOptrions = ar.GetTilesInRange(bc.board);

            for(int j=0;j<fireOptrions.Count;++j)
            {
                Tile fireTile = fireOptrions[j];
                AttackOption ao = null;
                if(map.ContainsKey(fireTile))
                {
                    ao = map[fireTile];
                }
                else
                {
                    ao = new AttackOption();
                    map[fireTile] = ao;
                    ao.target = fireTile;
                    ao.direction = actor.dir;
                    RateFireLocation(poa, ao);
                }
                ao.AddMoveTarget(moveTile);
            }
        }
        //최적의 위치를 저장해서 해당 작전에 전달
        actor.Place(startTile);
        List<AttackOption> list = new List<AttackOption>(map.Values);
        PickBestOption(poa, list);
    }

    void PlanDirectionDependent(PlanOfAttack poa)
    {
        Tile startTile = actor.tile;
        Directions startDirection = actor.dir;
        List<AttackOption> list = new List<AttackOption>();
        List<Tile> moveOptions = GetMoveOptions();

        for (int i = 0; i < moveOptions.Count;++i)
        {
            Tile moveTile = moveOptions[i];
            actor.Place(moveTile);

            for(int j=0;j<4;++j)
            {
                actor.dir = (Directions)j;
                AttackOption ao = new AttackOption();
                ao.target = moveTile;
                ao.direction = actor.dir;
                RateFireLocation(poa, ao);
                ao.AddMoveTarget(moveTile);
                list.Add(ao);
            }
        }
        actor.Place(startTile);
        actor.dir = startDirection;
        PickBestOption(poa, list);
    }
    //단순히 현재 액터가 도달할 수 있는 타일 목록을 반환
    List<Tile>GetMoveOptions()
    {
        return actor.GetComponent<Movement>().GetTilesInRange(bc.board);
    }

    //만약에 공격범위에 아군이 있다면 그것을 피하고 적군을 타겟팅 공격하는
    //최적의 방법을 찾는 함수(위치를 조정함)
    void RateFireLocation(PlanOfAttack poa,AttackOption option)
    {
        AbilityArea area = poa.ability.GetComponent<AbilityArea>();
        List<Tile> tiles = area.GetTilesInArea(bc.board, option.target.pos);
        option.areaTargets = tiles;
        option.isCasterMatch = IsAbilityTargetMatch(poa, actor.tile);

        for(int i=0;i<tiles.Count;++i)
        {
            Tile tile = tiles[i];
            if(actor.tile==tiles[i] || !poa.ability.IsTarget(tile))
            {
                continue;
            }
            bool isMatch = IsAbilityTargetMatch(poa, tile);
            option.AddMark(tile, isMatch);
        }
    }
    //일치 여부를 결정하는 방법 
    bool IsAbilityTargetMatch(PlanOfAttack poa,Tile tile)
    {
        bool isMatch = false;
        if(poa.targets==Targets.Tile)
        {
            isMatch = true;
        }
        else if(poa.targets!=Targets.None)
        {
            Alliance other = tile.content.GetComponentInChildren<Alliance>();
            if(other!=null&&Alliance.IsMatch(other,poa.targets))
            {
                isMatch = true;
            }
        }
        return isMatch;
    }

    //모든 전략에 점수를 매겨
    //가장 점수가 높은 전략을 계산해 주는 함수
    void PickBestOption(PlanOfAttack poa,List<AttackOption>list)
    {
        int bestScore = 1;
        List<AttackOption> bestOption = new List<AttackOption>();
        for(int i = 0;i < list.Count;++i)
        {
            AttackOption option = list[i];
            int score = option.GetScore(actor, poa.ability);
            if(score>bestScore)
            {
                bestScore = score;
                bestOption.Clear();
                bestOption.Add(option);
            }
            else if(score == bestScore)
            {
                bestOption.Add(option);
            }
        }

        if(bestOption.Count==0)
        {
            poa.ability = null; //수행하지 않아서 지움
            return;
        }

        List<AttackOption> finalPicks = new List<AttackOption>();
        bestScore = 0;
        for(int i=0;i<bestOption.Count;++i)
        {
            AttackOption option = bestOption[i];
            int score = option.bestAngleBasedScore;
            if(score>bestScore)
            {
                bestScore = score;
                finalPicks.Clear();
                finalPicks.Add(option);
            }
            else if(score==bestScore)
            {
                finalPicks.Add(option);
            }
        }
        AttackOption choice = finalPicks[UnityEngine.Random.Range(0, finalPicks.Count)];
        poa.fireLocation = choice.target.pos;
        poa.attackDirection = choice.direction;
        poa.moveLocation = choice.bestMoveTile.pos;
    }

    //주어진 부대에 가장 가까운 적이 어디에 있는지 알아내는 함수
    //적이고 ko되지 않은 유닛이 있는 타일을 찾은 경우 
    //이동할 가치가 있는 대상을 찾는다
    void FindNearestFoe()
    {
        nearestFoe = null;
        bc.board.Search(actor.tile, delegate (Tile arg1, Tile arg2)
         {
             if (nearestFoe == null && arg2.content != null)
             {
                 Alliance other = arg2.content.GetComponentInChildren<Alliance>();
                 if (other != null && Alliance.IsMatch(other, Targets.Foe))
                 {
                     Unit unit = other.GetComponent<Unit>();
                     Stats stats = unit.GetComponent<Stats>();
                     if (stats[StateTypes.HP] > 0)
                     {
                         nearestFoe = unit;
                         return true;
                     }
                 }
             }
             return nearestFoe == null;
         });
    }

    //가장 가까운적에게 최대한 가깝게 이동하는 함수
    void MoveTowardOpponent(PlanOfAttack poa)
    {
        List<Tile> moveOption = GetMoveOptions();
        FindNearestFoe();
        if(nearestFoe!=null)
        {
            Tile toCheck = nearestFoe.tile;
            while(toCheck!=null)
            {
                if(moveOption.Contains(toCheck))
                {
                    poa.moveLocation = toCheck.pos;
                    return;
                }
                toCheck = toCheck.prev;
            }
        }
        poa.moveLocation = actor.tile.pos;
    }

    //능력 사용 후 방향을 결정해야함
    //다시 적을 찾은 후 가까운적이랑 정면으로 마주하게 방향을 반복
    //공격후 적군과 마주하는 방향을 정하는 함수
    public Directions DetermineEndFacingDirection()
    {
        Directions dir = (Directions)UnityEngine.Random.Range(0, 4);
        FindNearestFoe();
        if(nearestFoe!=null)
        {
            Directions start = actor.dir;
            for(int i=0;i<4;++i)
            {
                actor.dir = (Directions)i;
                if(nearestFoe.GetFacing(actor)==Facing.front)
                {
                    dir = actor.dir;
                    break;
                }
            }
            actor.dir = start;
        }
        return dir;
    }
}
