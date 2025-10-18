using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftMouseInput : MonoBehaviour
{
    public static CraftMouseInput Instance;
    public Image progressImage;
    float holdTime = 3f; 
    private bool isHolding = false;

    private Coroutine holdCoroutine;
    private Coroutine fillCoroutine;

    private bool craftOnProcess;

    public Recipe selectedCraftRecipe;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (progressImage != null)
            progressImage.gameObject.SetActive(false);
        craftOnProcess = false;
    }
    void Update()
    {
        if (isHolding && Input.GetMouseButtonUp(0))
        {
            CancelHold();
            craftOnProcess = false;
        }

        if (progressImage != null)
            progressImage.transform.position = Input.mousePosition;
    }
    public void StartCoroutineWaitForHoldStart()
    {
        if(!craftOnProcess)
        {
            holdCoroutine = StartCoroutine(WaitForHoldStart());
        } 
    }


    IEnumerator WaitForHoldStart()
    {
        craftOnProcess = true;
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        isHolding = true;
        if (progressImage != null)
        {
            progressImage.fillAmount = 0f;
            progressImage.transform.position = Input.mousePosition;
            progressImage.gameObject.SetActive(true);
        }
        if(fillCoroutine != null)
            StopCoroutine(fillCoroutine);
        fillCoroutine = StartCoroutine(FillProgress());
        CraftTableSound.Instance.PlayClipProcessingOrComplete(true);
    }

    IEnumerator FillProgress()
    {
        float timer = 0f;
        progressImage.fillAmount = 0f;

        while (timer < holdTime)
        {
            if (!Input.GetMouseButton(0))
            {
                CancelHold();
                yield break;
            }

            timer += Time.deltaTime;
            progressImage.fillAmount = timer / holdTime;
            yield return null;
        }

        if (progressImage != null)
        {
            progressImage.fillAmount = 1f;
            progressImage.gameObject.SetActive(false);
        }
        
        isHolding = false;

        craftOnProcess = false;
        PlayerController playerController = PlayerController.Instance;
        Inventory playerInventory = playerController.GetComponent<Inventory>();
        CraftManager.Instance.Craft(playerInventory, selectedCraftRecipe);
        CraftTableSound.Instance.PlayClipProcessingOrComplete(false);
    }

    public void CancelHold()
    {
        if (!isHolding) return;

        if (holdCoroutine != null)
            StopCoroutine(holdCoroutine);
        if (fillCoroutine != null)
            StopCoroutine(fillCoroutine);

        if (progressImage != null)
        {
            progressImage.fillAmount = 0f;
            progressImage.gameObject.SetActive(false);
        }
        CraftTableSound.Instance.StopPlay();
        craftOnProcess = false;
        isHolding = false;
    }
}
