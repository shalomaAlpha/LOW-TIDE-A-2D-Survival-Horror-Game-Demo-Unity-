using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
public class ReturnIconItemInformation : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public ItemData DisplayItem;
    public int amount;
    public TMP_Text amountText;
    public Image itemIcon;
    public IconDatabase iconDatabase;

    // Update is called once per frame
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ItemToolTip.Instance != null && ItemToolTip.Instance.visiblity)
        {
            string name = DisplayItem.name;
            string description = DisplayItem.description;
            ItemToolTip.Instance.SetToolHintText(name, description);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ItemToolTip.Instance != null)
            ItemToolTip.Instance.SetToolHintText("", "");
    }

    public void SetUIElement()
    {
        int id = DisplayItem.id;
        itemIcon.sprite = iconDatabase.GetIcon(id);
        if (amount == 1)
        {
            amountText.text = " ";
        }
        else
        {
            amountText.text = "x" + amount;
        }
    }
}
