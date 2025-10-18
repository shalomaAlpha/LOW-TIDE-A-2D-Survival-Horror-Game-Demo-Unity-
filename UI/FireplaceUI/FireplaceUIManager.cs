using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Should be attached in the fireplace UIManager
/// the open and close is handled by fireplace.cs
/// 1. Handle the hint display and content 
/// </summary>
public class FireplaceUIManager : MonoBehaviour
{
    public static FireplaceUIManager Instance;
    public TMP_Text hintText;
    Coroutine hintAnimation;

    private float originalAlphua = 1f;
    private void Awake()
    {
        Instance = this;
    }


    IEnumerator setTextHint(float duration)
    {
        Color color = hintText.color;

        color.a = 0f;
        hintText.color = color;

        float fadeInDuration = 0.3f;
        float t = 0f;
        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0, originalAlphua, t / fadeInDuration);
            hintText.color = color;
            yield return null;
        }

        // Wait
        yield return new WaitForSeconds(duration);

        // Fade Out
        float fadeOutDuration = 0.3f;
        t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(originalAlphua, 0, t / fadeOutDuration);
            hintText.color = color;
            yield return null;
        }
    }

    public void PlayerHintAnimation(string content, float duration)
    {
        if (hintAnimation != null)
        {
            StopCoroutine(hintAnimation);
            Debug.Log("stop the old animation");
        }

        hintText.text = content; 
        hintAnimation = StartCoroutine(setTextHint(duration));
    }
}
