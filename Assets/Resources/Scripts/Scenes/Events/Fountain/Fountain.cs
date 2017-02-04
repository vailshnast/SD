using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fountain : MonoBehaviour {

    private Text text_Label;
    private GameObject[] buttons;

    private bool intialised;
    void Start()
    {
        Init();
    }

    void Init()
    {
        text_Label = GameObject.Find("Text_Label").GetComponent<Text>();
        buttons = GameObject.FindGameObjectsWithTag("Button");

        text_Label.text = "You found fountain\nDrink?";

        GetButton("Leave").SetActive(false);
        intialised = true;
    }

    private void OnEnable()
    {
        Refresh();
    }

    private void Refresh()
    {
        if(intialised)
        {
            text_Label.text = "You found fountain\nDrink?";
            GetButton("No").SetActive(true);
            GetButton("Yes").SetActive(true);
            GetButton("Leave").SetActive(false);
        }
    }

    public void Pressed_YES()
    {
        Restore(20);

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

    public void Restore(int procent)
    {
        int max_Health_Restored = (SaveSystem.Instance.get_sd_char().Health.value / 100) * procent;
        int max_Mana_Restored = (SaveSystem.Instance.get_sd_char().Mana.value / 100) * procent;
       
        

        if (max_Health_Restored + SaveSystem.Instance.get_sd_char().Health_Current.value > SaveSystem.Instance.get_sd_char().Health.value)
            max_Health_Restored = SaveSystem.Instance.get_sd_char().Health.value - SaveSystem.Instance.get_sd_char().Health_Current.value;

        if (max_Mana_Restored + SaveSystem.Instance.get_sd_char().Mana_Current.value > SaveSystem.Instance.get_sd_char().Mana.value)
            max_Mana_Restored = SaveSystem.Instance.get_sd_char().Mana.value - SaveSystem.Instance.get_sd_char().Mana_Current.value;

        SaveSystem.Instance.get_sd_char().AddPotionStats(max_Health_Restored, max_Mana_Restored);

        text_Label.text = "You restored:\n<size=55><color=red>" + max_Health_Restored + " Health</color> And <color=blue>" + max_Mana_Restored + " Mana</color></size>";     
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
