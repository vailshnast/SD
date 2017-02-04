using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Button_Proccesor_Sack : MonoBehaviour
{

    private Inventory_Sack inventory;
    private Sack_Tooltip tooltip;

    private bool scrollIsUsed;


    public Node node;


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
        inventory = gameObject.transform.FindChild("Inventory").GetComponent<Inventory_Sack>();
        tooltip = gameObject.GetComponent<Sack_Tooltip>();
    }
    
    public void Exit()
    {
        node.SetNodeEvent(NodeEvent.Empty_Path);
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
                        if (inventory.inventory_slot_list[i].transform.GetChild(0).GetComponent<Data_Item_Sack>().item.Group == "Rune" && inventory.inventory_slot_list[i].transform.GetChild(0).GetComponent<Data_Item_Sack>().item.ID == 4)
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
}
