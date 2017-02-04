using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Shop_Tooltip : MonoBehaviour {

    private Shop_Data_Item item;

    private string data;
    private string title;

    private GameObject tooltipExample;

    private SaveSystem Save_System;

    private Inventory_Hero_Preparation inventory;


    void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory_Hero_Preparation>();
        Save_System = FindObjectOfType<SaveSystem>();       

        tooltipExample = GameObject.Find("Tooltip_Example");
        tooltipExample.SetActive(false);
    }

    public void Activate(Shop_Data_Item data_item)
    {
        FindObjectOfType<Tooltip>().Deactivate();
        item = data_item;
        data = data_item.item.Description;
        title = data_item.item.Title;


        getChild(tooltipExample, "Tooltip_Name").GetComponent<Text>().text = title;
        getChild(tooltipExample, "Tooltip_Description").GetComponent<Text>().text = data;
        getChild(tooltipExample, "Tooltip_Price").GetComponent<Text>().text = data_item.item.Price.ToString();
        getChild(tooltipExample, "Tooltip_Sprite").GetComponent<Image>().sprite = item.GetComponent<Image>().sprite;


        tooltipExample.SetActive(true);

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
        if(item)
        item.transform.parent.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/HeroPreparation/BasicSlot");
        tooltipExample.SetActive(false);
    }

    public void Buy()
    {
        if (inventory.check_gold_enough(item.item.Price))
        {
            inventory.reduce_gold(item.item.Price);
            GameObject.Find("Text_gold_quantity").GetComponent<Text>().text = Save_System.get_chosed_hero_gold_quantity().ToString();
            inventory.AddItem(item.item.ID);
            //Deactivate();
        }
        else
        {
            getChild(tooltipExample, "Tooltip_Description").GetComponent<Text>().text = "Not enough gold";
        }
    }

}
