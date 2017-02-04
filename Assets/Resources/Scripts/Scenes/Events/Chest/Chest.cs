using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{

    private GameObject text_Panel;
    private GameObject[] buttons;

    private bool intialised;

    private List<GameObject> slots = new List<GameObject>();
    private Chest_Slot current_Slot
    {
        get
        {         
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].GetComponent<Chest_Slot>().slot_ID == 0)
                    return slots[i].GetComponent<Chest_Slot>();
            }

            return null;
        }
    }

    public int lockPick_Tries;
    private int lockPick_Tries_Max;

    public float distance = 15;
    public float size = 30;
    public float time_To_Move = 0.75f;

    public int slots_Amount { get; private set; }
    private int[] possible_Slots_Amounts = new int[] { 5, 9, 12 };

    private bool scrolling = false;
    private bool lockPicking = false;
    void Start()
    {
        Init();
    }

    void Init()
    {
        text_Panel = GameObject.Find("Text_Panel");
        buttons = GameObject.FindGameObjectsWithTag("Button");
        lockPick_Tries_Max = lockPick_Tries;
        text_Panel.transform.GetChild(0).GetComponent<Text>().text = "You found a chest";

        GetButton("LeftTap").SetActive(false);
        GetButton("RightTap").SetActive(false);

        LockPick_Exists();

        slots_Amount = possible_Slots_Amounts[Random.Range(0, possible_Slots_Amounts.Length)];

        intialised = true;
    }
    private void OnEnable()
    {
        Refresh();
    }
    private void Refresh()
    {
        if (intialised)
        {
            LockPick_Exists();
            GetButton("Chest").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/Explore/ChestObject");
            text_Panel.gameObject.SetActive(true);          
            text_Panel.transform.GetChild(0).GetComponent<Text>().text = "You found a chest";
            transform.FindDeepChild("Inventory").gameObject.SetActive(false);
            GetButton("Leave").SetActive(true);
            GetButton("Lockpick").SetActive(true);
            GetButton("Lockpick").GetComponent<RectTransform>().anchoredPosition = new Vector2(-160, 0);
            GetButton("Lockpick").transform.FindDeepChild("Lockpick_Tries").GetComponent<Text>().text = "";
            GetButton("LeftTap").SetActive(false);
            GetButton("RightTap").SetActive(false);
            lockPicking = false;
            lockPick_Tries = lockPick_Tries_Max;
            scrolling = false;
            slots_Amount = possible_Slots_Amounts[Random.Range(0, possible_Slots_Amounts.Length)];
        }
    }

    private bool LockPick_Exists()
    {
        if (SaveSystem.Instance.Check_Item_Existance(12))
            GetButton("Lockpick").transform.FindDeepChild("Panel").gameObject.SetActive(false);
        else
            GetButton("Lockpick").transform.FindDeepChild("Panel").gameObject.SetActive(true);
        return SaveSystem.Instance.Check_Item_Existance(12);
    }

    private void Create_Slots(int slots_Amount)
    {
        slots.Clear();

        if (slots_Amount < 2)
            slots_Amount = 2;

        for (int i = 0; i < slots_Amount; i++)
        {
            slots.Add(Instantiate(Resources.Load("Prefabs/Chest/Slot"), GameObject.Find("Slots").transform, false) as GameObject);
        }
        Init_Slots();
    }

    private void Init_Slots()
    {        
        for (int i = 0; i < slots.Count; i++)
        {
            RectTransform slot_Rect = slots[i].GetComponent<RectTransform>();
            RectTransform previous_Slot_Rect = i > 0 ? slots[i-1].GetComponent<RectTransform>() : null;
            slots[i].GetComponent<Chest_Slot>().slot_ID = i;

            float local_Size = i * size < size*2 ? i * size : size*2;
            //size
            slot_Rect.sizeDelta = new Vector2(slot_Rect.sizeDelta.x - local_Size, slot_Rect.sizeDelta.x - local_Size);
            slot_Rect.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(slot_Rect.sizeDelta.x - 22, slot_Rect.sizeDelta.y - 22);
            //position
            slot_Rect.anchoredPosition = i == 0 ? new Vector2(0, 391) : new Vector2(previous_Slot_Rect.anchoredPosition.x + previous_Slot_Rect.sizeDelta.x / 2 + slot_Rect.sizeDelta.x / 2 + distance, 391);
            //color
            slot_Rect.GetComponent<Image>().color = i == 0 ? slot_Rect.GetComponent<Image>().color : this.HexToColor("787878FF");
            slot_Rect.GetChild(0).GetComponent<Image>().color = slot_Rect.GetComponent<Image>().color;
            //set direction
            slots[i].GetComponent<Chest_Slot>().SetDirection();
        }
    }

    private void Scroll_Slots()
    {
        //If we use scrolling method,
        //when its already running,
        //different bugs will appear
        if (!scrolling)
        {
            scrolling = true;
            //Set tempWidth and ID
            for (int i = 0; i < slots.Count; i++)
            {
                Chest_Slot chest_Slot = slots[i].GetComponent<Chest_Slot>();
                RectTransform slot_Rect = slots[i].GetComponent<RectTransform>();
                chest_Slot.slot_ID--;

                if (chest_Slot.slot_ID == 0 || chest_Slot.slot_ID == 1)
                    chest_Slot.temp_Width = slot_Rect.sizeDelta.x + size;
                else if (chest_Slot.slot_ID < 0 && chest_Slot.slot_ID > -3)
                    chest_Slot.temp_Width = slot_Rect.sizeDelta.x - size;
                else chest_Slot.temp_Width = slot_Rect.sizeDelta.x;
            }
            for (int i = 0; i < slots.Count; i++)
            {
                RectTransform slot_Rect = slots[i].GetComponent<RectTransform>();
                RectTransform previous_Slot_Rect = i > 0 ? slots[i - 1].GetComponent<RectTransform>() : slots[i + 1].GetComponent<RectTransform>();

                Chest_Slot current_Slot = slots[i].GetComponent<Chest_Slot>();
                Chest_Slot previous_Slot = i > 0 ? slots[i - 1].GetComponent<Chest_Slot>() : slots[i + 1].GetComponent<Chest_Slot>();
                Color color = current_Slot.slot_ID == 0 ? Color.white : this.HexToColor("787878FF");

                if (i == 0)
                    StartCoroutine(Scroll_Slot(slot_Rect, new Vector2(slot_Rect.anchoredPosition.x - previous_Slot.temp_Width / 2 - current_Slot.temp_Width / 2 - distance, 391), current_Slot.temp_Width, time_To_Move, color));
                else
                    StartCoroutine(Scroll_Slot(slot_Rect, new Vector2(previous_Slot.previousPos.x, 391), current_Slot.temp_Width, time_To_Move, color));

            }
        }       
    }

    private IEnumerator Lerp_Width(float time,RectTransform rect,float width)
    {
        Vector2 originalScale = rect.sizeDelta;
        Vector2 targetScale = new Vector2(width, width);
        float originalTime = time;


        while (time > 0.0f)
        {           
            time -= Time.deltaTime;
            rect.sizeDelta = Vector3.Lerp(targetScale, originalScale, time / originalTime);
            rect.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(rect.sizeDelta.x - 22, rect.sizeDelta.y - 22);           
            yield return null;

        }
    }

    private IEnumerator Scroll_Slot(RectTransform rect, Vector2 position,float width, float timeToMove , Color target_Color)
    {
        StartCoroutine(Lerp_Width(timeToMove, rect, width));

        rect.GetComponent<Chest_Slot>().previousPos = rect.anchoredPosition;
        Vector2 currentPos = rect.anchoredPosition;
        Color original_Color = rect.GetComponent<Image>().color;

        float t = 0f;

        while (t <= 1)
        {
            t += Time.deltaTime / timeToMove;
            rect.anchoredPosition = Vector2.Lerp(currentPos, position, t);
            rect.GetComponent<Image>().color = Color.Lerp(original_Color, target_Color, t);
            rect.GetChild(0).GetComponent<Image>().color = rect.GetComponent<Image>().color;
            yield return null;
        }
        scrolling = false;
    }

    public void Unlock(Arrow_Direction direction)
    {
        if (!scrolling && current_Slot && current_Slot.Unlocked(direction))
        {
            Scroll_Slots();
            Open_Chest();
        }         
    }


    public void Lockpick_Try_Failed()
    {
        lockPick_Tries--;
        GetButton("Lockpick").transform.FindDeepChild("Lockpick_Tries").GetComponent<Text>().text = lockPick_Tries != 0 ? lockPick_Tries.ToString() : "";

        if (lockPick_Tries <= 0)
            Lockpick_Expired();

        
    }

    private void Lockpick_Expired()
    {
        lockPicking = false;
        SaveSystem.Instance.Delete_Item(12);
        LockPick_Exists();
        lockPick_Tries = lockPick_Tries_Max;
        GetButton("Lockpick").GetComponent<RectTransform>().anchoredPosition = new Vector2(-160, 0);
        GetButton("LeftTap").SetActive(false);
        GetButton("RightTap").SetActive(false);
        GetButton("Leave").SetActive(true);
        text_Panel.SetActive(true);
        Clear();     
    }

    private void Clear()
    {
        StopAllCoroutines();
        slots.Clear();        
        GameObject.Find("Slots").transform.DestroyChildren();
    }

    private void Open_Chest()
    {
        if (slots[slots.Count - 1].GetComponent<Chest_Slot>().unlocked)           
        {
            transform.FindDeepChild("Inventory").gameObject.SetActive(true);
            GetButton("Chest").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/Explore/OpenChest");
            Clear();
        }      
    }

    public void Clear_Node()
    {
        SaveSystem.Instance.get_Node().GetComponent<Node>().SetNodeEvent(NodeEvent.Empty_Path);
    }

    public void Pressed_LeftTap()
    {
        Unlock(Arrow_Direction.Left);
    }

    public void Pressed_RightTap()
    {
        Unlock(Arrow_Direction.Right);
    }

    public void Pressed_Leave()
    {      
        SceneSwitcher.Instance.explore_Scene();
    }

    public void Pressed_Lockpick()
    {
        if (!lockPicking && LockPick_Exists())          
        {
            lockPicking = true;
            Create_Slots(slots_Amount);

            GetButton("LeftTap").SetActive(true);
            GetButton("RightTap").SetActive(true);
            GetButton("Leave").SetActive(false);
            text_Panel.SetActive(false);

            GetButton("Lockpick").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 43);
            GetButton("Lockpick").transform.FindDeepChild("Lockpick_Tries").GetComponent<Text>().text = lockPick_Tries.ToString();
            GetButton("Lockpick").transform.FindDeepChild("Panel").gameObject.SetActive(true);

            text_Panel.SetActive(false);
        }
    }

    private GameObject GetButton(string name)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].name == name)
                return buttons[i];
        }
        return null;

    }
}
