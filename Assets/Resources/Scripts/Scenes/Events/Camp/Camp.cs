using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camp : MonoBehaviour
{
    private Text text_Label;
    private GameObject[] sprites;

    private List<Event> events = new List<Event>();
    private Event_Manager event_Manager;

    private bool intialised;
    void Start()
    {
        Init();

    }

    void Init()
    {
        text_Label = GameObject.Find("Text_Label").GetComponent<Text>();
        sprites = GameObject.FindGameObjectsWithTag("Sprite");
        
        text_Label.text = "You found Camp\nExplore?";

        GetSprite("Leave").SetActive(false);
        GetSprite("Fight").SetActive(false);
        GetSprite("Monster").SetActive(false);

        event_Manager = new Event_Manager(text_Label, new Event(30, Found_Items), new Event(15, Ambush), new Event(55, Nothing));

        intialised = true;
    }

    private void Refresh()
    {
        if (intialised)
        {
            text_Label.text = "You found Camp\nExplore?";
            transform.FindDeepChild("Inventory").gameObject.SetActive(false);
            GetSprite("Camp").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/Explore/CampObject");

            GetSprite("Text_Panel").SetActive(true);
            GetSprite("No").SetActive(true);
            GetSprite("Yes").SetActive(true);
            GetSprite("Camp").SetActive(true);
            GetSprite("Leave").SetActive(false);
            GetSprite("Fight").SetActive(false);
            GetSprite("Monster").SetActive(false);
        }
    }
    private GameObject GetSprite(string name)
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].name == name)
                return sprites[i];
        }
        return null;
    }
    private void OnEnable()
    {       
        Refresh();
    }
    private SD_Character Get_Hero()
    {
        return SaveSystem.Instance.get_sd_char();
    }
    public void Pressed_YES()
    {
        GetSprite("No").SetActive(false);
        GetSprite("Yes").SetActive(false);
        event_Manager.Create_Event();
    }

    public void Pressed_Fight()
    {
        SaveSystem.Instance.get_Node().GetComponent<Node>().SetNodeEvent(NodeEvent.Empty_Path);
        SceneSwitcher.Instance.battle_Scene();
    }

    public void Pressed_No()
    {
        SceneSwitcher.Instance.explore_Scene();
    }

    public void Pressed_Leave()
    {
        SaveSystem.Instance.get_Node().GetComponent<Node>().SetNodeEvent(NodeEvent.Empty_Path);
        SceneSwitcher.Instance.explore_Scene();
    }


    //Event methods
    public string Nothing()
    {
        GetSprite("Leave").SetActive(true);
        return "You found Nothing";
    }
    public string Found_Items()
    {
        transform.FindDeepChild("Inventory").gameObject.SetActive(true);
        GetSprite("Text_Panel").SetActive(false);
        GetSprite("Camp").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/Explore/BrokenCamp");
        return "You found something";
    }
    public string Ambush()
    {
        GetSprite("Monster").SetActive(true);
        GetSprite("Fight").SetActive(true);
        GetSprite("Camp").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/Explore/BrokenCamp");
        return "AMBUSH";
    }
}