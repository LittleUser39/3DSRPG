using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager _instance;
    public static InventoryManager instance
    {
        get
        {
            return _instance;
        }
    }

    public int invenMaxSize = 5;
    public InventoryUI invenUi;
    public EquipUI equipUI;
    public List<GameObject> items = new List<GameObject>();
    public List<Equippable> equipItems = new List<Equippable>();
    private void Awake()
    {
        _instance = this;
    }
  
    public void Add(GameObject item)
    {
        if (items.Count >= invenMaxSize)
            return ;

        items.Add(item);
        invenUi.UpdateUI();
    }

    public void Remove(GameObject item)
    {
        items.Remove(item);
        invenUi.UpdateUI();
    }
}
