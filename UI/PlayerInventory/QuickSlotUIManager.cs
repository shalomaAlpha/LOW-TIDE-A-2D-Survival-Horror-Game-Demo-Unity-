using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class QuickSlotUIManager : MonoBehaviour
{
    public static QuickSlotUIManager Instance { get; private set; }
    public IconDatabase iconDatabase;
    public Inventory playerInventory;
    public Image[] ItemIcon;
    public TMP_Text[] AmountText;
    public RectTransform cursorImage;
    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        for(int i=0;i<5;i++)
        {
            InventorySlot inventorySlot = playerInventory.slots[i];
            if (inventorySlot == null)
            {
                ItemIcon[i].enabled=false;
                AmountText[i].text = " ";
            }
            else
            {
                if (inventorySlot.amount != 0)
                {
                    ItemIcon[i].enabled = true;
                    Sprite icon = iconDatabase.GetIcon(inventorySlot.item.id);
                    if (icon != null)
                    {
                        ItemIcon[i].sprite = icon;
                    }
                    else
                    {
                        ItemIcon[i].sprite = iconDatabase.GetIcon(0);
                    }
                    AmountText[i].text = "X" + playerInventory.slots[i].amount;
                }
                else
                {
                    ItemIcon[i].enabled = false;
                    AmountText[i].text = " ";
                }
            }
        }
    }
    public void CursorMoveToIcon(int index)
    {
        cursorImage.position = ItemIcon[index].rectTransform.position;
    }
}
