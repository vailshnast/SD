using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class Rune_Item : MonoBehaviour, IPointerClickHandler
{
    private Explore_Tooltip tooltip;

	// Use this for initialization
	void Start () {
        tooltip = FindObjectOfType<Explore_Tooltip>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnPointerClick(PointerEventData eventData)
    {
        tooltip.SetRuneSlot(transform.parent.GetComponent<Rune_Slot>());
        tooltip.Activate(transform.parent.GetComponent<Rune_Slot>().GetRuneItem());
    }
}
