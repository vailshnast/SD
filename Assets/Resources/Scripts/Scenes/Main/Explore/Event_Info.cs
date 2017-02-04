using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_Info {

    public int[] inventory { get; private set; }
    public bool[] chest_Combination { get; private set; }
    public Monster monster { get; private set; }

    public Event_Info()
    {
        inventory = null;
        chest_Combination = null;
        monster = null;
    }
    public void Set_Info(int[] inventory)
    {
        this.inventory = inventory;
    }
    public void Set_Info(bool[] chest_Combination)
    {
        this.chest_Combination = chest_Combination;
    }
    public void Set_Info(Monster monster)
    {
        this.monster = monster;
    }
}
