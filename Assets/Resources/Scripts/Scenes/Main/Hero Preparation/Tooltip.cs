using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour {

    private Data_Item item;
    private string data;
    private string title;

    private GameObject tooltip_Example_Inventory;

    private SaveSystem Save_System;

    private Inventory_Hero_Preparation inventory;
    //private Data_Item item_link;

    void Start()
    {
        Save_System = FindObjectOfType<SaveSystem>();
        tooltip_Example_Inventory = GameObject.Find("Tooltip_Example_Inventory");
        tooltip_Example_Inventory.SetActive(false);

        inventory = gameObject.transform.Find("Inventory").GetComponent<Inventory_Hero_Preparation>();


    }

    public void Activate(Data_Item data_item)
    {
        FindObjectOfType<Shop_Tooltip>().Deactivate();
        item = data_item;

        data = data_item.item.Description;
        title = data_item.item.Title;



        getChild(tooltip_Example_Inventory, "Tooltip_Name").GetComponent<Text>().text = title;
        getChild(tooltip_Example_Inventory, "Tooltip_Description").GetComponent<Text>().text = data;
        getChild(tooltip_Example_Inventory, "Tooltip_Price").GetComponent<Text>().text = data_item.item.Price.ToString();
        getChild(tooltip_Example_Inventory, "Tooltip_Sprite").GetComponent<Image>().sprite = item.GetComponent<Image>().sprite;


        tooltip_Example_Inventory.SetActive(true);


    }
    private GameObject getChild(GameObject objectToLook, string name)
    {
        Transform[] allChildren = objectToLook.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.name == name)
                return child.gameObject;
        }
        return null;
    }

    public void Deactivate()
    {
        if (item)
            item.transform.parent.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/HeroPreparation/BasicSlot");
        tooltip_Example_Inventory.SetActive(false);
    }

    public void Sell()
    {
        Destroy(inventory.slots[item.slot].transform.GetChild(0).gameObject);
        inventory.items[item.slot] = new Item();
        inventory.reduce_gold(-item.item.Price);
        GameObject.Find("Text_gold_quantity").GetComponent<Text>().text = Save_System.get_chosed_hero_gold_quantity().ToString();
        Deactivate();
    }


}
