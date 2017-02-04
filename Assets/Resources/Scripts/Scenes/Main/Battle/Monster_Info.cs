using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Monster_Info : MonoBehaviour {

    private Monster monster;

    private Hero_Info hero_Info;
    
    public string MonsterName { get; set; }
    public int Health { get; set; }
    public int Dodge { get; set; }
    public int Defence { get; set; }
    public int Damage { get; set; }
    public double PrepareTime { get; set; }
    public double SwingTime { get; set; }

    private Dictionary<ImageDirection, Sprite> Sprites = new Dictionary<ImageDirection, Sprite>();
    private Image monsterImage;

    private int maxHealth;

    private Image healthFiller;

    public enum Size
    {
        Bigger,
        Normal
    }

    void Awake()
    {
        Initialise();
    }
	// Update is called once per frame
	void OnEnable () {
        if(maxHealth != 0)
        healthFiller.fillAmount = (float)Health / maxHealth;
    }
    
    void Initialise()
    {       
        hero_Info = FindObjectOfType<Hero_Info>();
        monsterImage = GameObject.Find("Enemy_Sprite").GetComponent<Image>();
        healthFiller = GameObject.Find("EnemyHP_Filler").GetComponent<Image>();
    }

    public Monster GetMonster()
    {
        return monster;
    }

    public void DealDamage(int damage)
    {
        Health = Health - damage;
        healthFiller.fillAmount = (float)Health / maxHealth;
    }
    public void SetMonsterParameters(Monster monster)
    {
        this.monster = monster;
        MonsterName = monster.MonsterName;
        Health = monster.Health;
        Dodge = monster.Dodge;
        Defence = monster.Defence;
        Damage = monster.Damage;
        PrepareTime = monster.PrepareTime;
        SwingTime = monster.SwingTime;

        Sprites = monster.GetSprites();   
        maxHealth = Health;
    }
    public void ShakeEnemySprite(float time , Vector2 shakeAmount)
    {
        iTween.ShakePosition(monsterImage.gameObject, shakeAmount, time);
    }
    public ImageDirection makeRandomSwipe()
    {
        ImageDirection direction;

        direction = GetRandomDirection();
        if (direction == ImageDirection.Left)
            monsterImage.sprite = Sprites[ImageDirection.Left];
        else
            monsterImage.sprite = Sprites[ImageDirection.Right];

        return direction;
    }
    public void ChangeSize(Size size)
    {
        RectTransform rectTransform = monsterImage.GetComponent<RectTransform>();
        switch (size)
        {
            case Size.Bigger:
                rectTransform.sizeDelta = new Vector2(962.5f, 883.5f);
                break;
            case Size.Normal:
                rectTransform.sizeDelta = new Vector2(641, 589);
                break;
            default:
                break;
        }
    }
    public void ChangeMonsterSpritePos(Vector2 pos)
    {
        monsterImage.GetComponent<RectTransform>().anchoredPosition = pos;
    }
    public void Adjust_Dodge(bool reset)
    {
        if (reset)
            Dodge += 2;
        else
            Dodge = monster.Dodge;

        hero_Info.SetTextParameters();
    }
    public void MoveSprite(float speed)
    {
        RectTransform rectTransform = monsterImage.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + speed, rectTransform.anchoredPosition.y);
    }
    public ImageDirection GetRandomDirection()
    {
        List<ImageDirection> directions = new List<ImageDirection>();
        directions.Add(ImageDirection.Left);
        directions.Add(ImageDirection.Right);
        return directions[Random.Range(0, directions.Count)];
    }
    public void SetPrepareStateSprite()
    {
        monsterImage.sprite = Sprites[ImageDirection.Main];
    }
}
