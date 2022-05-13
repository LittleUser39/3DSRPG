using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityRange : MonoBehaviour
{
    //사용자가 접근할수 있는 영역(x,z)축
    public int horizontal = 1;

    //사용자가 접근할 수 있는 타일 간의 높이 차이(y)축
    public int vertical = int.MaxValue;

    //방향 전환이 가능한지 여부
    //디폴트는 false 불가
    public virtual bool directionOriented { get { return false; } }

    //공격하는 댜상의 정보를 관리
    protected Unit unit { get { return GetComponentInParent<Unit>(); } }

    //도달할 수 있는 타일 리스트
    //abstract 이므로 AbilityRange를 상속받는 모든 클래스에서
    //구현되어야 하는 변수
    //이게 어빌리티(스킬의 범위 만 정하는거임)
    //todo 나중에 스킬 범위 만들때 이거 참고
    public abstract List<Tile> GetTilesInRange(Board board);
}
