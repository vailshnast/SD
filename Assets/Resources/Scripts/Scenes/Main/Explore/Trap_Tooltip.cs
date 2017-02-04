using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Trap_Tooltip : MonoBehaviour {

    private GameObject tooltip;
    private Image health_Filler;
    private Image damage_Filler;
    private Text health_Amount_Text;
    private Text tooltip_Description;

    public int damage_Percentage;

    void Start()
    {
        tooltip = transform.FindDeepChild("Tooltip").gameObject;

        health_Filler = tooltip.transform.FindDeepChild("Health_Filler").GetComponent<Image>();
        damage_Filler = tooltip.transform.FindDeepChild("Damage_Filler").GetComponent<Image>();
        health_Amount_Text = tooltip.transform.FindDeepChild("Health_Text").GetComponent<Text>();
        tooltip_Description = tooltip.transform.FindDeepChild("Tooltip_Description").GetComponent<Text>();

        tooltip.SetActive(false);
    }

    private SD_Character get_Char()
    {
        return SaveSystem.Instance.get_sd_char();
    }
    private IEnumerator Fade_Filler(float timeToFade)
    {
        float t = 0f;

        float current_Amount = damage_Filler.fillAmount;


        while (t <= 1)
        {
            t += Time.deltaTime / timeToFade;
            damage_Filler.fillAmount = Mathf.Lerp(current_Amount, health_Filler.fillAmount, t);
            yield return null;
        }
        yield return new WaitForSeconds(1);
    }
    public void Activate()
    {
        tooltip.SetActive(true);

        float damageAmount = ((float)get_Char().Health.value / 100) * damage_Percentage;


        get_Char().Health_Current.value -= System.Convert.ToInt32(damageAmount);
        Death();
        health_Filler.fillAmount = (float)get_Char().Health_Current.value / get_Char().Health.value;

        health_Amount_Text.text = get_Char().Health_Current.value + "/" + get_Char().Health.value;
        tooltip_Description.text = "You`ve lost " + damageAmount + " <color=red>health</color>";

        

        StartCoroutine(Fade_Filler(1));
    }
    private void Death()
    {
        if (get_Char().Health_Current.value <= 0)
        {
            StopAllCoroutines();
            SaveSystem.Instance.CreateNewSaveFile();
            SceneManager.LoadScene(0);
        }
    }
    public  void Deactivate()
    {
        tooltip.SetActive(false);
    }
}
