using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoadingText : MonoBehaviour
{
    public TMP_Text dayInforText;
    private Coroutine textAnimation;

    public void StartTextAnimation()
    {
        int dayNumber = TimeSystem.Instance.day;
        string infor = "DAY" + dayNumber;
        dayInforText.text = infor;

        textAnimation = StartCoroutine(TextAnimation());
    }

    public void EndAniamtion()
    {
        if(textAnimation!=null)
        {
            StopCoroutine(textAnimation);
        }
    }

    private IEnumerator TextAnimation()
    {
        float duration = 0.5f;
        float timer = 0f;
        float alpha = 1f;
        bool fadingOut = true;

        while (true)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            if (fadingOut)
                alpha = Mathf.Lerp(1f, 0f, t); 
            else
                alpha = Mathf.Lerp(0f, 1f, t); 

            Color color = dayInforText.color;
            color.a = alpha;
            dayInforText.color = color;

            if (timer >= duration)
            {
                fadingOut = !fadingOut;
                timer = 0f;
            }

            yield return null;
        }
    }
}
