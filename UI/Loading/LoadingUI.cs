using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    public static LoadingUI Instance;
    public LoadingText loadText;
    public GameObject LoadingCanvas;
    private CanvasGroup canvasGroup;

    Coroutine loadingCoroutine;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        canvasGroup = LoadingCanvas.GetComponent<CanvasGroup>();
        Initilize();

    }

    public void Initilize()
    {
        canvasGroup.alpha = 0;
        LoadingCanvas.SetActive(false);
    }

    public void StartLoadingAnimation()
    {
        if (loadingCoroutine != null)
            StopCoroutine(loadingCoroutine);
        loadingCoroutine = StartCoroutine(DoLoadingAnimation());
    }


    private IEnumerator DoLoadingAnimation()
    {
        GameManager.Instance.FreezePlayerMovementAndInteract(true);
        LoadingCanvas.SetActive(true);
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 0, 1, 1f)); 

        loadText.StartTextAnimation();
        yield return new WaitForSeconds(3f);
        loadText.EndAniamtion();

        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 1, 0, 1f)); 
        LoadingCanvas.SetActive(false);
        GameManager.Instance.FreezePlayerMovementAndInteract(false);
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup group, float start, float end, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            float t = time / duration;
            group.alpha = Mathf.Lerp(start, end, t);
            time += Time.deltaTime;
            yield return null;
        }
        group.alpha = end;
    }
}
