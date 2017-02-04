using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Altar : MonoBehaviour
{
    public enum Bless_Type
    {
        Positive,
        Negative
    }
    public class Bless
    {
        public int positive;
        public int negative;

        private Stat stat;
        public Bless(int positive,int negative, Stat stat)
        {
            this.stat = stat;
            this.positive =  Mathf.Abs(positive);
            this.negative = -Mathf.Abs(negative);
        }

        public string Bless_Hero()
        {
            Bless_Type type = Random.value < .5 ? Bless_Type.Positive : Bless_Type.Negative;
            if(type == Bless_Type.Negative)
            {
                stat.value += negative;

                if (stat.name == "Health" && stat.value < SaveSystem.Instance.get_sd_char().Health_Current.value)
                    SaveSystem.Instance.get_sd_char().Health_Current.value = stat.value;

                if (stat.name == "Mana" && stat.value < SaveSystem.Instance.get_sd_char().Mana_Current.value)
                    SaveSystem.Instance.get_sd_char().Mana_Current.value = stat.value;


                return "<color=red>Gods cursed you</color>\n" + negative + " maximum " + stat.name;
            }
            else
            {
                stat.value += positive;
                return "<color=#C48C28FF>Gods blessed you</color>\n+" + positive + " maximum " + stat.name;
            }               

        }
    }

    private List<Bless> blesses = new List<Bless>();

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

        text_Label.text = "You found an Altar\nPrey to your gods?";

        GetButton("Leave").SetActive(false);

        Init_Blesses();
        
        intialised = true;
    }

    private void OnEnable()
    {
        Refresh();
    }

    private SD_Character Get_Hero()
    {
        return SaveSystem.Instance.get_sd_char();
    }

    private void Init_Blesses()
    {
        blesses.AddRange(new List<Bless> {
            new Bless(10, 5, Get_Hero().Health),
            new Bless(5, 5, Get_Hero().Mana),
            new Bless(2, 1, Get_Hero().Defence),
            new Bless(3, 2, Get_Hero().Damage),
            new Bless(25, 25, Get_Hero().Power),
            new Bless(25, 25, Get_Hero().Magic) });
    }

    private Bless GetRandomBless()
    {
        return blesses[Random.Range(0, blesses.Count)];
    }

    private void Refresh()
    {
        if (intialised)
        {
            text_Label.text = "You found an Altar\nPrey to your gods?";
            GetButton("No").SetActive(true);
            GetButton("Yes").SetActive(true);
            GetButton("Leave").SetActive(false);
        }
    }

    public void Pressed_YES()
    {
        GetBless();

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

    public void GetBless()
    {
        text_Label.text = GetRandomBless().Bless_Hero();
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
