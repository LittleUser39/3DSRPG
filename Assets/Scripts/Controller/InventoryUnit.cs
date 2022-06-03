using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUnit : MonoBehaviour
{
    public Button button;
    public Image icon;
    string heroName;
    public GameObject equippable;
    //Equippable curItemData;

   
    public void AddItem(GameObject itemData)
    {
        equippable = itemData;
        //curItemData = equippable.GetComponent<Equippable>();
        //icon.sprite = equippable.GetComponent<Image>().sprite;
        //icon.enabled = true;
        button.interactable = true;
    }

    public void RemoveItem()
    {
        equippable = null;
        //curItemData = null;
        //icon.sprite = null;
        //icon.enabled = false;
        button.interactable = false;
    }

    public void UseItem()
    {
        Debug.Log(equippable.name + "가 사용되었습니다.");
        EquipItem(equippable);
        //InventoryManager.instance.Remove(equippable);
    }
    public void SetHeroName(string name)
    {
        heroName = name;
    }
    void EquipItem(GameObject item)
    {
        Debug.Log("아이템 장착하기");
        Equippable toEquip = item.GetComponent<Equippable>();
        Equipment equipment =GameObject.Find(heroName).GetComponent<Equipment>();

        //아이템 장착
        equipment.Equip(toEquip, toEquip.defaultSlots);
        
    }
}
