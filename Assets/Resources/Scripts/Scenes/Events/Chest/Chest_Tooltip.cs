using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Chest_Tooltip : MonoBehaviour
{
    #region Variables
    [HideInInspector]
    public Inventory_Chest inventory;
    [HideInInspector]
    public GameObject tooltip;
    private GameObject panel_tooltip;

    private Data_Item_Chest item;
    private GameObject name_item;
    private Vector2 positon_name;
    private GameObject description_item;

    private Rune_Slot currentRuneSlot;

    private GameObject torchPanel;
    private GameObject hp_mana_Panel;

    private Text buttonText;

    private Vector2 position_description;
    #endregion
    void Start()
    {
        Init();
    }

    private void Init()
    {
        tooltip = GameObject.Find("Tooltip_Chest");

        name_item = GameObject.Find("Tooltip_Name");
        description_item = GameObject.Find("Tooltip_Description");

        torchPanel = GameObject.Find("Torch_Panel");
        hp_mana_Panel = GameObject.Find("HP_Panel");
        buttonText = GameObject.Find("Button").transform.GetChild(0).GetComponent<Text>();

        tooltip.SetActive(false);
    }

    private void Information_Set(Data_Item_Chest data_item)
    {
        description_item.GetComponent<Text>().text = data_item.item.Description;
        name_item.GetComponent<Text>().text = data_item.item.Title;

        transform.getChild(tooltip, "Tooltip_Sprite").GetComponent<Image>().sprite = data_item.item.GetSprite();
        Panel_Processor(data_item);
    }
    private void Panel_Processor(Data_Item_Chest data_item)
    {
        switch (data_item.item.Group)
        {
            case "Scroll":
                buttonText.transform.parent.gameObject.SetActive(true);
                buttonText.text = "Use";
                torchPanel.SetActive(false);
                hp_mana_Panel.SetActive(false);

                break;

            case "Rune":
                buttonText.transform.parent.gameObject.SetActive(false);
                buttonText.text = "Insert";
                torchPanel.SetActive(false);
                hp_mana_Panel.SetActive(false);

                break;
            case "Potion":
                buttonText.transform.parent.gameObject.SetActive(true);
                buttonText.text = "Use";
                torchPanel.SetActive(false);
                hp_mana_Panel.SetActive(true);
                switch (data_item.item.Title)
                {
                    case "Health Potion":
                        inventory.Info_Load_Hp_Panel();
                        Debug.Log(hp_mana_Panel.transform.getChild("HP_MANA_Mask").GetComponent<Image>());
                        hp_mana_Panel.transform.getChild("HP_MANA_State_Text").GetComponent<Text>().text = "Current Health";
                        hp_mana_Panel.transform.getChild("HP_MANA_Mask").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/Inventory/HPfiller");
                        break;

                    case "Mana Potion":
                        inventory.Info_Load_Mana_Panel();
                        hp_mana_Panel.transform.getChild("HP_MANA_State_Text").GetComponent<Text>().text = "Current Mana";
                        hp_mana_Panel.transform.getChild("HP_MANA_Mask").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/Inventory/MPfiller");
                        break;
                    default:
                        break;
                }
                break;

            case "Torch":
                buttonText.transform.parent.gameObject.SetActive(true);
                buttonText.text = "Use";
                torchPanel.SetActive(true);
                hp_mana_Panel.SetActive(false);
                inventory.Info_Load_Torch_Panel();
                break;

            default:
                buttonText.transform.parent.gameObject.SetActive(false);
                torchPanel.SetActive(false);
                hp_mana_Panel.SetActive(false);
                break;
        }
    }


    public void Activate(Data_Item_Chest data_item)
    {
        //Link for a deactivation
        item = data_item;

        tooltip.SetActive(true);
        Information_Set(data_item);
    }
    public void Activate(Item item)
    {
        tooltip.SetActive(true);

        description_item.GetComponent<Text>().text = item.Description;
        name_item.GetComponent<Text>().text = item.Title;

        transform.getChild(tooltip, "Tooltip_Sprite").GetComponent<Image>().sprite = item.GetSprite();

        buttonText.text = "Destroy";
        torchPanel.SetActive(false);
        hp_mana_Panel.SetActive(false);
    }

    public void SetDescription(string description)
    {
        description_item.GetComponent<Text>().text = description;
    }

    public Data_Item_Chest getItem()
    {
        return item;
    }
    public void Deactivate(bool panelActive)
    {
        if (item)
            item.transform.parent.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/HeroPreparation/BasicSlot");

        tooltip.SetActive(panelActive);
    }
    public void DestroyItem(Data_Item_Chest data_item)
    {
        Destroy(inventory.inventory_slot_list[data_item.slot].transform.GetChild(0).gameObject);
        inventory.inventory_item_list[data_item.slot] = new Item();
        Deactivate(false);
    }
    public void DeactivateAndDisableScroll(bool panelActive)
    {
        FindObjectOfType<Button_Proccesor_Chest>().ScrollIsUsed = false;
        for (int i = 0; i < inventory.inventory_slot_list.Count; i++)
        {
            inventory.inventory_slot_list[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/HeroPreparation/BasicSlot");
        }

        tooltip.SetActive(false);
    }
    public Text getButtonText()
    {
        return buttonText;
    }

    public void SetRuneSlot(Rune_Slot slot)
    {
        currentRuneSlot = slot;
    }
    public Rune_Slot GetRuneSlot()
    {
        return currentRuneSlot;
    }
}
