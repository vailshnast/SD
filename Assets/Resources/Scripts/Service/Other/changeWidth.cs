using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class changeWidth : MonoBehaviour {

    public Image filler;
    public Alignment alignment;
    private RectTransform rectTransform;
    public enum Alignment
    {
        Left,
        Right
    }

	// Use this for initialization
	void Start () {
        rectTransform = GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if(alignment == Alignment.Left)
        {
            rectTransform.sizeDelta = new Vector2(filler.GetComponent<RectTransform>().rect.width * filler.fillAmount, rectTransform.sizeDelta.y);
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(filler.GetComponent<RectTransform>().rect.width - filler.GetComponent<RectTransform>().rect.width * filler.fillAmount, rectTransform.sizeDelta.y);
        }
	}
}
