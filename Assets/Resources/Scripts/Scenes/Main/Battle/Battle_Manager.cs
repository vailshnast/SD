using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;


public class Battle_Manager : Touch_processor {

    #region Variables

    private SaveSystem save_System;
    private Hero_Info hero_Info;
    private Spell_System spell_System;
    private Monster_Info monsterInfo;

    private GameObject[] targets;

    private Image finalBlowFiller;   
    private Image finalBlowImage;
    private Image phaseFiller;
    private Image attackBar;

    private Text phaseTime;
    private Text victory_Button;

    private float finalBowCharge = 0;

    public enum Battle_State
    {
        Prepare,
        Swing,
        Results,
        Enemy_Death
    }
    public Battle_State current_State { get; private set; }
    public bool staticMonster { get; set; }

    private List<info_Text> info_List = new List<info_Text>();
    public List<duration_spell> info_Duration_List = new List<duration_spell>();

    public delegate int FinalBlowDelegate();
    public FinalBlowDelegate finalblow_State;
        
    private ImageDirection swipeDirection = ImageDirection.Null;
    private ImageDirection monsterDirection;

    //If we dont check this in OnEnable, will cause different bugs,
    //because, OnEnable is called before Start().
    private bool initialised = false;

    private bool infoRunning;
    #endregion

    // Use this for initialization
    void Start () {
        Initialise();
    }

    void Initialise()
    {
        save_System = FindObjectOfType<SaveSystem>();

        spell_System = FindObjectOfType<Spell_System>();

        monsterInfo = FindObjectOfType<Monster_Info>();

        hero_Info = FindObjectOfType<Hero_Info>();
        SpawnTargets(save_System.get_Setting().Target_Count);

        targets = GameObject.FindGameObjectsWithTag("Target");

        finalBlowFiller = GameObject.Find("Final_Blow_Filler").GetComponent<Image>();           
        phaseFiller = GameObject.Find("AttackBar_Filler").GetComponent<Image>();
        attackBar = GameObject.Find("AttackBar").GetComponent<Image>();
        finalBlowImage = GameObject.Find("FinalBlowTarget").GetComponent<Image>();
        phaseTime = GameObject.Find("Phase_Time").GetComponent<Text>();
        victory_Button = GameObject.Find("Victory").GetComponent<Text>();

        SetTargetsPositions();

        current_State = Battle_State.Prepare;

        minDistance = 25;

        finalblow_State = Default;

        initialised = true;
    }

  

    //Whenever we enable battle scene,
    //we should refresh our scene,
    //so we dont have parameters from
    //the last fight.
    void OnEnable()
    {
        if (initialised)
        {
            Refresh();
        }
    }
    private void Refresh()
    {

        staticMonster = false;

        monsterInfo.SetPrepareStateSprite();

        ResetFinalBlow();

        Delete_Instantiated_Objects();

        SetTargetsPositions();
                
        phaseFiller.fillAmount = 1;

        ActivateTargets();

        spell_System.ClearSpells();

        Set_Targets_Damage();

        finalblow_State = Default;

        ChangeState(Battle_State.Prepare);
    }
    // Update is called once per frame
    void Update() {
        BattlePhaseControl();
    }

    private void BattlePhaseControl()
    {
        //All_Phases
        finalBlowFiller.fillAmount = finalBowCharge / monsterInfo.Dodge;

        switch (current_State)
        {
            case Battle_State.Prepare:
                phaseFiller.fillAmount -= 1f / System.Convert.ToSingle(monsterInfo.PrepareTime) * Time.deltaTime;
                phaseTime.text = Get_Time(phaseFiller.fillAmount * System.Convert.ToSingle(monsterInfo.PrepareTime));

                if (finalBlowFiller.fillAmount == 1)
                {
                    finalBlowImage.enabled = true;
                    DisableTargets();
                }

                if (phaseFiller.fillAmount <= 0)
                    ChangeState(Battle_State.Swing);

                //dead to targets
                if (monsterInfo.Health <= 0)
                {
                    staticMonster = true;
                    ChangeState(Battle_State.Results);
                }
                break;
            case Battle_State.Swing:
                phaseFiller.fillAmount -= 1.0f / System.Convert.ToSingle(monsterInfo.SwingTime) * Time.deltaTime;
                phaseTime.text = Get_Time(phaseFiller.fillAmount * System.Convert.ToSingle(monsterInfo.SwingTime));
                if (monsterDirection == swipeDirection || phaseFiller.fillAmount <= 0)
                {
                    //deal damage to player
                    hero_Info.DealDamageToHero(monsterInfo.Damage - hero_Info.GetHero().Defence.value);
                    info_List.Add(new info_Text("-"+ (monsterInfo.Damage - hero_Info.GetHero().Defence.value) + " Health",Color.red));
                    ChangeState(Battle_State.Results);                    
                }
                else if(swipeDirection != ImageDirection.Null)
                {
                    info_List.Add(new info_Text("Dodged",Color.green));
                    monsterInfo.Adjust_Dodge(false);
                    ChangeState(Battle_State.Results);
                }
                break;
            case Battle_State.Results:
                
                if (!staticMonster)
                {
                    switch (monsterDirection)
                    {
                        case ImageDirection.Left:
                            monsterInfo.MoveSprite(-20);
                            break;
                        case ImageDirection.Right:
                            monsterInfo.MoveSprite(20);
                            break;
                        default:
                            break;
                    }

                }
                else
                {
                    monsterInfo.SetPrepareStateSprite();
                }
                if (!infoRunning)
                {
                    if (monsterInfo.Health <= 0)
                    {
                        ChangeState(Battle_State.Enemy_Death);
                    }
                    else
                    {
                        ChangeState(Battle_State.Prepare);
                    }
                }
                break;
            default:
                break;
        }
        
    }
    IEnumerator Show_Info()
    {
        infoRunning = true;

        Proccess_Hero_Passive();

        for (int i = 0; i < info_List.Count; i++)
        {
            GameObject text = Instantiate(Resources.Load("Prefabs/Battle/Damage_Text"), new Vector2(0, 0), Quaternion.identity, GameObject.Find("Instantiated_Objects").transform) as GameObject;
            text.GetComponent<Text>().text = info_List[i].Info;
            text.GetComponent<Text>().color = info_List[i].Color;
            text.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 200);
            yield return new WaitForSeconds(0.4f);
        }

        if (info_List.Count > 0)
            yield return new WaitForSeconds(1);
        else
            yield return new WaitForSeconds(1);

        info_List.Clear();

        infoRunning = false;
         
    }

    public void ChangeState(Battle_State state)
    {
        if (current_State == state)
        {
            Debug.Log("same state");
            return;
        }

        ExitState(current_State);

        EnterState(state);

        current_State = state;
    }
    private void ExitState(Battle_State state)
    {
        Debug.Log("exit" + state);
        switch (state)
        {
            case Battle_State.Prepare:
                DisableTargets();
                Delete_Instantiated_Objects();
                break;
            case Battle_State.Swing:
                attackBar.gameObject.SetActive(false);
                break;
            case Battle_State.Results:
                staticMonster = false;
                monsterInfo.SetPrepareStateSprite();
                monsterInfo.ChangeSize(Monster_Info.Size.Normal);
                monsterInfo.ChangeMonsterSpritePos(new Vector2(0, 0));
                break;
            case Battle_State.Enemy_Death:
                victory_Button.enabled = false;
                monsterInfo.ChangeSize(Monster_Info.Size.Normal);
                break;
            default:
                break;
        }
    }
    private void EnterState(Battle_State state)
    {
        Debug.Log("enter" + state);
        switch (state)
        {

            case Battle_State.Prepare:

                attackBar.gameObject.SetActive(true);

                phaseFiller.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/Battle/" + "PreparationFiller");

                phaseFiller.fillAmount = 1;

                phaseTime.text = Get_Time(phaseFiller.fillAmount * System.Convert.ToSingle(monsterInfo.PrepareTime));
                //Maybe replace?
                staticMonster = false;
                ActivateTargets();
                Proccess_Durations();
                break;
            case Battle_State.Swing:
                phaseFiller.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/Battle/" + "SwingFiller");
                phaseFiller.fillAmount = 1;
                phaseTime.text = Get_Time(phaseFiller.fillAmount * System.Convert.ToSingle(monsterInfo.SwingTime));
                swipeDirection = ImageDirection.Null;
                monsterDirection = monsterInfo.makeRandomSwipe();
                break;
            case Battle_State.Results:

                attackBar.gameObject.SetActive(false);
                ResetFinalBlow();

                //coroutine starts, and right after that
                //actual object is disabled, so it will
                //give us error
                if (GameObject.Find("Instantiated_Objects") != null)
                    StartCoroutine(Show_Info());


                if (!staticMonster)
                {
                    monsterInfo.ChangeSize(Monster_Info.Size.Bigger);
                    switch (monsterDirection)
                    {
                        case ImageDirection.Left:
                            monsterInfo.ChangeMonsterSpritePos(new Vector2(850, 28));
                            break;
                        case ImageDirection.Right:
                            monsterInfo.ChangeMonsterSpritePos(new Vector2(-850, 28));
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    monsterInfo.ChangeSize(Monster_Info.Size.Bigger);
                    monsterInfo.ChangeMonsterSpritePos(new Vector2(0, 0));
                    monsterInfo.SetPrepareStateSprite();
                }


                break;
            case Battle_State.Enemy_Death:
                {
                    monsterInfo.ChangeSize(Monster_Info.Size.Bigger);
                    victory_Button.enabled = true;
                }
                break;
            default:
                break;
        }
    }

    private void Proccess_Hero_Passive()
    {
        string hero_Class = save_System.get_sd_char().Class_hero;
        switch (hero_Class)
        {
            case "Mage":
                AddInfoToList(new info_Text("+2 Mana", Color.blue));
                hero_Info.AdjustMana(-2);
                break;
            default:
                AddInfoToList(new info_Text(save_System.get_sd_char().Class_hero + " passive", Color.white));
                break;
        }
    }

    public void MonsterDied()
    {
        DisableTargets();
        ToggleTargetComponents(false);
        StopAllCoroutines();
        save_System.GetComponent<SceneSwitcher>().reward_Scene();      
    }

    public void ActivateFinalBlow()
    {
        finalBlowCoroutine();
    }
    public void RandomPos(GameObject target)
    {
        Vector2 randomPosition;

        randomPosition = new Vector2(Random.Range(-250, 350), Random.Range(-220, 400));

        for (int i = 0; i < GetTargetList(target).Count; i++)
        {

            if (Vector2.Distance(randomPosition, GetTargetList(target)[i].GetComponent<RectTransform>().anchoredPosition) < 100)
            {
                randomPosition = new Vector2(Random.Range(-250, 350), Random.Range(-220, 400));
                //Reset loop
                i = -1;
            }
        }
        //Debug.Log(randomPosition);
        target.GetComponent<RectTransform>().anchoredPosition = randomPosition;
    }
    public void AddFinalBlowCharge()
    {
        finalBowCharge++;
    }

    public void AddInfoToList(info_Text info)
    {
        info_List.Add(info);
    }

    public void AddSpellToList(duration_spell info)
    {
        info_Duration_List.Add(info);
    }
    public void RemoveSpellFromList(string name)
    {
        info_Duration_List.Remove(info_Duration_List.First(item => item.Spell_Name == name));
    }
    public duration_spell GetSpell(string name)
    {
        if (info_Duration_List.Count > 0)
            return info_Duration_List.First(item => item.Spell_Name == name);

        return null;
    }
    private void Proccess_Durations()
    {
        for (int i = 0; i < info_Duration_List.Count; i++)
        {   
            //Adding +1 to duraion at end of a phase        
            info_Duration_List[i].Current_Duration++;
            //Adding duration to text
            info_Duration_List[i].Slot.GetDuration().text = (info_Duration_List[i].Max_Duration - info_Duration_List[i].Current_Duration).ToString();
            //Checking if its the first phase after activation
          
            switch (info_Duration_List[i].Spell_Name)
            {
                case "Mana Leak":

                    if (info_Duration_List[i].Current_Duration == 1)
                    {
                        Set_Targets_Damage(spell_System.ManaLeak_Target_Damage);
                        finalblow_State = spell_System.ManaLeak_FinalBlow;
                        info_Duration_List[i].Slot.ToggleSkillFrame(true);
                    }
                    else if (info_Duration_List[i].Current_Duration == info_Duration_List[i].Max_Duration)
                        info_Duration_List[i].Slot.GetDuration().text = "";

                    break;
                default:
                    break;
            }
        }
    }
    private void SpawnTargets(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(Resources.Load("Prefabs/Battle/Target"), new Vector2(0, 0), Quaternion.identity, GameObject.Find("Targets").transform);
        }
    }

    
    private void SetTargetsPositions()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            RandomPos(targets[i]);
        }
    }
    
    private void ToggleTargetComponents(bool enable)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].GetComponent<Image>().enabled = false;
        }
    }
    private List<GameObject> GetTargetList(GameObject target)
    {
        List<GameObject> sortedTargets = new List<GameObject>();
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] != target)
            {
                sortedTargets.Add(targets[i]);
            }

        }
        return sortedTargets;
    }

    public void Set_Targets_Damage(Target.Target_Delegate damage_State)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].GetComponent<Target>().damage_State = damage_State;
        }
    }
    public void Set_Targets_Damage()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].GetComponent<Target>().damage_State = targets[i].GetComponent<Target>().Default;
        }
    }
    private void ActivateTargets()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].GetComponent<Target>().GetCurrentState() != TargetState.Static && targets[i].GetComponent<Target>().GetCurrentState() != TargetState.Clicked)
            targets[i].GetComponent<Target>().SwitchToStaticState();
        }
    }
    private void DisableTargets()
    {        
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].GetComponent<Target>().SwitchToInactiveState();
        }
    }
    
    private void finalBlowCoroutine()
    {
       
        DealFinalBlowDamage();
        ChangeState(Battle_State.Results);
    }
    
    private void ResetFinalBlow()
    {
        finalBlowFiller.fillAmount = 0;
        finalBowCharge = 0;
        finalBlowImage.enabled = false;
    }

    private void DealFinalBlowDamage()
    {
        staticMonster = true;

        int damage = finalblow_State();

        info_List.Add(new info_Text("Final Blow "+ damage, this.HexToColor("C48C28FF")));

        monsterInfo.Adjust_Dodge(true);

        monsterInfo.DealDamage(damage);
    }

    public int Default()
    {
        int damage = hero_Info.GetHero().Damage.value * hero_Info.GetHero().Power.value / 100;
        return damage;
    }

    private string Get_Time(float time)
    {
        string unedited_Time = time.ToString();
        return unedited_Time.Substring(0, unedited_Time.IndexOf(".") + 2);
    }
    private void Delete_Instantiated_Objects()
    {
        foreach (Transform child in GameObject.Find("Instantiated_Objects").transform)
            Destroy(child.gameObject);
    }

    protected override void action_left()
    {
        swipeDirection = ImageDirection.Left;
    }
    protected override void action_right()
    {
        swipeDirection = ImageDirection.Right;
    }
}

public class info_Text
{
    public Color Color;
    public string Info;

    public info_Text(string info, Color color)
    {
        Color = color;
        Info = info;
    }
}
public class duration_spell
{
    public string Spell_Name;
    public int Current_Duration;
    public int Max_Duration;
    public Skill_Slot Slot;

    public duration_spell(string spell_Name,int max_Duration, Skill_Slot slot)
    {
        Spell_Name = spell_Name;
        Max_Duration = max_Duration + 1;
        Slot = slot;
        Current_Duration = 0;
    }
}