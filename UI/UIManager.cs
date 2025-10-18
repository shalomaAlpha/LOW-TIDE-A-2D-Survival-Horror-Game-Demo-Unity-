using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 1.handle the bottom text hint
/// 2.handle the display of in game UI
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public CanvasGroup InGameUI;

    [SerializeField] TextMeshProUGUI tmpGuide;
    public bool AllowToDisplayOtherHint;

    Coroutine GuideDisplayAnimation;
    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
            AllowToDisplayOtherHint = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ModifyGuideText(string guideContent)
    {
        tmpGuide.text = guideContent;
    }

    public void DisplayBottomHint(float duration, string content)
    {
        if(GuideDisplayAnimation!=null)
        {
            StopCoroutine(GuideDisplayAnimation);
            GuideDisplayAnimation = StartCoroutine(HintDisplay(duration, content));
        }
    }

    IEnumerator HintDisplay(float duration, string content)
    {
        tmpGuide.text = content;
        yield return new WaitForSeconds(duration);
    }

    public void DisplayOrHideInGameUI(bool display)
    {
        if (display) InGameUI.alpha = 1;
        else InGameUI.alpha = 0;

        InGameUI.interactable = display;
        InGameUI.blocksRaycasts = display;
    }
}
