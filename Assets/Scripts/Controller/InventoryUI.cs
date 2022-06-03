using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    InventoryUnit[] items;

    private void Awake()
    {
       CreateItems();
    }
    public void UpdateUI()
    {
        items = GetComponentsInChildren<InventoryUnit>();
        for (int i = 0; i < items.Length; i++)
        {
            if (i < InventoryManager.instance.items.Count)
            {
               items[i].AddItem(InventoryManager.instance.items[i]);
            }
            else
            {
                items[i].RemoveItem();
            }
        }
    }
    void CreateItems()
    {
        // 무기1
        //InventoryManager.instance.Add(InventoryManager.instance.sword);
        GameObject sword = Resources.Load<GameObject>("Item/Sword");
        GameObject body = Resources.Load<GameObject>("Item/Body");
        GameObject shose = Resources.Load<GameObject>("Item/Shose");
        Button[] buttonTrans = transform.GetComponentsInChildren<Button>();

        for (int i = 0; i < 3; ++i)
        {
            GameObject sword1 = Instantiate(sword);
            sword1.transform.SetParent(buttonTrans[i].gameObject.transform);
            sword1.transform.position = buttonTrans[i].gameObject.transform.position;
            InventoryManager.instance.Add(sword1);
        }
        for (int i = 3; i < 6; ++i)
        {
            GameObject sword1 = Instantiate(body);
            sword1.transform.SetParent(buttonTrans[i].gameObject.transform);
            sword1.transform.position = buttonTrans[i].gameObject.transform.position;
            InventoryManager.instance.Add(sword1);
        }
        for (int i = 6; i < 9; ++i)
        {
            GameObject sword1 = Instantiate(shose);
            sword1.transform.SetParent(buttonTrans[i].gameObject.transform);
            sword1.transform.position = buttonTrans[i].gameObject.transform.position;
            InventoryManager.instance.Add(sword1);
        }
    }    
}

