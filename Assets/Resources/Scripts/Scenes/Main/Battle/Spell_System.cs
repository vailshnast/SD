using UnityEngine;
using System.Collections;

public class Spell_System : MonoBehaviour {

    private Battle_Manager battle_Manager;
    private Hero_Info hero_Info;
    private Monster_Info monster_Info;
    private SaveSystem save_System;
    private Skill_Slot[] slots;

    void Start()
    {
        battle_Manager = FindObjectOfType<Battle_Manager>();
        save_System = FindObjectOfType<SaveSystem>();
        monster_Info = FindObjectOfType<Monster_Info>();
        hero_Info = FindObjectOfType<Hero_Info>();
        slots = FindObjectsOfType<Skill_Slot>();
    }

    public void CastSpell(string spellName)
    {
        StartCoroutine(CastSpellCoroutine(spellName));
    }

   

    public IEnumerator CastSpellCoroutine(string spellName)
    {
        Skill_Slot slot = GetSlot(spellName);

        switch (spellName)
        {
            case "Mana Splash":
                if(battle_Manager.current_State != Battle_Manager.Battle_State.Results && battle_Manager.current_State != Battle_Manager.Battle_State.Enemy_Death
                    && save_System.get_sd_char().Mana_Current.value >= 30)
                {
                    int damage = save_System.get_sd_char().Magic.value / 100 * 50;
                    battle_Manager.AddInfoToList(new info_Text("Mana Splash " + damage, Color.cyan));
                    battle_Manager.staticMonster = true;
                    hero_Info.AdjustMana(30);
                    monster_Info.DealDamage(damage);
                    monster_Info.Adjust_Dodge(true);
                    slot.TogglePanel(true);
                    battle_Manager.ChangeState(Battle_Manager.Battle_State.Results);

                    //Useful method, to stop coroutine, until something is done
                    yield return new WaitUntil(() => battle_Manager.current_State == Battle_Manager.Battle_State.Prepare || battle_Manager.current_State == Battle_Manager.Battle_State.Enemy_Death);
                    slot.TogglePanel(false);
                }
                
                break;
            case "Discharge":
                if (battle_Manager.current_State != Battle_Manager.Battle_State.Results && battle_Manager.current_State != Battle_Manager.Battle_State.Enemy_Death
                    && save_System.get_sd_char().Mana_Current.value != 0)
                {
                    int damage = save_System.get_sd_char().Mana_Current.value * 3 * save_System.get_sd_char().Magic.value / 100;
                    int manaSpent = save_System.get_sd_char().Mana_Current.value;

                    battle_Manager.AddInfoToList(new info_Text("Discharge " + damage, Color.cyan));                                
                    monster_Info.DealDamage(damage);
                    hero_Info.AdjustMana(save_System.get_sd_char().Mana_Current.value);                                                       
                    slot.TogglePanel(true);

                    if(manaSpent >= 50)
                    {
                        monster_Info.Adjust_Dodge(true);
                        battle_Manager.staticMonster = true;
                        battle_Manager.ChangeState(Battle_Manager.Battle_State.Results);
                    }
                    

                    //Useful method, to stop coroutine, until something is done
                    yield return new WaitUntil(() => battle_Manager.current_State == Battle_Manager.Battle_State.Prepare || battle_Manager.current_State == Battle_Manager.Battle_State.Enemy_Death);
                    slot.TogglePanel(false);
                }

                break;

                case "Mana Leak":

                if (battle_Manager.current_State != Battle_Manager.Battle_State.Results && battle_Manager.current_State != Battle_Manager.Battle_State.Enemy_Death
                    && save_System.get_sd_char().Mana_Current.value >= 10)
                {
                    //if Spell isnt currently used
                    if (battle_Manager.GetSpell("Mana Leak") == null)
                    {
                        battle_Manager.AddInfoToList(new info_Text("Mana Leak Activated", Color.yellow));

                        hero_Info.AdjustMana(10);

                        slot.TogglePanel(true);

                        battle_Manager.AddSpellToList(new duration_spell("Mana Leak",2,slot));

                        yield return new WaitUntil(() => battle_Manager.GetSpell("Mana Leak").Current_Duration == battle_Manager.GetSpell("Mana Leak").Max_Duration || battle_Manager.current_State == Battle_Manager.Battle_State.Enemy_Death);

                        battle_Manager.Set_Targets_Damage();
                        battle_Manager.finalblow_State = battle_Manager.Default;
                        slot.TogglePanel(false);
                        slot.ToggleSkillFrame(false);
                        slot.GetDuration().text = "";
                        battle_Manager.RemoveSpellFromList("Mana Leak");
                        
                        Debug.Log("Spell has ended");
                    }
                    //if its currenlty used
                    else if (battle_Manager.GetSpell("Mana Leak").Current_Duration > 0)
                    {
                        //Reset duration of a spell
                        battle_Manager.GetSpell("Mana Leak").Current_Duration = 0;

                        battle_Manager.AddInfoToList(new info_Text("Mana Leak Refreshed", Color.yellow));

                        hero_Info.AdjustMana(10);
                    }
                }

                break;
            default:
                break;
        }
    }


    private Skill_Slot GetSlot(string name)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].getSkill().Name == name)
                return slots[i];
        }
        return null;
    }


    public void ClearSpells()
    {
        for (int i = 0; i < battle_Manager.info_Duration_List.Count; i++)
        {
            battle_Manager.info_Duration_List[i].Slot.TogglePanel(false);
            battle_Manager.info_Duration_List[i].Slot.GetDuration().text = "";
            battle_Manager.info_Duration_List[i].Slot.ToggleSkillFrame(false);
        }
        battle_Manager.info_Duration_List.Clear();
        battle_Manager.finalblow_State = battle_Manager.Default;
        battle_Manager.Set_Targets_Damage();
    }

    //Methods that affect targets,finalblow
    public int ManaLeak_Target_Damage()
    {
        int damage = hero_Info.GetHero().Damage.value - monster_Info.Defence;

        hero_Info.AdjustMana(-1);

        hero_Info.SetTextParameters();
        return damage;
    }
    public int ManaLeak_FinalBlow()
    {
        battle_Manager.AddInfoToList(new info_Text("+10 mana", Color.blue));
        int damage = hero_Info.GetHero().Damage.value * hero_Info.GetHero().Magic.value / 100;

        hero_Info.AdjustMana(-10);       

        hero_Info.SetTextParameters();
        return damage;
    }
}
