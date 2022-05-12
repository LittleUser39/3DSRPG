using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//소모성 아이템의 효과를 대상에게 적용시키는 코드
public class Consumable : MonoBehaviour
{
    public void Consume(GameObject target)
    {
        Feature[] features = GetComponentsInChildren<Feature>();
        for(int i=0;i<features.Length;++i)
        {
            features[i].Apply(target);
        }
    }
}
