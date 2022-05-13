using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//아이템을 장착하거나 장착해제하는 기능들을 하는 클래스
public class Equipment : MonoBehaviour
{
    public const string EquippedNotification = "Equipment.EquippedNotification";
    public const string UnEquippedNotification = "Equipment.UnEquippedNotification";

    public IList<Equippable> Items { get { return _items.AsReadOnly(); } }

    //장착중인 아이템 리스트
    List<Equippable> _items = new List<Equippable>();

    public void Equip(Equippable item,EquipSlots slots)
    {
        //해당 슬롯에 다른 아이템이 있으면 벗긴다
        UnEquip(slots);

        //장착리스트에 추가
        _items.Add(item);

        //해당 아이템이 장착하는 유닛의 자식오브젝트가 됨
        item.transform.SetParent(transform);
        item.slots = slots;

        //아이템 능력치 만큼 캐릭터 능력치 증가
        item.OnEquip();

        // Equipment.EquippedNotification 키로 저장된 델리게이트 호출
        this.PostNotification(EquippedNotification, item);
    }

    //아이템 장착해제
    public void UnEquip(Equippable item)
    {
        //아이템 능력치 만큼 캐릭터 능력치 감소
        item.OnUnEquip();

        //해당 슬롯을 빈 슬록으로 만들기
        item.slots = EquipSlots.None;
        item.transform.SetParent(transform);

        //장착중인 아이템 중에서 제외
        _items.Remove(item);

        //저장된 델리게이트 호출
        this.PostNotification(UnEquippedNotification, item);
    }

    //특정슬롯의 아이템 장착해제
    public void UnEquip(EquipSlots slots)
    {
        for (int i = _items.Count - 1; i >= 0; --i)
        {
            Equippable item = _items[i];

            if ((item.slots & slots) != EquipSlots.None)
            {
                UnEquip(item);
            }
        }
    }
}
