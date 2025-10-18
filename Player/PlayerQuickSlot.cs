using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQuickSlot : MonoBehaviour
{
    public static PlayerQuickSlot Instance; 
    Inventory playerInventory;  
    public int selectedSlot = 0; // current selected slot

    ItemData selectedItem;

    InventorySlot selectedItemSlot;
    PlayerAttack playerAttack;
    PlayerConsumableItem consumableItemComponent;

    public bool allowToInteract=true;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        playerInventory = GetComponent<Inventory>();
        playerAttack = GetComponent<PlayerAttack>();
        consumableItemComponent = GetComponent<PlayerConsumableItem>();
        QuickSlotUIManager.Instance.CursorMoveToIcon(0);
    }
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectSlot(4);

        if (PlayerInventoryUI.Instance.isOpen) return;

        if (allowToInteract&& Input.GetMouseButtonDown(0))
        {
            if (selectedItem != null)
            {
                switch (selectedItem.type)
                {
                    case ItemType.Weapon:
                        playerAttack.Attack(selectedItem);
                        break;
                    case ItemType.Consumable:
                        consumableItemComponent.UseSupply(selectedItem);
                        break;
                }
            }
        }
    }

    public void SelectSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= playerInventory.slots.Count)
            return;
        selectedItemSlot = playerInventory.slots[slotIndex];
        QuickSlotUIManager.Instance.CursorMoveToIcon(slotIndex);
        if(selectedItemSlot!=null &&selectedItemSlot.amount!=0)
        {
            selectedItem = selectedItemSlot.item;
            Debug.Log("select" + selectedItem.name);
        }
        else
        {
            selectedItem = null;
        }
        
    }
    public ItemData getCurrentSelectedItemData()
    {
        if(selectedItemSlot==null)
        {
            return null;
        }
        if(selectedItemSlot.item==null)
        {
            return null;
        }
        else
        {
            return selectedItemSlot.item;
        }
    }

}
