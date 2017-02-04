using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Load_Game : MonoBehaviour {

    private Database_saves database_Saves;
    private GameObject continue_Button;

    private bool save_Exits = false;

    void Start()
    {
        database_Saves = FindObjectOfType<Database_saves>();
        continue_Button = GameObject.Find("Button_continue");

        if(database_Saves.get_character().Name == null)
        {
            //Debug.Log("Continue Disabled");
            save_Exits = false;
            continue_Button.GetComponent<Image>().color = new Color(1, 1, 1, 0.6f);
            continue_Button.transform.GetChild(0).GetComponent<Text>().color = new Color(1, 1, 1, 0.6f);

        }
        else
        {
            //Debug.Log("Continue activated");
            save_Exits = true;
            continue_Button.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
            continue_Button.transform.GetChild(0).GetComponent<Text>().color = new Color(1, 1, 1, 1);
        }
    }

    public void Continue()
    {
        if(save_Exits)
        {
            FindObjectOfType<SaveSystem>().set_Loaded_hero();
            FindObjectOfType<SceneSwitcher>().explore_Scene();
        }
    }

}
