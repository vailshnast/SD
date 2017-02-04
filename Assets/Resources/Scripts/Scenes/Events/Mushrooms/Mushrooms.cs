using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mushrooms : MonoBehaviour
{
    private Text text_Label;
    private GameObject[] buttons;

    private Event_Manager event_Manager;

    private bool intialised;
    void Start()
    {
        Init();

    }

    void Init()
    {
        text_Label = GameObject.Find("Text_Label").GetComponent<Text>();
        buttons = GameObject.FindGameObjectsWithTag("Button");

        text_Label.text = "You found mushrooms\nEat?";

        GetButton("Leave").SetActive(false);

        event_Manager = new Event_Manager(text_Label,new Event(30, Health_Restore), new Event(15, Health_Reduce), new Event(55, Nothing));

        intialised = true;
    }


    public void Pressed_YES()
    {
        event_Manager.Create_Event();

        GetButton("No").SetActive(false);
        GetButton("Yes").SetActive(false);
        GetButton("Leave").SetActive(true);
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

    private GameObject GetButton(string name)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].name == name)
                return buttons[i];
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

    private void Refresh()
    {
        if (intialised)
        {
            text_Label.text = "You found mushrooms\nEat?";

            GetButton("No").SetActive(true);
            GetButton("Yes").SetActive(true);
            GetButton("Leave").SetActive(false);
        }
    }
    //Event methods
    private string Health_Reduce()
    {
        Get_Hero().Health.value -= 10;

        if (Get_Hero().Health_Current.value > Get_Hero().Health.value)
            Get_Hero().Health_Current.value = Get_Hero().Health.value;

        return "<color=red>Your were poisoned</color>\n-10 maximum Health";
    }
    private string Health_Restore()
    {
        int max_Health_Restored = (Get_Hero().Health.value / 100) * 50;

        if (max_Health_Restored + Get_Hero().Health_Current.value > Get_Hero().Health.value)
            max_Health_Restored = Get_Hero().Health.value - Get_Hero().Health_Current.value;

        Get_Hero().AddPotionStats(max_Health_Restored,0);

        return "You restored:\n" + "<color=red>" + max_Health_Restored + " Health</color>";
    }
    private string Nothing()
    {
        return "Nothing Happened";
    }
}


