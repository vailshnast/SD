using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Skill_Tooltip : MonoBehaviour {

    private GameObject currentSlot;
    private GameObject fade_Panel;
    private GameObject parameters;
    private Skill skill;
	// Use this for initialization
	void Start () {
        fade_Panel = GameObject.Find("Fade_Panel");
        parameters = transform.getChild(gameObject, "Parameters");
        fade_Panel.SetActive(false);
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void EnableTooltip(Transform slot)
    {
        //Change slot sprite to basic if it exists
        if(currentSlot)
        {
            currentSlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/Reward/Basicslot");
            currentSlot.transform.GetChild(0).GetComponent<Image>().sprite = skill.GetSkillSprite();
        }

        //initialise skill to have easy access
        //currentSlot to disable when we need it    
        currentSlot = slot.parent.gameObject;
        skill = slot.GetComponent<Skill_Slot>().getSkill();

        //change skill sprite to symbol sprite
        slot.GetComponent<Image>().sprite = skill.GetSymbolSprite();

        //Activate fadePanel
        fade_Panel.SetActive(true);

        //Change sprite on slot
        currentSlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/Reward/Yellowslot");

        //Change text.
        transform.getChild(gameObject, "Tooltip_Name").GetComponent<Text>().text = skill.Name;
        transform.getChild(gameObject, "Tooltip_Description").GetComponent<Text>().text = skill.Description;



        //Disable parameteters object if it length < 0,
        //if we dont do that tooltip will have a big gap
        //on the bottom
        if (skill.Parameters.Length == 0)
            parameters.SetActive(false);
        else
        {
            //Destory all parameters
            parameters.SetActive(true);
            transform.DestroyChildren(transform.getChild(gameObject, "Parameters"));            
        } 
              
        //Make parameters we need
        for (int i = 0; i < skill.Parameters.Length; i++)
        {
            GameObject param = Instantiate(Resources.Load("Prefabs/New Game/Param"), transform.getChild(gameObject, "Parameters").transform, false) as GameObject;
            param.GetComponent<Text>().text = "<color=#C48C28FF>" + skill.Parameters[i].Name + "</color>" + skill.Parameters[i].Value;
        }
    }
   
    public void DisableTooltip()
    {
        //Change slot sprite to basic if it exists
        if (currentSlot)
        { 
            currentSlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/Reward/Basicslot");
            currentSlot.transform.GetChild(0).GetComponent<Image>().sprite = skill.GetSkillSprite();
        }
            


        //Disable fadePanel,tooltip
        fade_Panel.SetActive(false);
        gameObject.SetActive(false);
    }

}
