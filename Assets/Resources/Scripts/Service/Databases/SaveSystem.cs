using UnityEngine;
using System.Collections;
using LitJson;
using System.IO;

using System;
using System.Collections.Generic;

public class SaveSystem : Database
{
    public static SaveSystem Instance { get; private set; }
    private SD_Character sd_char;
    private GameObject current_Node;

    private Database_saves database_saves;
    private Database_Heroes database_heroes;
    private Database_Settings database_settings;

    private Rune_System rune_System;

    public int chosed_hero_id;
    public int[] chosed_hero_inventory;


    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        Instance = this;

        database_saves = gameObject.GetComponent<Database_saves>();
        database_heroes = GameObject.Find("EventSystem").GetComponent<Database_Heroes>();
        database_settings = GameObject.Find("Save_System").GetComponent<Database_Settings>();
        chosed_hero_id = 0;
        chosed_hero_inventory = new int[20];
    }


    public void InsertRune(Item item)
    {
        sd_char.AddStats(item.stats.Health, item.stats.Mana, item.stats.Defence, item.stats.Damage, item.stats.Magic, item.stats.Power);
    }
    public void DestroyRune(Item item)
    {
        sd_char.AddStats(-item.stats.Health, -item.stats.Mana, -item.stats.Defence, -item.stats.Damage, -item.stats.Magic, -item.stats.Power);
    }

    public void DrinkPotion(Item item)
    {
        sd_char.AddPotionStats(item.stats.Health, item.stats.Mana);
    }
    public void UseOil(int torchesCount)
    {
        sd_char.Torch_Quantity += torchesCount;
    }
    public void SaveRunes()
    {
        rune_System = FindObjectOfType<Rune_System>();
        List<Rune_Slot> slots = rune_System.GetRuneSlots();

        Debug.Log(sd_char.List_Runes.Length);
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].Rune_Slot_Is_Emty)
                sd_char.List_Runes[i] = -1;
            else
                sd_char.List_Runes[i] = slots[i].GetRuneItem().ID;
        }
        save();
    }
    public void CreateNewSaveFile()
    {
        database_saves.CreateEmtySaveFile();
    }
    public void save_base()
    {
        database_saves.save_base();
    }

    public void save_character()
    {
        database_saves.save_character(sd_char);
    }

    public void set_Loaded_hero()
    {
        sd_char = database_saves.get_character();
    }

    public void set_chosed_hero_id(int id_hero)
    {
        chosed_hero_id = id_hero;
        sd_char = new SD_Character
            (
            "Default",
            database_heroes.getHero(id_hero).Title,
            database_heroes.getHero(id_hero).Health,
            database_heroes.getHero(id_hero).Mana,
            database_heroes.getHero(id_hero).Damage,
            database_heroes.getHero(id_hero).Defence,
            database_heroes.getHero(id_hero).Power,
            database_heroes.getHero(id_hero).Magic,
            database_heroes.getHero(id_hero).Starting_Gold,
            database_settings.get_Current_Setting().Starting_Torches_Count,
            database_heroes.getHero(id_hero).Items,
            database_heroes.getHero(id_hero).RuneSlots
            );
    }

    public Hero get_Default_Hero(string Class)
    {
        return database_heroes.getHero(Class);
    }
    public Hero get_Default_Hero()
    {
        return database_heroes.getHero(sd_char.Class_hero);
    }

    public void set_chosed_hero_inv(int[] inventory)
    {
        chosed_hero_inventory = inventory;
    }

    public void set_chosed_hero_gold_quantity(int gold)
    {
        sd_char.Gold_Quantity = gold;
    }

    public int get_chosed_hero_gold_quantity()
    {
        return sd_char.Gold_Quantity;
    }

    public SD_Character get_sd_char()
    {
        return sd_char;
    }
    public Setting get_Setting()
    {
        return database_settings.get_Current_Setting();
    }
    public void set_sd_char(SD_Character character)
    {
        sd_char = character;
        save();
    }
    public void set_Node(GameObject node)
    {
        current_Node = node;
    }
    public GameObject get_Node()
    {
        return current_Node;
    }
    public bool Check_Item_Existance(int id)
    {
        for (int i = 0; i < chosed_hero_inventory.Length; i++)
        {
            if (chosed_hero_inventory[i] == id)
                return true;
        }
        return false;
    }
    public void Delete_Item(int id)
    {
        for (int i = 0; i < chosed_hero_inventory.Length; i++)
        {
            if(chosed_hero_inventory[i] == id)
            {
                chosed_hero_inventory[i] = 0;
                return;
            }
        }
    }
    public void save()
    {
        save_character();
        save_base();
    }
    
}
