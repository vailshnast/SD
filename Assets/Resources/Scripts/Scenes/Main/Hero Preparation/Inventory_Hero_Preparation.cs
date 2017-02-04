using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory_Hero_Preparation : MonoBehaviour {


    private Database_Items database_Items;

    private SaveSystem Save_System;

    private GameObject slotPanel;
    
    private GameObject slot;
    private GameObject item;

    
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

    private int amountofslots;

    [HideInInspector]
    public Shop_Tooltip shop_tooltip;

    //If we dont check this in OnEnable, will cause different bugs,
    //because, OnEnable is called before Start().
    private bool initialised = false;

    public void reduce_gold(int item_price)
    {
        Save_System.set_chosed_hero_gold_quantity(Save_System.get_chosed_hero_gold_quantity() - item_price);
    }
    public bool check_gold_enough(int gold)
    {
        if (Save_System.get_chosed_hero_gold_quantity() >= gold)
            return true;
        else
            return false;
    }

    void Start()
    {
        Save_System = FindObjectOfType<SaveSystem>();
        database_Items = GetComponent<Database_Items>();
        
        
        GameObject.Find("Text_gold_quantity").GetComponent<Text>().text = Save_System.get_chosed_hero_gold_quantity().ToString();

        shop_tooltip = transform.parent.parent.gameObject.GetComponent<Shop_Tooltip>();
        slotPanel = transform.FindChild("Panel_Items").FindChild("Slots_Panel").gameObject;

        slot = Resources.Load("Prefabs/Slot", typeof(GameObject)) as GameObject;
        item = Resources.Load("Prefabs/Item", typeof(GameObject)) as GameObject;

        amountofslots = 10;

        InitInventory();

        initialised = true;
    }
    void OnEnable()
    {
        if (initialised)
        {
            DeleteItems(0);
            for (int i = 0; i < Save_System.get_sd_char().List_Items.Length; i++)
            {
                if (Save_System.get_sd_char().List_Items[i] != 0)
                    AddItem(Save_System.get_sd_char().List_Items[i]);
            }
            GameObject.Find("Text_gold_quantity").GetComponent<Text>().text = Save_System.get_chosed_hero_gold_quantity().ToString();
        }
    }
    void InitInventory()
    {
        for (int i = 0; i < amountofslots; i++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(slot));
            slots[i].name = "Slot#"+(i+1);
            slots[i].transform.GetComponent<Data_Slot>().id = i;
            slots[i].transform.SetParent(slotPanel.transform, false);
        }
        for (int i = 0; i < Save_System.get_sd_char().List_Items.Length; i++)
        {
            if (Save_System.get_sd_char().List_Items[i] != 0)
                AddItem(Save_System.get_sd_char().List_Items[i]);
        }

    }
    private void DeleteItems(int index)
    {
        for (int i = index; i < slots.Count; i++)
        {
            if (slots[i].transform.childCount > 0)
                Destroy(slots[i].transform.GetChild(0).gameObject);
            items[i] = new Item();
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
                    new_item.GetComponent<Data_Item>().item = candidateItem;
                    new_item.GetComponent<Data_Item>().slot = i;
                    new_item.GetComponent<Image>().sprite = candidateItem.GetSprite();
                    new_item.name = candidateItem.Title;
                    new_item.transform.SetParent(slots[i].transform, false);
                    break;
                }
            }
    }


    public int[] parse_inventory()
    {
        int[] inventory = new int[10];
        for (int a = 0; a < 10; a++)
        {
            //inventory[a] = inventory_item_list[a].ID;
            if (slots[a].transform.childCount > 0)
            {
                inventory[a] = slots[a].transform.GetChild(0).GetComponent<Data_Item>().item.ID;
            }

        }

        return inventory;
    }

    public void parse_information()
    {
        Save_System.set_chosed_hero_inv(parse_inventory());
    }
}
