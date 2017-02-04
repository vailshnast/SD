using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Data_Item_Reward : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{

    public Item item;
    public int amount;
    public int slot;

    private Inventory_Reward inventory;
    private Vector2 offset;
    public Reward_Tooltip tooltip;


    private Button_Proccesor_Reward button_Proccesor;

    void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory_Reward>();
        button_Proccesor = FindObjectOfType<Button_Proccesor_Reward>();
        tooltip = gameObject.transform.parent.parent.parent.parent.parent.GetComponent<Reward_Tooltip>();

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        FindObjectOfType<Reward_Tooltip>().Deactivate(false);
        if (item != null)
        {
            offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
            this.transform.SetParent(transform.parent.parent.parent.parent, false);
            this.transform.position = eventData.position - offset;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

    }

    public void OnDrag(PointerEventData eventData)
    {

        for (int i = 0; i < inventory.inventory_slot_list.Count; i++)
        {
            inventory.inventory_slot_list[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/HeroPreparation/BasicSlot");
        }
        if (item != null)
        {
            this.transform.position = eventData.position - offset;

        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        gameObject.transform.SetParent(inventory.inventory_slot_list[slot].transform, false);
        transform.position = inventory.inventory_slot_list[slot].transform.position;
        GetComponent<RectTransform>().sizeDelta = new Vector2(94, 94);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (item.Group == "Rune" && button_Proccesor.ScrollIsUsed  && item.ID == 4)
        {
            Item randomItem = inventory.getDatabaseItems().getItem(Random.Range(6, 11));
            item = randomItem;
            GetComponent<Image>().sprite = randomItem.GetSprite();
            button_Proccesor.ScrollIsUsed = false;
            tooltip.DestroyItem(tooltip.getItem());
        }
        else
        {
            button_Proccesor.ScrollIsUsed = false;
        }
        for (int i = 0; i < inventory.inventory_slot_list.Count; i++)
        {
            if (inventory.inventory_slot_list[i] != transform.parent)
            {
                inventory.inventory_slot_list[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/HeroPreparation/BasicSlot");
            }
        }

        transform.parent.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/HeroPreparation/YellowSlot");
        tooltip.Activate(this);

    }
}
