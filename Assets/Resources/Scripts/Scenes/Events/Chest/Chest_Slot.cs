using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest_Slot:MonoBehaviour{

    public float temp_Width { get; set; }
    public int slot_ID { get; set; }
    
    public bool unlocked { get;private set;}

    public Arrow_Direction direction;
    public Vector2 previousPos { get; set; }

    public void SetDirection()
    {
        direction = (Arrow_Direction)Random.Range(0, 2);
        transform.GetChild(0).GetComponent<Image>().sprite = direction == Arrow_Direction.Left ? transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/Explore/Arrow_Left") : transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/Explore/Arrow_Right");
    }

    public bool Unlocked(Arrow_Direction direction)
    {
        if (this.direction == direction)
        {
            transform.GetChild(0).GetComponent<Image>().sprite = direction == Arrow_Direction.Left ? transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/Explore/Arrow_Left") : transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/Explore/Arrow_Right");
            transform.GetChild(0).gameObject.SetActive(true);
            unlocked = true;
            return true;
        }
        else
        {
            FindObjectOfType<Chest>().Lockpick_Try_Failed();
            transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CorePrototype/Explore/Cross");
            transform.GetChild(0).gameObject.SetActive(true);
            unlocked = false;
            return false;
        }
    }
}
public enum Arrow_Direction
{
    Left,
    Right
}
