using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class Data_Slot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    private Inventory_Hero_Preparation inventory;
    public int id;

    void Start()
    {
        inventory = gameObject.transform.parent.parent.parent.gameObject.GetComponent<Inventory_Hero_Preparation>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        Data_Item buffered_item = eventData.pointerDrag.GetComponent<Data_Item>();     
        if (inventory.items[id].ID == 0)
        {
            inventory.items[buffered_item.slot] = new Item();
            inventory.items[id] = buffered_item.item;
            buffered_item.slot = id;
        }
        else
        {
            if (transform.childCount > 0)
            {
                Transform duplicated_item = this.transform.GetChild(0);
                duplicated_item.GetComponent<Data_Item>().slot = buffered_item.slot;
                duplicated_item.transform.SetParent(inventory.slots[buffered_item.slot].transform, false);
                duplicated_item.position = inventory.slots[buffered_item.slot].transform.position;

                buffered_item.slot = id;
                buffered_item.transform.SetParent(this.transform, false);
                buffered_item.transform.position = this.transform.position;

                inventory.items[buffered_item.slot] = duplicated_item.GetComponent<Data_Item>().item;
                inventory.items[id] = buffered_item.item;
            }
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            FindObjectOfType<Tooltip>().Deactivate();
            FindObjectOfType<Shop_Tooltip>().Deactivate();
        }

    }
}
