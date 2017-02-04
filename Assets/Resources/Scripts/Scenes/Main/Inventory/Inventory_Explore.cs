using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory_Explore : MonoBehaviour
{
    private SaveSystem save_system;

    private Database_Items database_items;

    private GameObject tooltip;

    private GameObject inventory_panel_slot;
    private GameObject inventory_slot;
    public List<GameObject> inventory_slot_list = new List<GameObject>();
    private GameObject inventory_item;
    public List<Item> inventory_item_list = new List<Item>();

    //If we dont check this in OnEnable, will cause different bugs,
    //because, OnEnable is called before Start().
    private bool initialised = false;

    void Start () {
        Init();
    }

    private void Init()
    {
        save_system = GameObject.Find("Save_System").GetComponent<SaveSystem>();
        database_items = gameObject.GetComponent<Database_Items>();

        inventory_slot = Resources.Load("Prefabs/Slot_Explore", typeof(GameObject)) as GameObject;
        inventory_item = Resources.Load("Prefabs/Item_Explore", typeof(GameObject)) as GameObject;

        inventory_panel_slot = transform.FindChild("Panel_Items").FindChild("Slots_Panel").gameObject;

        loadSkills();
        Initiate_Inventory();
        Info_Load();

        GameObject.Find("Stats_panel").SetActive(false);
        initialised = true;
    }
    void OnEnable()
    {
        if (initialised)
        {
            Info_Load();
            DeleteItems(0);
            for (int i = 0; i < 10; i++)
            {
                if (save_system.chosed_hero_inventory[i] != 0)
                    Add_Item(save_system.chosed_hero_inventory[i],i);
            }
        }
    }
    private void DeleteItems(int index)
    {
        for (int i = index; i < inventory_slot_list.Count; i++)
        {
            if (inventory_slot_list[i].transform.childCount > 0)
                Destroy(inventory_slot_list[i].transform.GetChild(0).gameObject);
            inventory_item_list[i] = new Item();
        }

    }
    public Database_Items getDatabaseItems()
    {
        return database_items;
    }
    private void Initiate_Inventory()
    {
        for (int i = 0; i < 10; i++)
        {
            inventory_item_list.Add(new Item());
            inventory_slot_list.Add(Instantiate(inventory_slot));
            inventory_slot_list[i].name = "Slot#" + (i + 1);

            inventory_slot_list[i].transform.GetComponent<Data_Slot_Explore>().item_id = i;
            inventory_slot_list[i].transform.SetParent(inventory_panel_slot.transform, false);
        }
        for (int i = 0; i < 10; i++)
        {
            if (save_system.chosed_hero_inventory[i] != 0)
                Add_Item(save_system.chosed_hero_inventory[i],i);
        }
    }

    private void Add_Item(int id , int index)
    {
        Item candidateItem = database_items.getItem(id);
        for (int i = index; i < inventory_item_list.Count; i++)
        {
            if (inventory_item_list[i].ID == 0)
            {
                // To do: Fix the item sorting bug
                inventory_item_list[i] = candidateItem;
                GameObject new_item = Instantiate(inventory_item);
                new_item.GetComponent<Data_Item_Explore>().item = candidateItem;
                new_item.GetComponent<Data_Item_Explore>().slot = i;
                new_item.GetComponent<Image>().sprite = candidateItem.GetSprite();
                new_item.name = candidateItem.Title;
                new_item.transform.SetParent(inventory_slot_list[i].transform, false);
                break;
            }
        }
    }

    public void InsertRune(Item item)
    {
        save_system.InsertRune(item);
        Info_Load();
    }
    public void DestroyRune(Item item)
    {
        save_system.DestroyRune(item);
        Info_Load();
    }
    public void DrinkPotion(Item item)
    {
        save_system.DrinkPotion(item);
        Info_Load();
    }
    public void UseOil(int torchesCount)
    {
        save_system.UseOil(torchesCount);
    }
    public void hurt()
    {
        SaveSystem.Instance.get_sd_char().Health_Current.value -= 10;
        Info_Load();
    }


    public void Info_Load_Hp_Panel()
    {
        float current_hp = (save_system.get_sd_char().Health_Current.value / (float)save_system.get_sd_char().Health.value);

        GameObject.Find("HP_MANA_Mask").GetComponent<Image>().fillAmount = current_hp;
        GameObject.Find("Tooltip_HP_Text").GetComponent<Text>().text = save_system.get_sd_char().Health_Current.value + "/" + save_system.get_sd_char().Health.value;
    }
    public void Info_Load_Mana_Panel()
    {
        float current_mana = (save_system.get_sd_char().Mana_Current.value / (float)save_system.get_sd_char().Mana.value);

        GameObject.Find("HP_MANA_Mask").GetComponent<Image>().fillAmount = current_mana;
        GameObject.Find("Tooltip_HP_Text").GetComponent<Text>().text = save_system.get_sd_char().Mana_Current.value + "/" + save_system.get_sd_char().Mana.value;
    }
    public void Info_Load_Torch_Panel()
    {
        GameObject.Find("Tooltip_Torch_Count").GetComponent<Text>().text = save_system.get_sd_char().Torch_Quantity.ToString();
    }
    public void Info_Load()
    {
        GameObject.Find("Name").GetComponent<Text>().text = save_system.get_sd_char().Class_hero.ToString();
        //
        gameObject.transform.FindChild("Panel_stats").FindChild("Stat_Defence").FindChild("Stat").GetComponent<Text>().text = save_system.get_sd_char().Defence.value.ToString();
        gameObject.transform.FindChild("Panel_stats").FindChild("Stat_Damage").FindChild("Stat").GetComponent<Text>().text = save_system.get_sd_char().Damage.value.ToString();
        gameObject.transform.FindChild("Panel_stats").FindChild("Stat_Finalblow").FindChild("Stat").GetComponent<Text>().text = (save_system.get_sd_char().Power.value / 100 * save_system.get_sd_char().Damage.value).ToString();
        gameObject.transform.FindChild("Panel_stats").FindChild("Stat_Power").FindChild("Stat").GetComponent<Text>().text = save_system.get_sd_char().Power.value.ToString() + "%";
        gameObject.transform.FindChild("Panel_stats").FindChild("Stat_Magic").FindChild("Stat").GetComponent<Text>().text = save_system.get_sd_char().Magic.value.ToString() + "%";
        //
        gameObject.transform.FindChild("Panel_Items").FindChild("Panel_Coins").FindChild("Text_gold_quantity").GetComponent<Text>().text = save_system.get_chosed_hero_gold_quantity().ToString();
        //
        float current_hp = (float)((float)save_system.get_sd_char().Health_Current.value / (float)save_system.get_sd_char().Health.value);
        float current_mp = (float)((float)save_system.get_sd_char().Mana_Current.value / (float)save_system.get_sd_char().Mana.value);
        //
        GameObject.Find("Mask_HP").GetComponent<Image>().fillAmount = current_hp;
        GameObject.Find("Text_HP").GetComponent<Text>().text = save_system.get_sd_char().Health_Current.value + "/"+ save_system.get_sd_char().Health.value;
        GameObject.Find("Mask_MP").GetComponent<Image>().fillAmount = current_mp;
        GameObject.Find("Text_MP").GetComponent<Text>().text = save_system.get_sd_char().Mana_Current.value + "/" + save_system.get_sd_char().Mana.value;
        
        //Will skills change during game?
        //GameObject.Find("Skill_1").GetComponent<Skill_Slot>().SetSkill(save_system.get_Default_Hero(save_system.get_sd_char().Class_hero).Hero_Skills[0]);
        //GameObject.Find("Skill_2").GetComponent<Skill_Slot>().SetSkill(save_system.get_Default_Hero(save_system.get_sd_char().Class_hero).Hero_Skills[1]);
        //GameObject.Find("Skill_3").GetComponent<Skill_Slot>().SetSkill(save_system.get_Default_Hero(save_system.get_sd_char().Class_hero).Hero_Skills[2]);
    }
    private void loadSkills()
    {
        GameObject.Find("Skill_1").GetComponent<Skill_Slot>().SetSkill(save_system.get_Default_Hero().Hero_Skills[0]);
        GameObject.Find("Skill_2").GetComponent<Skill_Slot>().SetSkill(save_system.get_Default_Hero().Hero_Skills[1]);
        GameObject.Find("Skill_3").GetComponent<Skill_Slot>().SetSkill(save_system.get_Default_Hero().Hero_Skills[2]);
        GameObject.Find("Skill_4").GetComponent<Skill_Slot>().SetSkill(save_system.get_Default_Hero().Hero_Skills[3]);
    }
    public int[] parse_inventory()
    {
        int[] inventory = new int[10];
        for (int a = 0; a < 10; a++)
        {
            //inventory[a] = inventory_item_list[a].ID;
            if (inventory_slot_list[a].transform.childCount > 0)
            {
                inventory[a] = inventory_slot_list[a].transform.GetChild(0).GetComponent<Data_Item_Explore>().item.ID;
                Debug.Log(inventory_slot_list[a].transform.GetChild(0).GetComponent<Data_Item_Explore>().item.Title);
            }
                
        }

        return inventory;
    }

    public void parse_information()
    {
        save_system.set_chosed_hero_inv(parse_inventory());

    }
}
