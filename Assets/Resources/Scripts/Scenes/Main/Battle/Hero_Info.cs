using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Hero_Info : MonoBehaviour {

    private Database_Heroes database_Heroes;

    private SaveSystem save_system;

    private Monster_Info monsterInfo;

    private Spell_System spell_System;

    //If we dont check this in OnEnable, will cause different bugs,
    //because, OnEnable is called before Start().
    private bool initialised = false;

    // Use this for initialization
    void Start () {
        Initialise();   
    }
    
    void Initialise()
    {
        save_system = FindObjectOfType<SaveSystem>();
        monsterInfo = GetComponent<Monster_Info>();
        spell_System = FindObjectOfType<Spell_System>();
        monsterInfo.SetMonsterParameters(save_system.get_Node().GetComponent<Node>().GetNodeMonster());
        loadSkills();
        SetTextParameters();

        initialised = true;
    }
    void OnEnable()
    {
        if(initialised)
        {
            monsterInfo.SetMonsterParameters(save_system.get_Node().GetComponent<Node>().GetNodeMonster());
            SetTextParameters();
        }
    }
    private void loadSkills()
    {
        transform.getChild(gameObject, "Skill_1").GetComponent<Skill_Slot>().SetSkill(save_system.get_Default_Hero().Hero_Skills[0]);
        transform.getChild(gameObject, "Skill_2").GetComponent<Skill_Slot>().SetSkill(save_system.get_Default_Hero().Hero_Skills[1]);
        transform.getChild(gameObject, "Skill_3").GetComponent<Skill_Slot>().SetSkill(save_system.get_Default_Hero().Hero_Skills[2]);
    }
    public void DealDamageToHero(int damage)
    {
        if (damage < 3)
            damage = 3;

        save_system.get_sd_char().Health_Current.value -= damage;

        SetTextParameters();
        Death();      
    }
    public void AdjustMana(int mana)
    {
        if (save_system.get_sd_char().Mana_Current.value - mana < save_system.get_sd_char().Mana.value)
            save_system.get_sd_char().Mana_Current.value -= mana;
        else save_system.get_sd_char().Mana_Current = save_system.get_sd_char().Mana;
        

        SetTextParameters();
    }
    public void Death()
    {
        if (save_system.get_sd_char().Health_Current.value <= 0)
        {
            save_system.CreateNewSaveFile();
            SceneManager.LoadScene(0);
        }
    }
    public SD_Character GetHero()
    {
        return save_system.get_sd_char();
    }
    public void SetTextParameters()
    {
        GameObject.Find("EnemyName_Text").GetComponent<Text>().text = monsterInfo.MonsterName;

        GameObject.Find("Dmg_Amount").GetComponent<Text>().text = monsterInfo.Damage.ToString();

        GameObject.Find("Def_Amount").GetComponent<Text>().text = monsterInfo.Defence.ToString();

        GameObject.Find("Dodge_Amount").GetComponent<Text>().text = monsterInfo.Dodge.ToString();

        GameObject.Find("Health_Amount").GetComponent<Text>().text = save_system.get_sd_char().Health_Current.value.ToString();
        
        GameObject.Find("Mana_Amount").GetComponent<Text>().text = save_system.get_sd_char().Mana_Current.value.ToString();

        GameObject.Find("Health_Filler").GetComponent<Image>().fillAmount = (float)save_system.get_sd_char().Health_Current.value / save_system.get_sd_char().Health.value;


        GameObject.Find("Mana_Filler").GetComponent<Image>().fillAmount = (float)save_system.get_sd_char().Mana_Current.value / save_system.get_sd_char().Mana.value;
    }
}
