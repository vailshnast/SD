using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Button_Proccesor : MonoBehaviour {

    private Inventory_Explore inventory;
    private Explore_Tooltip tooltip;

    private bool scrollIsUsed;

    private Rune_System rune_System;

    public bool ScrollIsUsed
    {
        get
        {
            return scrollIsUsed;
        }
        set
        {
            scrollIsUsed = value;
        }
    }
    // Use this for initialization
    void Start () {
        inventory = gameObject.transform.FindChild("Inventory").GetComponent<Inventory_Explore>();
        rune_System = FindObjectOfType<Rune_System>();
        tooltip = gameObject.GetComponent<Explore_Tooltip>();
        
    }



    public void ButtonPressed()
    {
        if(tooltip.getButtonText().text == "Destroy")
        {
            inventory.DestroyRune(tooltip.GetRuneSlot().GetRuneItem());
            tooltip.GetRuneSlot().DestroyRune();
            tooltip.Deactivate(false);
        }
        else
        {
            ButtonUse();
        }
       
    }

    private void ButtonUse()
    {
        switch (tooltip.getItem().item.Group)
        {
            case "Scroll":
                scrollIsUsed = true;
                for (int i = 0; i < inventory.inventory_slot_list.Count; i++)
                {
                    if (inventory.inventory_slot_list[i].transform.childCount > 0)
                    {
                        //If checking slot contains unidentified rune
                        if (inventory.inventory_slot_list[i].transform.GetChild(0).GetComponent<Data_Item_Explore>().item.Group == "Rune" && inventory.inventory_slot_list[i].transform.GetChild(0).GetComponent<Data_Item_Explore>().item.ID == 4)
                        {
                            inventory.inventory_slot_list[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/HeroPreparation/YellowSlot");
                        }
                    }
                }
                tooltip.Deactivate(true);
                break;

            case "Rune":

                if (tooltip.getItem().item.ID != 4)
                {
                    rune_System.AddRune(tooltip.getItem());
                    inventory.InsertRune(tooltip.getItem().item);
                }
                else
                {
                    if(!rune_System.Slots_Are_Full())
                    {
                        rune_System.AddRandomRune(tooltip.getItem());
                        tooltip.Activate(tooltip.getItem().item);
                        inventory.InsertRune(tooltip.getItem().item);
                    }
                    else
                        FindObjectOfType<Explore_Tooltip>().SetDescription("Full slots");

                }

                break;

            case "Potion":
                inventory.DrinkPotion(tooltip.getItem().item);
                tooltip.DestroyItem(tooltip.getItem());
                break;

            case "Torch":
                inventory.UseOil(5);
                tooltip.DestroyItem(tooltip.getItem());
                break;             
        }
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
}
