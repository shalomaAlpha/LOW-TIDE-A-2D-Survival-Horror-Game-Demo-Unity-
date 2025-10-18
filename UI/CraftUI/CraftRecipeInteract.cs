using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftRecipeInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    RawImage PanelImage;
    Color normalColor = new Color(1f,1f,1f,0f);
    Color highlightColor = new Color(1f, 1f, 1f,0.1f);
    CraftRecipeBar craftRecipeBar;

    private void Start()
    {
        PanelImage = GetComponent<RawImage>();
        craftRecipeBar = gameObject.GetComponentInParent<CraftRecipeBar>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        PanelImage.color = highlightColor;
        if (Input.GetMouseButton(0)) return;

        CraftMouseInput.Instance.selectedCraftRecipe = craftRecipeBar.recipe;
        if(CraftManager.Instance.CanCreateOrNot(craftRecipeBar.recipe))
        {
            int outputID=craftRecipeBar.recipe.getOutputID();
            Debug.Log("Start to craft"+outputID);
            CraftMouseInput.Instance.StartCoroutineWaitForHoldStart();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PanelImage.color = normalColor;
        CraftMouseInput.Instance.CancelHold();
        CraftMouseInput.Instance.selectedCraftRecipe = null;
    }
}
