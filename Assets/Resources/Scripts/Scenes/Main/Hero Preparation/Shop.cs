using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class Shop : MonoBehaviour{
    GameObject slotPanel;
    Database_Items database_Items;
    GameObject slot;
    GameObject item;

    GameObject inventory;

    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

    

    void Start()
    {
        inventory = GameObject.Find("Inventory");
        database_Items = inventory.GetComponent<Database_Items>();
        slot = Resources.Load("Prefabs/Shop_Slot", typeof(GameObject)) as GameObject;
        item = Resources.Load("Prefabs/Shop_Item", typeof(GameObject)) as GameObject;
        slotPanel = GameObject.Find("Shop_Slots_Panel");
        initShop();
        AddItem(1);
        AddItem(2);
        AddItem(3);
        AddItem(4);
        AddItem(5);
        AddItem(12);
    }

    void initShop()
    {
        for (int i = 0; i < 10; i++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(slot));
            slots[i].name = "Slot#" + (i + 1);
            slots[i].transform.GetComponent<Shop_Data_Slot>().id = i;
            slots[i].transform.SetParent(slotPanel.transform, false);
        }
    }

    public void AddItem(int id)
    {
        Item candidateItem = database_Items.getItem(id);


        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ID == 0)
            {
                items[i] = candidateItem;
                GameObject new_item = Instantiate(item);
                new_item.GetComponent<Shop_Data_Item>().item = candidateItem;
                new_item.GetComponent<Shop_Data_Item>().slot = i;
                new_item.GetComponent<Image>().sprite = candidateItem.GetSprite();
                new_item.name = candidateItem.Title;
                new_item.transform.SetParent(slots[i].transform, false);
                break;
            }
        }

    }

    public void sell(Item item)
    {
        inventory.GetComponent<Inventory_Hero_Preparation>().reduce_gold(item.Price);
        if (inventory.GetComponent<Inventory_Hero_Preparation>().check_gold_enough(item.Price))
            inventory.GetComponent<Inventory_Hero_Preparation>().AddItem(item.ID);
    }
}
