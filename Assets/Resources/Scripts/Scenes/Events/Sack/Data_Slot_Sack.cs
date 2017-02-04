using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class Data_Slot_Sack : MonoBehaviour, IDropHandler, IPointerClickHandler
{

    private Inventory_Sack inventory;
    public int item_id;

    private Button_Proccesor_Sack button_Proccesor;

    void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory_Sack>();
        button_Proccesor = FindObjectOfType<Button_Proccesor_Sack>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Data_Item_Sack buffered_item = eventData.pointerDrag.GetComponent<Data_Item_Sack>();
        if (inventory.inventory_item_list[item_id].ID == 0)
        {
            inventory.inventory_item_list[buffered_item.slot] = new Item();
            inventory.inventory_item_list[item_id] = buffered_item.item;
            buffered_item.slot = item_id;
        }
        else
        {
            if (transform.childCount > 0)
            {
                Transform duplicated_item = transform.GetChild(0);

                duplicated_item.GetComponent<Data_Item_Sack>().slot = buffered_item.slot;
                duplicated_item.transform.SetParent(inventory.inventory_slot_list[buffered_item.slot].transform, false);
                duplicated_item.position = inventory.inventory_slot_list[buffered_item.slot].transform.position;
                buffered_item.slot = item_id;
                buffered_item.transform.SetParent(transform, false);
                buffered_item.transform.position = transform.position;

                inventory.inventory_item_list[buffered_item.slot] = duplicated_item.GetComponent<Data_Item_Sack>().item;

                inventory.inventory_item_list[item_id] = buffered_item.item;
            }
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            for (int i = 0; i < inventory.inventory_slot_list.Count; i++)
            {
                if (inventory.inventory_slot_list[i] != transform.parent)
                {
                    inventory.inventory_slot_list[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/HeroPreparation/BasicSlot");
                }
            }
            button_Proccesor.ScrollIsUsed = false;
            FindObjectOfType<Sack_Tooltip>().Deactivate(false);
        }

    }

}
