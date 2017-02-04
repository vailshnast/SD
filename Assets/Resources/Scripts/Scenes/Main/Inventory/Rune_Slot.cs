using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Rune_Slot : MonoBehaviour {

    private bool rune_Slot_Is_Emty = true;

    public bool Rune_Slot_Is_Emty
    {
        get
        {
            return rune_Slot_Is_Emty;
        }
        set
        {
            rune_Slot_Is_Emty = value;
        }
    }


    private Item runeItem;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void InsertRune(Data_Item_Explore item)
    {
        runeItem = item.item;


        transform.GetChild(0).GetComponent<Image>().enabled = true;
        transform.GetChild(0).GetComponent<Image>().sprite = FindRuneSprite(runeItem.GetSprite().name);


        Rune_Slot_Is_Emty = false;
    }
    public void DestroyRune()
    {
        transform.GetChild(0).GetComponent<Image>().enabled = false;
        Rune_Slot_Is_Emty = true;
    }

    private Sprite FindRuneSprite(string runeName)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Items/Runes");
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].name == runeName)
                return sprites[i];
        }
        return null;
    }

    public Item GetRuneItem()
    {
        return runeItem;
    }

}
