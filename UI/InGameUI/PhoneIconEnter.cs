using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PhoneIconEnter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject TextHint;
    private Vector3 hoverOffset = new Vector3(0, 10f, 0);
    private Vector3 originalPosition;
    void Start()
    {
        originalPosition = transform.localPosition;
        TextHint.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localPosition = originalPosition + hoverOffset;
        TextHint.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localPosition = originalPosition;
        TextHint.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PlayerInventoryUI.Instance.isOpen)
        {
            Debug.Log("Called to close inventory panel");
            PlayerInventoryUI.Instance.isOpen = false;
            PlayerInventoryUI.Instance.inventoryUI.SetActive(false);
            ItemToolTip.Instance.SetVisibility(false);
            GameManager.Instance.FreezePlayerMovementAndInteract(false);
        }

        if(!PhoneUIManager.Instance.isOpen)
        {
            PhoneUIManager.Instance.OpenPhoneUI();
            PhoneUIManager.Instance.RefreshUI();
        }
        else
        {
            PhoneUIManager.Instance.ClosePhoneUI();
        }

        if(DocumentManager.Instance.isViewing)
        {
            DocumentManager.Instance.OnBackPressed();
        }

    }
}
