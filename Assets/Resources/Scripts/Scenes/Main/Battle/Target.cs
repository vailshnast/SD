using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Target : MonoBehaviour {

    private SaveSystem save_System;
    private Setting setting;

    private Battle_Manager battle_Manager;
    private Monster_Info monsterInfo;
    private Hero_Info hero_Info;

    private Image imageComponent;

    private bool staticRerandoming;
    private bool onClickRerandoming;

    private TargetState currentState = TargetState.Static;

    public delegate int Target_Delegate();
    public Target_Delegate damage_State;

    // Use this for initialization
    void Start () {
        battle_Manager = FindObjectOfType<Battle_Manager>();
        save_System = FindObjectOfType<SaveSystem>();
        setting = save_System.get_Setting();
        monsterInfo = FindObjectOfType<Monster_Info>();
        hero_Info = FindObjectOfType<Hero_Info>();
        imageComponent = GetComponent<Image>();

        damage_State = Default;
    }

    private IEnumerator OnClickRerandom()
    {
        
        onClickRerandoming = true;

        imageComponent.enabled = false;
        yield return new WaitForSeconds(Random.Range(ToFloat(setting.OnClick_Time_To_Appear.Min), ToFloat(setting.OnClick_Time_To_Appear.Max)));

        imageComponent.enabled = true;

        battle_Manager.RandomPos(gameObject);

        onClickRerandoming = false;
        SwitchToStaticState();
        
        
    }
    private IEnumerator StaticRerandom()
    {
        staticRerandoming = true;

        yield return new WaitForSeconds(Random.Range(ToFloat(setting.Static_Time_To_Appear.Min), ToFloat(setting.Static_Time_To_Appear.Max)));

        imageComponent.enabled = true;      

        yield return new WaitForSeconds(Random.Range(ToFloat(setting.Static_Time_To_Dissappear.Min), ToFloat(setting.Static_Time_To_Dissappear.Max)));
        
        imageComponent.enabled = false;
        battle_Manager.RandomPos(gameObject);
        staticRerandoming = false;
    }

    private void targetControl()
    {
        switch (currentState)
        {
            case TargetState.Static:
                if (!staticRerandoming)
                    StartCoroutine(StaticRerandom());
                break;
            case TargetState.Clicked:
                if(!onClickRerandoming)
                StartCoroutine(OnClickRerandom());
                break;
            case TargetState.Inactive:
                imageComponent.enabled = false;
                break;
            default:
                break;
        }
    }
    // Update is called once per frame
    void Update () {
        targetControl();
	}

    public void Default_Target_Click()
    {
        SwitchToClickedState();

        GameObject text;
        text = Instantiate(Resources.Load("Prefabs/Battle/Damage_Text"), new Vector2(0, 0), Quaternion.identity, GameObject.Find("Instantiated_Objects").transform) as GameObject;

        int damage = damage_State();

        if (damage < 3)
            damage = 3;

        text.GetComponent<Text>().text = damage.ToString();

        //Shake enemy sprite
        monsterInfo.ShakeEnemySprite(0.2f, new Vector2(25, 25));

        text.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-81, 67), Random.Range(135, 250));

        monsterInfo.DealDamage(damage);
        battle_Manager.AddFinalBlowCharge(); 
    }

    public int Default()
    {
        int damage = hero_Info.GetHero().Damage.value - monsterInfo.Defence;
        return damage;
    }

    public void SwitchToClickedState()
    {
        onClickRerandoming = false;   
        StopAllCoroutines();
        currentState = TargetState.Clicked;
    }
    public void SwitchToStaticState()
    {
        staticRerandoming = false;
        StopCoroutine(OnClickRerandom());

        //if (currentState == TargetState.Inactive)
          //  imageComponent.enabled = true;

        currentState = TargetState.Static;
    }
    public void SwitchToInactiveState()
    {
        StopAllCoroutines();            
        currentState = TargetState.Inactive;
    }
    
    public TargetState GetCurrentState()
    {
        return currentState;
    }

    private float ToFloat(double value)
    {
        return System.Convert.ToSingle(value);
    }
}
