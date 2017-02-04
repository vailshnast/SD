using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class Inventory_Sack : MonoBehaviour
{
    #region Variables

    public class Sack_Item
    {
        public int item_ID;
        public int maxCount;
        public int chance;
        public int priority;

        public Sack_Item(int item_ID,int maxCount, int chance, int priority)
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
           
            if (save_system.get_Node().GetComponent<Node>().event_Info.inventory == null)
                AddReward(new Sack_Item(2, 2, 15, 1), new Sack_Item(5, 2, 15, 1), new Sack_Item(12, 1, 15, 2), new Sack_Item(11, 2, 15, 3));
            else
            {
                Load_Reward();
            }
                
        }

    }
    private void Init()
    {
        save_system = GameObject.Find("Save_System").GetComponent<SaveSystem>();

        database_items = gameObject.GetComponent<Database_Items>();

        inventory_slot = Resources.Load("Prefabs/Slot_Sack", typeof(GameObject)) as GameObject;
        inventory_item = Resources.Load("Prefabs/Item_Sack", typeof(GameObject)) as GameObject;

        inventory_panel_slot = transform.FindChild("Panel_Items").FindChild("Slots_Panel").gameObject;
        reward_panel_slot = transform.FindChild("Reward_Items").FindChild("Slots_Panel").gameObject;

        Initiate_Inventory();
        
        AddReward(new Sack_Item(2, 2, 15, 1), new Sack_Item(5, 2, 15, 1), new Sack_Item(12, 1, 15, 2), new Sack_Item(11, 2, 15, 3));

        Info_Load_Gold();
        initialised = true;
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

                inventory_slot_list[i].transform.GetComponent<Data_Slot_Sack>().item_id = i;
                inventory_slot_list[i].transform.SetParent(inventory_panel_slot.transform, false);
            }

            if (i >= 10)
            {
                inventory_item_list.Add(new Item());
                inventory_slot_list.Add(Instantiate(inventory_slot));
                inventory_slot_list[i].name = "Slot#" + (i + 1);

                inventory_slot_list[i].transform.GetComponent<Data_Slot_Sack>().item_id = i;
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
                    Debug.Log(inventory_slot_list[i].transform.GetChild(0).GetComponent<Data_Item_Sack>().item.Title);
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
                if (inventory_slot_list[i].transform.childCount > 0 && inventory_slot_list[i].transform.GetChild(0).GetComponent<Data_Item_Sack>().item.ID != 0)
                {
                    Add_Item(inventory_slot_list[i].transform.GetChild(0).GetComponent<Data_Item_Sack>().item.ID, 0);
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

    public void AddReward(params Sack_Item[] sack_items)
    {
        float randomChance = 0;
        int index = 0;

        sack_items = sack_items.OrderBy(w => w.priority).ToArray();

        for (int i = 0; i < sack_items.Length; i++)
        {
            for (int j = 0; j < sack_items[i].maxCount; j++)
            {
                randomChance = Random.value * 100;

                if (randomChance < sack_items[i].chance && index < 5)
                {
                    Add_Item(sack_items[i].item_ID, 10);
                    index++;
                    //Debug.Log("random: " + randomChance + " / Chance_To_Get: " + monsters.getMonster(0).rewards[i].Chance_To_Get);
                }
            }
        }

    }

    private void Load_Reward()
    {
        for (int i = 10; i < save_system.get_Node().GetComponent<Node>().event_Info.inventory.Length; i++)
        {
            if (save_system.get_Node().GetComponent<Node>().event_Info.inventory[i] != 0)
                Add_Item(save_system.get_Node().GetComponent<Node>().event_Info.inventory[i], i);
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
                new_item.GetComponent<Data_Item_Sack>().item = candidateItem;
                new_item.GetComponent<Data_Item_Sack>().slot = i;
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
        gameObject.transform.FindChild("Panel_Items").FindChild("Gold_quantity").FindChild("Text_gold_quantity").GetComponent<Text>().text = save_system.get_sd_char().Gold_Quantity.ToString();
    }

    private int[] parse_inventory()
    {
        int[] inventory = new int[10];
        for (int a = 0; a < 10; a++)
        {
            //inventory[a] = inventory_item_list[a].ID;
            if (inventory_slot_list[a].transform.childCount > 0)
            {
                inventory[a] = inventory_slot_list[a].transform.GetChild(0).GetComponent<Data_Item_Sack>().item.ID;
                //Debug.Log(inventory_slot_list[a].transform.GetChild(0).GetComponent<Data_Item_Reward>().item.Title);
            }
        }

        return inventory;
    }

    private int[] parse_Reward()
    {
        int[] inventory = new int[15];
        bool empty = true;
        for (int i = 10; i < 15; i++)
        {
            if (inventory_slot_list[i].transform.childCount > 0)
                inventory[i] = inventory_slot_list[i].transform.GetChild(0).GetComponent<Data_Item_Sack>().item.ID;
            else
                inventory[i] = 0;

            if (inventory[i] != 0)
                empty = false;
        }
        if(empty)
        {
            save_system.get_Node().GetComponent<Node>().SetNodeEvent(NodeEvent.Empty_Path);
            return null;
        }
        else
        {
            return inventory;
        }
        
    }

    public void parse_information()
    {
        save_system.set_chosed_hero_inv(parse_inventory());
        save_system.get_Node().GetComponent<Node>().event_Info.Set_Info(parse_Reward());
    }
}
