using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HPUIController : MonoBehaviour
{
    public TMP_Text HPtext;
    public HPComponent PlayerHPcomponent;
    public Image HPBar;
    int playerCurrentHP;
    float currentHPpercentage;
    void Start()
    {
        playerCurrentHP = PlayerHPcomponent.maxHP;
        currentHPpercentage = 1f;
        HPBar.fillAmount = currentHPpercentage;
        HPtext.text = "HP: " + PlayerHPcomponent.maxHP+" / "+ PlayerHPcomponent.maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCurrentHP == PlayerHPcomponent.currentHP) return;
        else
        {
            int changeValue = PlayerHPcomponent.currentHP - playerCurrentHP;
            playerCurrentHP = PlayerHPcomponent.currentHP;
            float changePercentage = changeValue / PlayerHPcomponent.maxHP;
            StartCoroutine(HPBarChangeAnimation(changeValue,changePercentage,2f));
        }
    }

    IEnumerator HPBarChangeAnimation(int changeValue, float changePercentage,float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            int HealthToAdd = Mathf.RoundToInt(Mathf.Lerp(0, changeValue, t));
            float fillToAdd = Mathf.Lerp(0, changePercentage, t);

            HPtext.text = "HP: "+$"{playerCurrentHP+HealthToAdd} / {PlayerHPcomponent.maxHP}";
            HPBar.fillAmount = currentHPpercentage+fillToAdd;

            yield return null;
        }
    }
}
