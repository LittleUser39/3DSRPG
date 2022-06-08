using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipUI : MonoBehaviour
{
    EquipUnit[] items;
    private void Awake()
    {
        items = GetComponentsInChildren<EquipUnit>();
    }

    public void GetItem(string name)
    {
        GameObject hero = GameObject.Find(name);

        Equipment equipItmes = hero.GetComponent<Equipment>();

        for (int i = 0; i < items.Length; ++i)
        {
            items[i].AddItem(null);
            
            if (equipItmes._items.Count >= i)
            {
                Equippable equippable = equipItmes._items[i];
                items[i].AddItem(equippable);
            }
           
        }
    }
    
}
