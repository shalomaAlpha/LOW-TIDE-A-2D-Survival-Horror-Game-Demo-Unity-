using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BagIconEnter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject TextHint;
    public Sprite defaultSprite;
    public Sprite hoverSprite;

    private Vector2 originalScale;
    private Image imageComponent;

    void Start()
    {
        imageComponent = GetComponent<Image>();
        originalScale = transform.localScale;
        imageComponent.sprite = defaultSprite;
        TextHint.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(PlayerInventoryUI.Instance.allowToOpenInventory)
        {
            transform.localScale = originalScale * 1.1f;
            imageComponent.sprite = hoverSprite;
            TextHint.SetActive(true);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
        imageComponent.sprite = defaultSprite;
        TextHint.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(PhoneUIManager.Instance.isOpen)
        {
            PhoneUIManager.Instance.isOpen = false;
            PhoneUIManager.Instance.ClosePhoneUI();
        }
        if(DocumentManager.Instance.isViewing)
        {
            DocumentManager.Instance.isViewing = false;
            GameObject currentShowDocument = DocumentManager.Instance.currentShowDocument;
            if (currentShowDocument != null) currentShowDocument.SetActive(false);
            DocumentManager.Instance.DocumentViewUI.SetActive(false);
        }

        if (PlayerInventoryUI.Instance.allowToOpenInventory)
        {
            PlayerInventoryUI.Instance.OpenInventoryPanel();
        }
        
    }
}
