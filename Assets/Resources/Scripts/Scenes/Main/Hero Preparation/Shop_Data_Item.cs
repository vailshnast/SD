using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop_Data_Item : MonoBehaviour, IPointerClickHandler
{

    public Item item;
    public int amount;
    public int slot;

    private Inventory_Hero_Preparation inventory;
    private Shop shop;
    private Vector2 offset;
    public Shop_Tooltip tooltip;

    void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory_Hero_Preparation>();
        shop = FindObjectOfType<Shop>();
        tooltip = inventory.shop_tooltip;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
        for (int i = 0; i < inventory.slots.Count; i++)
        {
            if (inventory.slots[i] != transform.parent)
            {
                inventory.slots[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/HeroPreparation/BasicSlot");
            }
        }
        for (int i = 0; i < shop.slots.Count; i++)
        {
            if (shop.slots[i] != transform.parent)
            {
                shop.slots[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/HeroPreparation/BasicSlot");
            }
        }
        transform.parent.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/HeroPreparation/YellowSlot");
        tooltip.Activate(this);
        
    }
}
    