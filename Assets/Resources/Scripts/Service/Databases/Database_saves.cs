using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;

public class Database_saves : Database
{

    private SD_Character data_save = new SD_Character();
    private JsonData json_slots_save;

    void Awake()
    {
        Initialise();
        //load_characters();
    }
    public SD_Character load_character(int slot)
    {
        return data_save;
    }

    public SD_Character get_character()
    {
        return data_save;
    }

    public void save_character(SD_Character sd_char)
    {
        data_save = sd_char;
    }

    public void Initialise_Save_Database(string file, string baseName)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (file == "")
            {
                data_save = new SD_Character();

                string filepath = Application.persistentDataPath + "/" + baseName + ".json";
                if (!File.Exists(filepath))

                {

                    WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + baseName + ".json");
                    //TODO A coroutine instead of this dangerous cliff
                    while (!loadDB.isDone) { }
                    File.WriteAllBytes(filepath, loadDB.bytes);
                }
                File.WriteAllText(filepath, PrettyPrint(data_save));
            }
            else
            {
                data_save = JsonMapper.ToObject<SD_Character>(json_slots_save.ToJson());
            }

        }
        else
        {
            if (file == "")
            {
                data_save = new SD_Character();
                File.WriteAllText(Application.dataPath + "/" + baseName + ".json", PrettyPrint(data_save));
            }
            else
            {
                data_save = JsonMapper.ToObject<SD_Character>(json_slots_save.ToJson());
            }

        }
    }
    public void NewFile(string file, string baseName)
    {
        if (Application.platform == RuntimePlatform.Android)
        {

            data_save = new SD_Character();

            string filepath = Application.persistentDataPath + "/" + baseName + ".json";
            Debug.Log("Running system");
            if (!File.Exists(filepath))

            {

                WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + baseName + ".json");
                //TODO A coroutine instead of this dangerous cliff
                while (!loadDB.isDone) { }
                File.WriteAllBytes(filepath, loadDB.bytes);
            }
            File.WriteAllText(filepath, PrettyPrint(data_save));
        }
        else
        {
            data_save = new SD_Character();
            File.WriteAllText(Application.dataPath + "/" + baseName + ".json", PrettyPrint(data_save));

        }
    }
    private void Initialise()
    {
        json_slots_save = LoadBase("Save_slots");
        Initialise_Save_Database(json_slots_save.ToJson(), "Save_slots");
    }

    public void CreateEmtySaveFile()
    {
        json_slots_save = LoadBase("Save_slots");
        NewFile(json_slots_save.ToJson(), "Save_slots");
    }

    public void save_base()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            string filepath = Application.persistentDataPath + "/" + "Save_slots" + ".json";
            Debug.Log("Running system");
            if (!File.Exists(filepath))

            {

                WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + "Save_slots" + ".json");
                //TODO A coroutine instead of this dangerous cliff
                while (!loadDB.isDone) { }
                File.WriteAllBytes(filepath, loadDB.bytes);
            }
            File.WriteAllText(filepath, PrettyPrint(data_save));
        }
        else
        {
            File.WriteAllText(Application.dataPath + "/" + "Save_slots" + ".json", PrettyPrint(data_save));
        }
    }

}

public class SD_Character
{
    public Stat[] stats;
    public string Name { get; set; }
    public string Class_hero { get; set; }
    public Stat Health { get; set; }
    public Stat Health_Current { get ; set; }
    public Stat Mana { get; set; }
    public Stat Mana_Current { get; set; }
    public Stat Damage { get; set; }
    public Stat Defence { get; set; }
    public Stat Power { get; set; }
    public Stat Magic { get; set; }
    public int Gold_Quantity { get; set; }
    public int Torch_Quantity { get; set; }
    public int[] List_Runes { get; set; }
    public int[] List_Items { get; set; }

    public SD_Character(string name, string class_hero, Stat health, Stat mana, Stat damage, Stat defence, Stat power, Stat magic, int gold_quantity, int torch_quantity, int[] list_Items, int runeSlots)
    {
        this.Name = name;
        this.Class_hero = class_hero;
        this.Health = new Stat(health.value,"Health");
        this.Health_Current = new Stat(health.value);
        this.Mana = new Stat(mana.value,"Mana");
        this.Mana_Current = new Stat(mana.value);
        this.Damage = new Stat(damage.value,"Damage");
        this.Defence = new Stat(defence.value,"Defence");
        this.Power = new Stat(power.value,"Power");
        this.Magic = new Stat(magic.value,"Magic");
        this.Gold_Quantity = gold_quantity;
        this.Torch_Quantity = torch_quantity;
        this.List_Items = list_Items;
        this.List_Runes = new int[runeSlots];
        for (int i = 0; i < List_Runes.Length; i++)
        {
            List_Runes[i] = -1;
        }
    }

    public SD_Character()
    {

    }
    public void AddStats(int health, int mana, int defence, int damage, int magic, int power)
    {
        this.Health.value += health;
        this.Mana.value += mana;

        if (Health_Current.value > Health.value)
            Health_Current = Health;

        if (Mana_Current.value > Mana.value)
            Mana_Current = Mana;

        this.Damage.value += damage;
        this.Defence.value += defence;
        this.Power.value += power;
        this.Magic.value += magic;
    }
    public void AddPotionStats(int health, int mana)
    {
        if ((Health_Current.value + health) > Health.value)
            Health_Current = Health;
        else
            Health_Current.value += health;

        if ((Mana_Current.value + mana) > Mana.value)
            Mana_Current = Mana;
        else
            Mana_Current.value += mana;
    }
    public void SetRunes(int[] runes)
    {
        List_Runes = runes;
    }
    public void SetItems(int[] items)
    {
        List_Items = items;
    }
}
