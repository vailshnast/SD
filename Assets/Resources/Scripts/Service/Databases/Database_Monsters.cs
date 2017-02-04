using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using LitJson;
using System.IO;
using UnityEngine.UI;

public class Database_Monsters : Database
{
    private Monsters database_Monsters;
    private JsonData json_monsterData;
    void Awake()
    {
        json_monsterData = LoadBase("Monsters");
        Initialise_Monsters_Database(json_monsterData.ToJson(), "Monsters");
    }


    //Поиск предмета по ID в базе данных предметов внутри Unity
    public Monster getMonster(int id)
    {
        for (int i = 0; i < database_Monsters.monsters.Count; i++)
        {
            if (database_Monsters.monsters[i].ID == id)
                return database_Monsters.monsters[i];
        }
        return null;

    }

    //Загрзука БД в Unity
    public void Initialise_Monsters_Database(string file, string baseName)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (file == "")
            {
                database_Monsters = new Monsters();
                database_Monsters.monsters.Add(new Monster(0, "Ghoul", "Enemy sprites", "Danger", 750, 10, 6, 24, 5.0, 0.65));
                database_Monsters.monsters[0].rewards.Add(new Reward(4, 10, 1, 1));
                database_Monsters.monsters[0].rewards.Add(new Reward(2, 20, 2, 2));
                database_Monsters.monsters[0].rewards.Add(new Reward(5, 20, 2, 2));
                database_Monsters.monsters[0].rewards.Add(new Reward(1, 10, 3, 2));
                database_Monsters.monsters[0].rewards.Add(new Reward(3, 10, 3, 2));
                database_Monsters.monsters[0].rewards.Add(new Reward(11, 20, 4, 2));

                string filepath = Application.persistentDataPath + "/" + baseName + ".json";
                if (!File.Exists(filepath))

                {

                    WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + baseName + ".json");
                    //TODO A coroutine instead of this dangerous cliff
                    while (!loadDB.isDone) { }
                    File.WriteAllBytes(filepath, loadDB.bytes);
                }
                File.WriteAllText(filepath, PrettyPrint(database_Monsters));
                SetSprites();
            }
            else
            {
                database_Monsters = JsonMapper.ToObject<Monsters>(json_monsterData.ToJson());
                SetSprites();
            }

        }
        else
        {
            if (file == "")
            {
                database_Monsters = new Monsters();
                database_Monsters.monsters.Add(new Monster(0, "Ghoul", "Enemy sprites", "Danger", 750, 10, 6, 24, 5.0, 0.65));
                database_Monsters.monsters[0].rewards.Add(new Reward(4, 10, 1, 1));
                database_Monsters.monsters[0].rewards.Add(new Reward(2, 20, 2, 2));
                database_Monsters.monsters[0].rewards.Add(new Reward(5, 20, 2, 2));
                database_Monsters.monsters[0].rewards.Add(new Reward(1, 10, 3, 2));
                database_Monsters.monsters[0].rewards.Add(new Reward(3, 10, 3, 2));
                database_Monsters.monsters[0].rewards.Add(new Reward(11, 20, 4, 2));
                File.WriteAllText(Application.dataPath + "/" + baseName + ".json", PrettyPrint(database_Monsters));
                SetSprites();
            }
            else
            {
                database_Monsters = JsonMapper.ToObject<Monsters>(json_monsterData.ToJson());
                SetSprites();
            }

        }
    }

    public void SetSprites()
    {
        for (int i = 0; i < database_Monsters.monsters.Count; i++)
        {
            database_Monsters.monsters[i].SetSprites();
        }
    }
}
public class Monsters
{
    public List<Monster> monsters = new List<Monster>();

    public Monsters()
    {

    }
}
public class Monster
{
    public int ID { get; set; }
    public string MonsterName { get; set; }
    public string SpriteName { get; set; }
    public string Description { get; set; }
    public int Health { get; set; }
    public int Dodge { get; set; }
    public int Defence { get; set; }
    public int Damage { get; set; }
    public double PrepareTime { get; set; }
    public double SwingTime { get; set; }
    public List<Reward> rewards = new List<Reward>();
    private Dictionary<ImageDirection, Sprite> Sprites = new Dictionary<ImageDirection, Sprite>();


    //array of monster id's represents the inventory
    public Monster(int id, string monsterName, string spriteName, string description, int health, int dodge, int defence, int damage, double prepareTime, double swingTime)
    {
        this.ID = id;
        this.MonsterName = monsterName;
        this.SpriteName = spriteName;
        this.Description = description;
        this.Health = health;
        this.Dodge = dodge;
        this.Defence = defence;
        this.Damage = damage;
        this.PrepareTime = prepareTime;
        this.SwingTime = swingTime;
    }
    public Monster()
    {

    }
    public Dictionary<ImageDirection, Sprite> GetSprites()
    {
        return Sprites;
    }
    public void SetSprites()
    {
        Sprites.Add(ImageDirection.Main, null);
        Sprites.Add(ImageDirection.Left, null);
        Sprites.Add(ImageDirection.Right, null);

        this.Sprites[ImageDirection.Right] = Resources.LoadAll<Sprite>("Sprites/CorePrototype/Battle/" + SpriteName)[0];
        this.Sprites[ImageDirection.Left] = Resources.LoadAll<Sprite>("Sprites/CorePrototype/Battle/" + SpriteName)[1];
        this.Sprites[ImageDirection.Main] = Resources.LoadAll<Sprite>("Sprites/CorePrototype/Battle/" + SpriteName)[2];
    }
}
public class Reward
{
    public int item_ID;
    public int Chance_To_Get;
    public int Priority;
    public int Max_Count;

    public Reward(int Item_ID, int chance_To_Get,int priority, int max_Count)
    {
        item_ID = Item_ID;
        Chance_To_Get = chance_To_Get;
        Priority = priority;
        Max_Count = max_Count;
    }
    public Reward()
    {

    }
}