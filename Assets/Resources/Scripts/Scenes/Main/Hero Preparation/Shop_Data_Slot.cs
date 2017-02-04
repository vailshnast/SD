using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Shop_Data_Slot : MonoBehaviour, IPointerClickHandler
{
    public int id;

    void Start()
    {

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
