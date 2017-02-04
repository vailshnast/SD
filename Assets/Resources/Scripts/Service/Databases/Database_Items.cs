using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;
using UnityEngine.UI;

public class Database_Items : Database
{
    private Items database_items;
    private JsonData json_itemData;

    void Awake()
    {
        json_itemData = LoadBase("Items");
        Initialise_Items_Database(json_itemData.ToJson(), "Items");       
    }

//Поиск предмета по ID в базе данных предметов внутри Unity
    public Item getItem(int id)
    {
        for (int i = 0; i < database_items.items.Count; i++)
        {
            if (database_items.items[i].ID == id)
                return database_items.items[i];
        }
        return null;

    }


    //Загрзука БД в Unity
    public void Initialise_Items_Database(string file, string baseName)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (file == "")
            {
                database_items = new Items();

                database_items.items.Add(new Item(0, "void", "0", "", "", 0, false, 0, 0, 0, 0, 0, 0));
                database_items.items.Add(new Item(1, "scroll", "Scroll", "Rune Scroll", "Use to identify rune", 100, false, 0, 0, 0, 0, 0, 0));
                database_items.items.Add(new Item(2, "potion", "Potion", "Health Potion", "Restore 25 health", 250, false, 25, 0, 0, 0, 0, 0));
                database_items.items.Add(new Item(3, "oil", "Torch", "Torch oil", "Gives 5 torch charges", 100, false, 0, 0, 0, 0, 0, 0));
                database_items.items.Add(new Item(4, "rune", "Rune", "Rune", "Mysterious rune", 1000, false, 0, 0, 0, 0, 0, 0));
                database_items.items.Add(new Item(5, "potionMana", "Potion", "Mana Potion", "Restore 25 mana", 250, false, 0, 25, 0, 0, 0, 0));
                database_items.items.Add(new Item(6, "jo", "Rune", "Jo Rune", "+10 health", 0, false, 10, 0, 0, 0, 0, 0));
                database_items.items.Add(new Item(7, "yok", "Rune", "Yok Rune", "+3 defence", 0, false, 0, 0, 3, 0, 0, 0));
                database_items.items.Add(new Item(8, "rab", "Rune", "Rab Rune", "+5 damage", 0, false, 0, 0, 0, 5, 0, 0));
                database_items.items.Add(new Item(9, "doo", "Rune", "Doo Rune", "+75% power", 0, false, 0, 0, 0, 0, 0, 75));
                database_items.items.Add(new Item(10, "zo", "Rune", "Zo Rune", "+5 damage\n+50% power", 0, false, 0, 0, 0, 5, 0, 50));
                database_items.items.Add(new Item(11, "Treasure", "Treasure", "Treasure", "You can sell it\nFor some gold", 200, false, 0, 0, 0, 0, 0, 0));
                database_items.items.Add(new Item(12, "Lockpick", "Lockpick", "Lockpick", "Use to lock pick\na chest", 200, false, 0, 0, 0, 0, 0, 0));
                string filepath = Application.persistentDataPath + "/" + baseName + ".json";

                if (!File.Exists(filepath))

                {

                    WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + baseName + ".json");
                    //TODO A coroutine instead of this dangerous cliff
                    while (!loadDB.isDone) { }
                    File.WriteAllBytes(filepath, loadDB.bytes);
                }
                File.WriteAllText(filepath, PrettyPrint(database_items));
                SetSprites();
            }
            else
            {
                database_items = JsonMapper.ToObject<Items>(json_itemData.ToJson());
                SetSprites();
            }

        }
        else
        {
            if (file == "")
            {
                database_items = new Items();

                database_items.items.Add(new Item(0, "void", "0", "", "", 0, false, 0, 0, 0, 0, 0, 0));
                database_items.items.Add(new Item(1, "scroll", "Scroll", "Rune Scroll", "Use to identify rune", 100, false, 0, 0, 0, 0, 0, 0));
                database_items.items.Add(new Item(2, "potion", "Potion", "Health Potion", "Restore 25 health", 250, false, 25, 0, 0, 0, 0, 0));
                database_items.items.Add(new Item(3, "oil", "Torch", "Torch oil", "Gives 5 torch charges", 100, false, 0, 0, 0, 0, 0, 0));
                database_items.items.Add(new Item(4, "rune", "Rune", "Rune", "Mysterious rune", 1000, false, 0, 0, 0, 0, 0, 0));
                database_items.items.Add(new Item(5, "potionMana", "Potion", "Mana Potion", "Restore 25 mana", 250, false, 0, 25, 0, 0, 0, 0));
                database_items.items.Add(new Item(6, "jo", "Rune", "Jo Rune", "+10 health", 0, false, 10, 0, 0, 0, 0, 0));
                database_items.items.Add(new Item(7, "yok", "Rune", "Yok Rune", "+3 defence", 0, false, 0, 0, 3, 0, 0, 0));
                database_items.items.Add(new Item(8, "rab", "Rune", "Rab Rune", "+5 damage", 0, false, 0, 0, 0, 5, 0, 0));
                database_items.items.Add(new Item(9, "doo", "Rune", "Doo Rune", "+75% power", 0, false, 0, 0, 0, 0, 0, 75));
                database_items.items.Add(new Item(10, "zo", "Rune", "Zo Rune", "+5 damage\n+50% power", 0, false, 0, 0, 0, 5, 0, 50));
                database_items.items.Add(new Item(11, "Treasure", "Treasure", "Treasure", "You can sell it\nFor some gold", 200, false, 0, 0, 0, 0, 0, 0));
                database_items.items.Add(new Item(12, "Lockpick", "Lockpick", "Lockpick", "Use to lock pick\na chest", 200, false, 0, 0, 0, 0, 0, 0));

                File.WriteAllText(Application.dataPath + "/" + baseName + ".json", PrettyPrint(database_items));
                SetSprites();
            }
            else
            {
                database_items = JsonMapper.ToObject<Items>(json_itemData.ToJson());
                SetSprites();
            }

        }
    }
    public void SetSprites()
    {
        for (int i = 0; i < database_items.items.Count; i++)
        {
            database_items.items[i].SetSprite();
        }
    }
}

public class Items
{
    public List<Item> items = new List<Item>();
    
    public Items()
    {

    }
}
//Класс предмета в Unity
public class Item
{   
    public int ID { get; set; }
    public string Slug { get; set; }
    public string Group { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public bool Stackable { get; set; }
    private Sprite Sprite { get; set; }

    public Stats stats;
    


    // Конструктор предмета
    public Item(int id, string slug, string group, string title, string description, int price, bool stackable , int health,int mana, int defence, int damage , int magic , int power)
    {
        stats = new Stats(health,mana,defence,damage,magic,power);
        
        this.ID = id;
        this.Slug = slug;
        this.Group = group;
        this.Title = title;
        this.Description = description;
        this.Price = price;
        this.Stackable = stackable;
        
    }

    //Конструктор пустого предмета
    public Item()
    {
        this.ID = 0;
    }

    public void SetSprite()
    {
        this.Sprite = Resources.Load<Sprite>("Sprites/Items/" + Slug);
    }
    public Sprite GetSprite()
    {
        return Sprite;
    }
}
public class Stats
{
    public int Health;
    public int Mana;
    public int Defence;
    public int Damage;
    public int Magic;
    public int Power;

    public Stats(int health, int mana, int defence, int damage, int magic, int power)
    {
        Health = health;
        Mana = mana;
        Defence = defence;
        Damage = damage;
        Magic = magic;
        Power = power;
    }
    public Stats()
    {

    }
}
