using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Button_Proccesor_Reward : MonoBehaviour
{

    private Inventory_Reward inventory;
    private Reward_Tooltip tooltip;

    private bool scrollIsUsed;



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
    void Start()
    {
        inventory = gameObject.transform.FindChild("Inventory").GetComponent<Inventory_Reward>();
        tooltip = gameObject.GetComponent<Reward_Tooltip>();
    }

    public void ButtonPressed()
    {
        if (tooltip.getButtonText().text == "Destroy")
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
                        if (inventory.inventory_slot_list[i].transform.GetChild(0).GetComponent<Data_Item_Reward>().item.Group == "Rune" && inventory.inventory_slot_list[i].transform.GetChild(0).GetComponent<Data_Item_Reward>().item.ID == 4)
                        {
                            inventory.inventory_slot_list[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/HeroPreparation/YellowSlot");
                        }
                    }
                }
                tooltip.Deactivate(true);
                break;

            case "Rune":

                

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
