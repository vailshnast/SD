using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Torch : MonoBehaviour {

    void OnEnable()
    {
        GetComponent<Text>().text = FindObjectOfType<SaveSystem>().get_sd_char().Torch_Quantity.ToString();
    }
}
