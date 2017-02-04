using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;
using System.Text;

public class Database_Heroes : Database
{
    private Heroes database_heroes;
    private JsonData json_heroData;
   
    void Awake () {
        json_heroData = LoadBase("Heroes");
        Initialise_Heroes_Database(json_heroData.ToJson(),"Heroes");
    }
    

    //Поиск предмета по ID в базе данных предметов внутри Unity
    public Hero getHero(int id)
    {
        for (int i = 0; i < database_heroes.heroes.Count; i++)
        {
            if (database_heroes.heroes[i].ID == id)
                return database_heroes.heroes[i];
        }
        return null;

    }
    public Hero getHero(string Class)
    {
        for (int i = 0; i < database_heroes.heroes.Count; i++)
        {
            if (database_heroes.heroes[i].Title == Class)
                return database_heroes.heroes[i];
        }
        return null;

    }
    //Загрзука БД в Unity
    public void Initialise_Heroes_Database(string file, string baseName)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (file == "")
            {
                database_heroes = new Heroes();
                #region Heroes_Default_Android
                database_heroes.heroes.Add(new Hero(0, "HeroWar_0", "Warrior", "SimpleText", 125, 20, 6, 24, 400, 100, 1000, 6, new int[] { },

                new Skill[] { new Skill("Deals fixed damage that is strengthened by Magic.Resets attack phase.",
                                                                   "Mana Splash", "ManaSplash", "ManaSplashSymbol", new SkillParameter[] { new SkillParameter("Manacost: ", "15"), new SkillParameter("Damage: ", "50") } ),

                                                         new Skill("Consumes all your current mana and deals damage proportionally to amount of consumed mana, strengthened by Magic.Resets attack phase.",
                                                                   "Discharge", "Discharge", "DischargeSymbol", new SkillParameter[] { new SkillParameter("Manacost: ", "100%"), new SkillParameter("Magic Modifire: ", "3x") } ),

                                                         new Skill("Hero is granted with a buff, that grants mana for every successful attack up to 1 phase cycle duration. Successful finalblow with buff restores 10 mana, and damage will be magic.",
                                                                   "Mana Leak", "ManaLeak", "ManaLeakSymbol", new SkillParameter[] { new SkillParameter("Manacost: ", "5"), new SkillParameter("Duration: ", "1 cycle") , new SkillParameter("Finalblow modifire: ", "Magic")} ),

                                                         new Skill("Regenerates 2 mana after 1 cycle",
                                                                   "Mana Regeneration", "Passive", "Passive", new SkillParameter[] { })
                }));

                database_heroes.heroes.Add(new Hero(1, "HeroMage_0", "Mage", "SimpleText", 50, 120, 1, 7, 125, 400, 1000, 5, new int[] { 5, 5 },

                                           new Skill[] { new Skill("Deals fixed damage that is strengthened by Magic.Resets attack phase.",
                                                                   "Mana Splash", "ManaSplash", "ManaSplashSymbol", new SkillParameter[] { new SkillParameter("Manacost: ", "15"), new SkillParameter("Damage: ", "50") } ),

                                                         new Skill("Consumes all your current mana and deals damage proportionally to amount of consumed mana, strengthened by Magic.Resets attack phase.",
                                                                   "Discharge", "Discharge", "DischargeSymbol", new SkillParameter[] { new SkillParameter("Manacost: ", "100%"), new SkillParameter("Magic Modifire: ", "3x") } ),

                                                         new Skill("Hero is granted with a buff, that grants mana for every successful attack up to 1 phase cycle duration. Successful finalblow with buff restores 10 mana, and damage will be magic.",
                                                                   "Mana Leak", "ManaLeak", "ManaLeakSymbol", new SkillParameter[] { new SkillParameter("Manacost: ", "5"), new SkillParameter("Duration: ", "1 cycle") , new SkillParameter("Finalblow modifire: ", "Magic")} ),

                                                         new Skill("Regenerates 2 mana after 1 cycle",
                                                                   "Mana Regeneration", "Passive", "Passive", new SkillParameter[] { })
                                           }));

                database_heroes.heroes.Add(new Hero(2, "HeroCena_0", "JOHN CENA", "AND HIS NAME IS", 200, 250, 150, 400, 210, 250, 1000, 6, new int[] { },

                                           new Skill[] { new Skill("Deals fixed damage that is strengthened by Magic.Resets attack phase.",
                                                                   "Mana Splash", "ManaSplash", "ManaSplashSymbol", new SkillParameter[] { new SkillParameter("Manacost: ", "15"), new SkillParameter("Damage: ", "50") } ),

                                                         new Skill("Consumes all your current mana and deals damage proportionally to amount of consumed mana, strengthened by Magic.Resets attack phase.",
                                                                   "Discharge", "Discharge", "DischargeSymbol", new SkillParameter[] { new SkillParameter("Manacost: ", "100%"), new SkillParameter("Magic Modifire: ", "3x") } ),

                                                         new Skill("Hero is granted with a buff, that grants mana for every successful attack up to 1 phase cycle duration. Successful finalblow with buff restores 10 mana, and damage will be magic.",
                                                                   "Mana Leak", "ManaLeak", "ManaLeakSymbol", new SkillParameter[] { new SkillParameter("Manacost: ", "5"), new SkillParameter("Duration: ", "1 cycle") , new SkillParameter("Finalblow modifire: ", "Magic")} ),

                                                         new Skill("Regenerates 2 mana after 1 cycle",
                                                                   "Mana Regeneration", "Passive", "Passive", new SkillParameter[] { })
                                           }));
                #endregion
                string filepath = Application.persistentDataPath + "/" + baseName + ".json";
                if (!File.Exists(filepath))

                {

                    WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + baseName + ".json");
                    //TODO A coroutine instead of this dangerous cliff
                    while (!loadDB.isDone) { }
                    File.WriteAllBytes(filepath, loadDB.bytes);
                }
                File.WriteAllText(filepath, PrettyPrint(database_heroes));
                SetSprites();
            }
            else
            {             
                database_heroes = JsonMapper.ToObject<Heroes>(json_heroData.ToJson());
                SetSprites();
            }

        }
        else
        {
            if (file == "")
            {
                database_heroes = new Heroes();
                #region Heroes_Default_PC
                database_heroes.heroes.Add(new Hero(0, "HeroWar_0", "Warrior", "SimpleText", 125, 20, 6, 24, 400, 100, 1000, 6, new int[] { },

                          new Skill[] { new Skill("Deals fixed damage that is strengthened by Magic.Resets attack phase.",
                                                                   "Mana Splash", "ManaSplash", "ManaSplashSymbol", new SkillParameter[] { new SkillParameter("Manacost: ", "15"), new SkillParameter("Damage: ", "50") } ),

                                                         new Skill("Consumes all your current mana and deals damage proportionally to amount of consumed mana, strengthened by Magic.Resets attack phase.",
                                                                   "Discharge", "Discharge", "DischargeSymbol", new SkillParameter[] { new SkillParameter("Manacost: ", "100%"), new SkillParameter("Magic Modifire: ", "3x") } ),

                                                         new Skill("Hero is granted with a buff, that grants mana for every successful attack up to 1 phase cycle duration. Successful finalblow with buff restores 10 mana, and damage will be magic.",
                                                                   "Mana Leak", "ManaLeak", "ManaLeakSymbol", new SkillParameter[] { new SkillParameter("Manacost: ", "5"), new SkillParameter("Duration: ", "1 cycle") , new SkillParameter("Finalblow modifire: ", "Magic")} ),

                                                         new Skill("Regenerates 2 mana after 1 cycle",
                                                                   "Mana Regeneration", "Passive", "Passive", new SkillParameter[] { })
                          }));

                database_heroes.heroes.Add(new Hero(1, "HeroMage_0", "Mage", "SimpleText", 50, 120, 1, 7, 125, 400, 1000, 5, new int[] { 5, 5 },

                                           new Skill[] { new Skill("Deals fixed damage that is strengthened by Magic.Resets attack phase.",
                                                                   "Mana Splash", "ManaSplash", "ManaSplashSymbol", new SkillParameter[] { new SkillParameter("Manacost: ", "15"), new SkillParameter("Damage: ", "50") } ),

                                                         new Skill("Consumes all your current mana and deals damage proportionally to amount of consumed mana, strengthened by Magic.Resets attack phase.",
                                                                   "Discharge", "Discharge", "DischargeSymbol", new SkillParameter[] { new SkillParameter("Manacost: ", "100%"), new SkillParameter("Magic Modifire: ", "3x") } ),

                                                         new Skill("Hero is granted with a buff, that grants mana for every successful attack up to 1 phase cycle duration. Successful finalblow with buff restores 10 mana, and damage will be magic.",
                                                                   "Mana Leak", "ManaLeak", "ManaLeakSymbol", new SkillParameter[] { new SkillParameter("Manacost: ", "5"), new SkillParameter("Duration: ", "1 cycle") , new SkillParameter("Finalblow modifire: ", "Magic")} ),

                                                         new Skill("Regenerates 2 mana after 1 cycle",
                                                                   "Mana Regeneration", "Passive", "Passive", new SkillParameter[] { })
                                           }));

                database_heroes.heroes.Add(new Hero(2, "HeroCena_0", "JOHN CENA", "AND HIS NAME IS", 200, 250, 150, 400, 210, 250, 1000, 6, new int[] { },

                                           new Skill[] { new Skill("Deals fixed damage that is strengthened by Magic.Resets attack phase.",
                                                                   "Mana Splash", "ManaSplash", "ManaSplashSymbol", new SkillParameter[] { new SkillParameter("Manacost: ", "15"), new SkillParameter("Damage: ", "50") } ),

                                                         new Skill("Consumes all your current mana and deals damage proportionally to amount of consumed mana, strengthened by Magic.Resets attack phase.",
                                                                   "Discharge", "Discharge", "DischargeSymbol", new SkillParameter[] { new SkillParameter("Manacost: ", "100%"), new SkillParameter("Magic Modifire: ", "3x") } ),

                                                         new Skill("Hero is granted with a buff, that grants mana for every successful attack up to 1 phase cycle duration. Successful finalblow with buff restores 10 mana, and damage will be magic.",
                                                                   "Mana Leak", "ManaLeak", "ManaLeakSymbol", new SkillParameter[] { new SkillParameter("Manacost: ", "5"), new SkillParameter("Duration: ", "1 cycle") , new SkillParameter("Finalblow modifire: ", "Magic")} ),

                                                         new Skill("Regenerates 2 mana after 1 cycle",
                                                                   "Mana Regeneration", "Passive", "Passive", new SkillParameter[] { })
                                           }));
                #endregion
                File.WriteAllText(Application.dataPath + "/" + baseName + ".json", PrettyPrint(database_heroes));
                
                SetSprites();
            }
            else
            {
                database_heroes = JsonMapper.ToObject<Heroes>(json_heroData.ToJson());
                SetSprites();
            }

        }
    }

    public int getHeroesCount()
    {
        return database_heroes.heroes.Count;
    }
    public void SetSprites()
    {
        for (int i = 0; i < database_heroes.heroes.Count; i++)
        {
            database_heroes.heroes[i].SetSprite();

            for (int j = 0; j < database_heroes.heroes[i].Hero_Skills.Length; j++)
            {
                database_heroes.heroes[i].Hero_Skills[j].SetSprite();
            }
        }
    }
}

public class Heroes
{
    public List<Hero> heroes = new List<Hero>();
    public Heroes()
    {

    }
}
public class Hero
{
    public int ID { get; set; }
    public string Slug { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public Stat Health { get; set; }
    public Stat Mana { get; set; }
    public Stat Defence { get; set; }
    public Stat Damage { get; set; }
    public Stat Power { get; set; }
    public Stat Magic { get; set; }
    public int Starting_Gold { get; set; }
    public int RuneSlots { get; set; }
    private Sprite Sprite { get; set; }
    public int[] Items { get; set; }

    public Skill[] Hero_Skills;

    public Hero(int id, string slug, string title, string description, int health, int mana,
                int defence, int damage, int power, int magic, int gold_Quantity, int runeSlots, int[] items, Skill[] hero_Skills)
    {
        this.ID = id;
        this.Slug = slug;
        this.Title = title;
        this.Description = description;

        Health = new Stat(health);
        Mana = new Stat(mana);
        Defence = new Stat(defence);
        Damage = new Stat(damage);
        Power = new Stat(power);
        Magic = new Stat(magic);

        this.Starting_Gold = gold_Quantity;
        this.Items = items;
        Hero_Skills = hero_Skills;
        RuneSlots = runeSlots;
    }
    public Hero()
    {

    }
    public void SetSprite()
    {
        Sprite = Resources.Load<Sprite>("Sprites/CorePrototype/HeroChoise/" + Slug);
    }
    public Sprite GetSprite()
    {
        return Sprite;
    }
}
public class Skill
{
    public string Name;
    public string Description;
    public SkillParameter[] Parameters;

    public string SkillSpriteName;
    public string SymbolSpriteName;

    private Sprite skillSprite;
    private Sprite symbolSprite;

    public Skill(string description, string name, string skillSpriteName, string symbolSpriteName , SkillParameter[] parameters)
    {
        Name = name;
        Description = description;
        Parameters = parameters;

        SkillSpriteName = skillSpriteName;
        SymbolSpriteName = symbolSpriteName;       
    }

    public void SetSprite()
    {
        skillSprite = Resources.Load<Sprite>("Sprites/CorePrototype/Arcanist Icons/" + SkillSpriteName);
        symbolSprite = Resources.Load<Sprite>("Sprites/CorePrototype/Arcanist Icons/" + SymbolSpriteName);
    }
    public Sprite GetSkillSprite()
    {
        return skillSprite;
    }
    public Sprite GetSymbolSprite()
    {
        return symbolSprite;
    }
    public Skill()
    {

    }
}
public class SkillParameter
{
    public string Name;
    public string Value;

    public SkillParameter(string name, string value)
    {
        Name = name;
        Value = value;
    }

    public SkillParameter()
    {

    }
}
public class Stat
{
    public int value { get; set; }
    public string name;
    public Stat(int value, string name)
    {
        this.value = value;
        this.name = name;
    }
    public Stat(int value)
    {
        this.value = value;
    }

    public Stat()
    {

    }
}
