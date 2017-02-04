using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class Data_Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler {
    public Item item;
    public int amount;
    public int slot;

    private Inventory_Hero_Preparation inventory;
    private Shop shop;
    private Vector2 offset;
    public Tooltip tooltip;
    private GameObject panel_tooltip;

    void Start()
    {
        inventory = transform.parent.parent.parent.parent.GetComponent<Inventory_Hero_Preparation>();
        shop = FindObjectOfType<Shop>();
        tooltip = inventory.transform.parent.GetComponent<Tooltip>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        FindObjectOfType<Shop_Tooltip>().Deactivate();
        FindObjectOfType<Tooltip>().Deactivate();
        if (item != null)
        {
            offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
            this.transform.SetParent(transform.parent.parent.parent, false);
            this.transform.position = eventData.position - offset;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        for (int i = 0; i < inventory.slots.Count; i++)
        { 
            inventory.slots[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/HeroPreparation/BasicSlot");
        }
        if (item != null)
        {
            this.transform.position = eventData.position - offset;
            
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(inventory.slots[slot].transform, false);
        transform.position = inventory.slots[slot].transform.position;
        GetComponent<RectTransform>().sizeDelta = new Vector2(94, 94);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        for (int i = 0; i < inventory.slots.Count; i++)
        {
            if(inventory.slots[i] != transform.parent)
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
