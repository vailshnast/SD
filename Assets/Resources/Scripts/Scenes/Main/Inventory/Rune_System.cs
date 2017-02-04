using UnityEngine;
using System.Collections.Generic;

public class Rune_System : MonoBehaviour {

    private Rune_Slot[] allRuneSlots;
    private List<Rune_Slot> runeSlots = new List<Rune_Slot>();
    private Explore_Tooltip tooltip;

    private SaveSystem save_System;

    //If we dont check this in OnEnable, will cause different bugs,
    //because, OnEnable is called before Start().
    private bool initialised = false;

    // Use this for initialization
    void Start () {
        save_System = FindObjectOfType<SaveSystem>();
        allRuneSlots = FindObjectsOfType<Rune_Slot>();
        tooltip = FindObjectOfType<Explore_Tooltip>();
        SetRunes();
        initialised = true;        
	}

    private void SetRunes()
    {      
        switch (save_System.get_Default_Hero().RuneSlots)
        {
            case 3:
                SelectRuneLayout(new string[] { "BasicCircle_Top", "BasicCircle_Bottom_Left", "BasicCircle_Bottom_Right" });
                break;
            case 4:
                SelectRuneLayout(new string[] { "BasicCircle_Top", "BasicCircle_Bottom_Left", "BasicCircle_Bottom_Right" , "BasicCircle_Bottom" });
                break;
            case 5:
                SelectRuneLayout(new string[] { "BasicCircle_Bottom_Left", "BasicCircle_Bottom_Right", "BasicCircle_Bottom" , "BasicCircle_Top_Left", "BasicCircle_Top_Right" });
                break;
            case 6:
                SelectRuneLayout(new string[] { "BasicCircle_Top", "BasicCircle_Bottom_Left", "BasicCircle_Bottom_Right", "BasicCircle_Bottom", "BasicCircle_Top_Left", "BasicCircle_Top_Right" });
                break;
            default:
                SelectRuneLayout(new string[] { "BasicCircle_Top", "BasicCircle_Bottom" });
            break;
        }
    }
    
    public void SelectRuneLayout(string[] runeNames)
    {
        for (int i = 0; i < allRuneSlots.Length; i++)
        {
            allRuneSlots[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < allRuneSlots.Length; i++)
        {
            for (int j = 0; j < runeNames.Length; j++)
            {
                if(allRuneSlots[i].name == runeNames[j])                   
                {
                    allRuneSlots[i].gameObject.SetActive(true);
                    runeSlots.Add(allRuneSlots[i]);                  
                }
            }
        }
    }
    public void ClearRunes()
    {
        for (int i = 0; i < runeSlots.Count; i++)
        {
            runeSlots[i].DestroyRune();
        }
        runeSlots.Clear();
    }
    public void AddRune(Data_Item_Explore item)
    {
        for (int i = 0; i < runeSlots.Count; i++)
        {
            if (runeSlots[i].Rune_Slot_Is_Emty)
            {
                runeSlots[i].InsertRune(item);

                tooltip.DestroyItem(tooltip.getItem());
                tooltip.Deactivate(false);

                return;
            }
        }
        FindObjectOfType<Explore_Tooltip>().SetDescription("Full slots");
    }

    public void AddRandomRune(Data_Item_Explore item)
    {
        item.item = FindObjectOfType<Inventory_Explore>().getDatabaseItems().getItem(Random.Range(6, 11));
        
        for (int i = 0; i < runeSlots.Count; i++)
        {
            if (runeSlots[i].Rune_Slot_Is_Emty)
            {
                runeSlots[i].InsertRune(item);

                tooltip.SetRuneSlot(runeSlots[i]);
                tooltip.DestroyItem(tooltip.getItem());

                tooltip.Deactivate(false);

                return;
            }
        }
        Debug.Log("1");
        FindObjectOfType<Explore_Tooltip>().SetDescription("Full slots");
    }
    
    public bool Slots_Are_Full()
    {
        for (int i = 0; i < runeSlots.Count; i++)
        {
            if (runeSlots[i].Rune_Slot_Is_Emty)
                return false;
        }
        return true;
    }

    public List<Rune_Slot> GetRuneSlots()
    {
        return runeSlots;
    }
}
