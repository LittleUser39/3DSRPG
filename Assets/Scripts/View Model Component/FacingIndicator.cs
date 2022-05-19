using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//유닛이 향하는 방향으로 강조하는 클래스
//유닛이 바라보는 방향을 화살표등으로 알려주는 클래스
public class FacingIndicator : MonoBehaviour
{
    [SerializeField] Renderer[] direction;
    [SerializeField] Material normal;
    [SerializeField] Material selected;

    public void SetDirection(Directions dir)
    {
        //방향을 설정
        int index = (int)dir;
        for(int i = 0; i < 4; ++i)
        {
            direction[i].material = (i == index) ? selected : normal;
        }
    }
}
