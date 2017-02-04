using UnityEngine;
using System.Collections;

public class DamageText : MonoBehaviour {

    [SerializeField]
    private float speed;

    private RectTransform rectTransform;

	// Use this for initialization
	void Start () {
        rectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + speed);
        if (rectTransform.anchoredPosition.y > 536.7f)
            Destroy(gameObject);
	}
}
