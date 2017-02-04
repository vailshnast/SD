using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hero_chose : MonoBehaviour {

    Database_Heroes database_Heroes;
    
    
    int chosed_hero = 0;

    void Start () {
        database_Heroes = FindObjectOfType<Database_Heroes>();
        hero_Load(chosed_hero);
    }

    public void call_next_hero()
    {
        next_hero();
    }

    public void call_previous_hero()
    {
        previous_hero();
    }

    private void next_hero()
    {
        chosed_hero++;
        if (chosed_hero == database_Heroes.getHeroesCount())
            chosed_hero = 0;
        hero_Load(chosed_hero);
    }

    private void previous_hero()
    {
        if (chosed_hero - 1 < 0)
            chosed_hero = database_Heroes.getHeroesCount() - 1;
        else chosed_hero--;
        hero_Load(chosed_hero);
    }

    public void hero_Load(int id_hero)
    {
        Hero hero = database_Heroes.getHero(id_hero);
        load_hero_info(hero);
    }

    private void load_hero_info(Hero hero)
    {
        GameObject.Find("Hero_Image").GetComponent<Image>().sprite = hero.GetSprite();

        GameObject.Find("Class_Name").GetComponent<Text>().text = hero.Title;

        GameObject.Find("stat_health").GetComponent<Text>().text = ""+hero.Health.value;
        GameObject.Find("stat_mana").GetComponent<Text>().text = "" + hero.Mana.value;

        GameObject.Find("stat_def").GetComponent<Text>().text = "" + hero.Defence.value;
        GameObject.Find("stat_dmg").GetComponent<Text>().text = "" + hero.Damage.value;

        GameObject.Find("stat_pwr").GetComponent<Text>().text = "" + hero.Power.value;
        GameObject.Find("stat_mgc").GetComponent<Text>().text = "" + hero.Magic.value;

        GameObject.Find("Skill_1").GetComponent<Skill_Slot>().SetSkill(hero.Hero_Skills[0]);
        GameObject.Find("Skill_2").GetComponent<Skill_Slot>().SetSkill(hero.Hero_Skills[1]);
        GameObject.Find("Skill_3").GetComponent<Skill_Slot>().SetSkill(hero.Hero_Skills[2]);
        GameObject.Find("Skill_4").GetComponent<Skill_Slot>().SetSkill(hero.Hero_Skills[3]);
    }

    public void chose_hero()
    {
        SaveSystem ss = GameObject.Find("Save_System").GetComponent<SaveSystem>();
        ss.set_chosed_hero_id(chosed_hero);
    }


}
