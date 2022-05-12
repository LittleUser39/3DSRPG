using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//장착과 해제에 대해 능력치 변화하는 클래스
public class Equippable : MonoBehaviour
{
    //장비 슬롯에 관한 변수
    public EquipSlots defaultSlots;
    public EquipSlots secondaySlots;
    public EquipSlots slots;
    //착용 되었나 변수
    bool _isEquipped;

    //아이템 장착
    public void OnEquip()
    {
        if (_isEquipped) return;
        _isEquipped = true;

        Feature[] features = GetComponentsInChildren<Feature>();
        for(int i=0; i<features.Length;++i)
        {
            //아이템의 능력치 만큼 증가시킨다
            features[i].Activate(gameObject);
        }
    }

    public void OnUnEquip()
    {
        if (!_isEquipped) return;
        _isEquipped = false;
        Feature[] features = GetComponentsInChildren<Feature>();
        for(int i=0; i<features.Length;++i)
        {
            //장착해제한 아이템의 능력치 만큼 감소
            features[i].Deactivate();
        }
    }    
}
