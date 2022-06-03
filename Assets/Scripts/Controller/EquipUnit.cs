using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipUnit : MonoBehaviour
{

    public Button button;
    public Image icon;
    public Equippable equippable;
  

  
    public void AddItem(Equippable itemData)
    {
        if (itemData == null)
        {
            icon.sprite = null;
            return;
        }
            

        equippable = itemData;
        //equip = itemData.gameObject;
        //curItemData = equippable.GetComponent<Equippable>();
        icon.sprite = equippable.gameObject.GetComponent<Image>().sprite;
        button.interactable = true;
    }

    public void RemoveItem()
    {
        equippable = null;
        button.interactable = false;
    }

    public void UseItem()
    {
        Debug.Log(equippable.name + "가 사용되었습니다.");
       // EquipItem(equippable);
    }
}
