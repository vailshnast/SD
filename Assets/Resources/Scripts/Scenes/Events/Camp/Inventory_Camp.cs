using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class Inventory_Camp : MonoBehaviour
{
    #region Variables

    public class Camp_Item
    {
        public int item_ID;
        public int maxCount;
        public int chance;
        public int priority;

        public Camp_Item(int item_ID, int maxCount, int chance, int priority)
        {
            this.item_ID = item_ID;
            this.maxCount = maxCount;
            this.chance = chance;
            this.priority = priority;
        }

    }


    public SaveSystem save_system;

    private Database_Items database_items;

    private GameObject tooltip;

    private GameObject inventory_panel_slot;

    private GameObject inventory_slot;
    private GameObject inventory_item;

    private GameObject reward_panel_slot;

    public List<GameObject> inventory_slot_list = new List<GameObject>();

    public List<Item> inventory_item_list = new List<Item>();
    #endregion
    //If we dont check this in OnEnable, will cause different bugs,
    //because, OnEnable is called before Start().
    private bool initialised = false;
    void Start()
    {
        Init();
    }
    void OnEnable()
    {
        if (initialised)
        {
            Info_Load_Gold();
            DeleteItems(0);
            for (int i = 0; i < 10; i++)
            {
                if (save_system.chosed_hero_inventory[i] != 0)
                    Add_Item(save_system.chosed_hero_inventory[i], i);
            }
            AddReward(new Camp_Item(2, 2, 15, 1), new Camp_Item(5, 2, 15, 1), new Camp_Item(12, 1, 15, 2), new Camp_Item(11, 2, 15, 3));
        }
    }
    private void Init()
    {
        save_system = GameObject.Find("Save_System").GetComponent<SaveSystem>();

        database_items = gameObject.GetComponent<Database_Items>();

        inventory_slot = Resources.Load("Prefabs/Slot_Camp", typeof(GameObject)) as GameObject;
        inventory_item = Resources.Load("Prefabs/Item_Camp", typeof(GameObject)) as GameObject;

        inventory_panel_slot = transform.FindChild("Panel_Items").FindChild("Slots_Panel").gameObject;
        reward_panel_slot = transform.FindChild("Reward_Items").FindChild("Slots_Panel").gameObject;

        Initiate_Inventory();
        initialised = true;

        tooltip = GameObject.Find("Tooltip_Chest");
        FindObjectOfType<Camp_Tooltip>().inventory = this;
        gameObject.SetActive(false);
    }

    public Database_Items getDatabaseItems()
    {
        return database_items;
    }
    private void Initiate_Inventory()
    {
        for (int i = 0; i < 15; i++)
        {
            if (i < 10)
            {
                inventory_item_list.Add(new Item());
                inventory_slot_list.Add(Instantiate(inventory_slot));
                inventory_slot_list[i].name = "Slot#" + (i + 1);

                inventory_slot_list[i].transform.GetComponent<Data_Slot_Camp>().item_id = i;
                inventory_slot_list[i].transform.SetParent(inventory_panel_slot.transform, false);
            }

            if (i >= 10)
            {
                inventory_item_list.Add(new Item());
                inventory_slot_list.Add(Instantiate(inventory_slot));
                inventory_slot_list[i].name = "Slot#" + (i + 1);

                inventory_slot_list[i].transform.GetComponent<Data_Slot_Camp>().item_id = i;
                inventory_slot_list[i].transform.SetParent(reward_panel_slot.transform, false);
            }

        }

        for (int i = 0; i < 10; i++)
        {
            if (save_system.chosed_hero_inventory[i] != 0)
                Add_Item(save_system.chosed_hero_inventory[i], i);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            DeleteItems(10);
            AddReward();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            for (int i = 10; i < 15; i++)
            {
                if (inventory_slot_list[i].transform.childCount > 0)
                {
                    Debug.Log(inventory_slot_list[i].transform.GetChild(0).GetComponent<Data_Item_Camp>().item.Title);
                }
            }
        }
    }
    public void TakeAll()
    {
        if (!SlotsAreFull())
        {
            for (int i = 10; i < 15; i++)
            {
                //if (inventory_slot_list[i].transform.childCount > 0)
                //Debug.Log(inventory_slot_list[i].transform.GetChild(0).GetComponent<Data_Item_Reward>().item.ID);
                if (inventory_slot_list[i].transform.childCount > 0 && inventory_slot_list[i].transform.GetChild(0).GetComponent<Data_Item_Camp>().item.ID != 0)
                {
                    Add_Item(inventory_slot_list[i].transform.GetChild(0).GetComponent<Data_Item_Camp>().item.ID, 0);
                    DeleteItem(i);
                }

            }
        }
    }
    private bool SlotsAreFull()
    {
        for (int i = 0; i < 10; i++)
        {
            if (inventory_item_list[i].ID == 0)
                return false;
        }
        return true;
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
    private void DeleteItem(int index)
    {
        if (inventory_slot_list[index].transform.childCount > 0)
            Destroy(inventory_slot_list[index].transform.GetChild(0).gameObject);

        inventory_item_list[index] = new Item();
    }

    public void AddReward(params Camp_Item[] camp_Items)
    {
        float randomChance = 0;
        int index = 0;

        camp_Items = camp_Items.OrderBy(w => w.priority).ToArray();


        for (int i = 0; i < camp_Items.Length; i++)
        {
            Debug.Log(camp_Items[i].priority);
            for (int j = 0; j < camp_Items[i].maxCount; j++)
            {
                randomChance = Random.value * 100;

                if (randomChance < camp_Items[i].chance && index < 5)
                {
                    Add_Item(camp_Items[i].item_ID, 10);
                    index++;
                    //Debug.Log("random: " + randomChance + " / Chance_To_Get: " + monsters.getMonster(0).rewards[i].Chance_To_Get);
                }
            }
        }

    }

    private void Add_Item(int id, int index)
    {
        Item candidateItem = database_items.getItem(id);
        for (int i = index; i < inventory_item_list.Count; i++)
        {
            if (inventory_item_list[i].ID == 0)
            {
                // To do: Fix the item sorting bug
                inventory_item_list[i] = candidateItem;
                GameObject new_item = Instantiate(inventory_item);
                new_item.GetComponent<Data_Item_Camp>().item = candidateItem;
                new_item.GetComponent<Data_Item_Camp>().slot = i;
                new_item.GetComponent<Image>().sprite = candidateItem.GetSprite();
                new_item.name = candidateItem.Title;
                new_item.transform.SetParent(inventory_slot_list[i].transform, false);
                break;
            }
        }
    }

    private bool CheckItemPresence(Item item_parsed)
    {
        foreach (Item item_processed in inventory_item_list as IList)
        {
            if (item_parsed.ID == item_processed.ID)
                return true;
        }
        return false;
    }

    public void DestroyRune(Item item)
    {
        save_system.DestroyRune(item);
    }
    public void DrinkPotion(Item item)
    {
        save_system.DrinkPotion(item);
    }
    public void UseOil(int torchesCount)
    {
        save_system.UseOil(torchesCount);
    }

    public void Info_Load_Hp_Panel()
    {
        float current_hp = (save_system.get_sd_char().Health_Current.value / (float)save_system.get_sd_char().Health.value);

        GameObject.Find("HP_MANA_Mask").GetComponent<Image>().fillAmount = current_hp;
        GameObject.Find("HP_MANA_Text").GetComponent<Text>().text = save_system.get_sd_char().Health_Current.value + "/" + save_system.get_sd_char().Health.value;
    }
    public void Info_Load_Mana_Panel()
    {
        float current_mana = (save_system.get_sd_char().Mana_Current.value / (float)save_system.get_sd_char().Mana.value);

        GameObject.Find("HP_MANA_Mask").GetComponent<Image>().fillAmount = current_mana;
        GameObject.Find("HP_MANA_Text").GetComponent<Text>().text = save_system.get_sd_char().Mana_Current.value + "/" + save_system.get_sd_char().Mana.value;
    }
    public void Info_Load_Torch_Panel()
    {
        GameObject.Find("Tooltip_Torch_Count").GetComponent<Text>().text = save_system.get_sd_char().Torch_Quantity.ToString();
    }
    public void Info_Load_Gold()
    {
        GameObject.Find("Text_gold_quantity").GetComponent<Text>().text = save_system.get_sd_char().Gold_Quantity.ToString();
    }

    public int[] parse_inventory()
    {
        int[] inventory = new int[10];
        for (int a = 0; a < 10; a++)
        {
            //inventory[a] = inventory_item_list[a].ID;
            if (inventory_slot_list[a].transform.childCount > 0)
            {
                inventory[a] = inventory_slot_list[a].transform.GetChild(0).GetComponent<Data_Item_Camp>().item.ID;
                //Debug.Log(inventory_slot_list[a].transform.GetChild(0).GetComponent<Data_Item_Reward>().item.Title);
            }

        }

        return inventory;
    }

    public void parse_information()
    {
        save_system.set_chosed_hero_inv(parse_inventory());

    }
}
