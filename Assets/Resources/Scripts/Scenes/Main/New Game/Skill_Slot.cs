using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Skill_Slot : MonoBehaviour {

    private Spell_System spell_system;
    private Skill skill;
    private Image fade_Panel;
    private GameObject tooltip;
    private Text duration;

	// Use this for initialization
	void Awake () {
        tooltip = GameObject.Find("Tooltip_Skill");
        spell_system = FindObjectOfType<Spell_System>();          
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetSkill(Skill skill)
    {
        this.skill = skill;
        GetComponent<Image>().sprite = skill.GetSkillSprite();
    }
    public void CastSpell()
    {
        spell_system.CastSpell(skill.Name);
    }
    public void ToggleSkillFrame(bool active)
    {
        if(active)
            transform.parent.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/Battle/YellowSlot");
        else
            transform.parent.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/Battle/BasicSlot");

    }
    public void TogglePanel(bool active)
    {
        transform.GetChild(0).GetComponent<Image>().enabled = active;
    }
    public void EnableTooltip()
    {
        tooltip.SetActive(true);
        tooltip.GetComponent<Skill_Tooltip>().EnableTooltip(transform);
    }
    public Text GetDuration()
    {
        return transform.GetChild(1).GetComponent<Text>();
    }
    public Skill getSkill()
    {
        return skill;
    }
}
