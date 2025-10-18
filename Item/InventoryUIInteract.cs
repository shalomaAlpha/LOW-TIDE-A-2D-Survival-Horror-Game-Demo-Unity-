using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryUIInteract : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IPointerEnterHandler,
    IPointerExitHandler
{
    /// <summary>
    /// This deal with the item slot interactive functions 
    /// like the dragging and mouse enter
    /// </summary>
    private PlayerInventoryUI inventoryUI;
    public int slotIndex;
    private static InventoryUIInteract draggedSlot;
    public PlayerInventoryInteract playerInventoryInteract;
    private void Start()
    {
        inventoryUI = PlayerInventoryUI.Instance;
    }

    public void SetSlotIndex(int index)
    {
        slotIndex = index;
        Debug.Log("SetIndex:"+index);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (inventoryUI == null)
        {
            //Debug.Log("Dont allow to interact inventoryUI is null");
            return;
        }
           
        if (GetComponent<RawImage>() == null) return;
        // start to drag
        else
        {
            //Debug.Log("allow to interact inventoryUI is not null");
            draggedSlot = this;
            GetComponent<RawImage>().color = new Color(1, 1, 1, 0.6f);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (inventoryUI == null || draggedSlot == null) return;
        if (GetComponent<RawImage>() == null) return;
        // stop dragging
        GetComponent<RawImage>().color = Color.white;
        GameObject targetObject = eventData.pointerEnter;
        
        if (targetObject != null)
        {

            InventoryUIInteract targetSlot = targetObject.GetComponent<InventoryUIInteract>();
            Slot_FuelOnly TargetfuelCheckComponent = targetObject.GetComponent<Slot_FuelOnly>();
            Slot_FishOnly TargetfishCheckComponent = targetObject.GetComponent<Slot_FishOnly>();

            Slot_FuelOnly DraggedFuelCheckComponent = draggedSlot.GetComponent<Slot_FuelOnly>();
            Slot_FishOnly DraggedFishCheckComponent = draggedSlot.GetComponent<Slot_FishOnly>();
            if (targetSlot != null)
            {
                int sourceIndex = draggedSlot.slotIndex;
                int targetIndex = targetSlot.slotIndex;
                int playerSlotAmount = inventoryUI.GetPlayerInventory().slotNumber;
                if (TargetfuelCheckComponent != null)  
                {
                    TargetfuelCheckComponent.AddFuelToSlot(sourceIndex, targetIndex, playerSlotAmount);
                    draggedSlot = null;
                    return;
                }
                else if(TargetfishCheckComponent!=null)  //target slot is fish_only slot
                {
                    TargetfishCheckComponent.AddFishToTheSlot(sourceIndex, targetIndex, playerSlotAmount);
                    draggedSlot = null;
                    return;
                }
                else if(DraggedFishCheckComponent!=null)
                {
                    DraggedFishCheckComponent.RemoveFishFromTheSlot(sourceIndex, targetIndex, playerSlotAmount);
                    draggedSlot = null;
                    return;
                }
                else if(DraggedFuelCheckComponent!=null)
                {
                    DraggedFuelCheckComponent.RemoveFishFromTheSlot(sourceIndex, targetIndex, playerSlotAmount);
                    draggedSlot = null;
                    return;
                }

                if (inventoryUI.allowToOpenInventory)
                {
                    inventoryUI.SwapItems(draggedSlot.slotIndex, targetSlot.slotIndex);
                    //the switch item is only happend in the player inventory
                }
                else  
                {
                    if(draggedSlot.slotIndex!=targetSlot.slotIndex)
                    {
                        playerInventoryInteract.SwapItem(draggedSlot.slotIndex, targetSlot.slotIndex);
                        Debug.Log($"Swapped items between slot {draggedSlot.slotIndex} and slot {targetSlot.slotIndex}");
                    }
             
                }
            }
        }
        draggedSlot = null;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ItemToolTip.Instance != null && ItemToolTip.Instance.visiblity)
        {
            if (inventoryUI.allowToOpenInventory)
            {
                //Debug.Log("Index: " + slotIndex);
                int playerSlotNumber = inventoryUI.GetPlayerSlotAmount();
                if (slotIndex >= playerSlotNumber) slotIndex -= playerSlotNumber;
                ItemData item=PlayerInventoryUI.Instance.returnItemDataByIndex(slotIndex);
                if (item != null) ItemToolTip.Instance.SetToolHintText(item.name, item.description);
                else ItemToolTip.Instance.SetToolHintText("", "");
            }
            else
            {
                int PlayerSlotNumber = playerInventoryInteract.GetPlayerInventoryAmount();
                if (slotIndex > PlayerSlotNumber)
                {
                    int containerSlotIndex = slotIndex - PlayerSlotNumber;
                    ItemData item = playerInventoryInteract.ReturnContainerItemDataByIndex(containerSlotIndex);
                    if (item != null) ItemToolTip.Instance.SetToolHintText(item.name, item.description);
                    else ItemToolTip.Instance.SetToolHintText("", "");
                }
                else if(slotIndex == PlayerSlotNumber)
                {
                    int containerSlotIndex = 0;
                    ItemData item = playerInventoryInteract.ReturnContainerItemDataByIndex(containerSlotIndex);
                    if (item != null) ItemToolTip.Instance.SetToolHintText(item.name, item.description);
                    else ItemToolTip.Instance.SetToolHintText("", "");
                }
                else
                {
                    ItemData item = playerInventoryInteract.ReturnPlayerInventoryItemDataByIndex(slotIndex);
                    if (item != null) ItemToolTip.Instance.SetToolHintText(item.name, item.description);
                    else ItemToolTip.Instance.SetToolHintText("", "");
                }
            }
            
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(ItemToolTip.Instance!=null)
            ItemToolTip.Instance.SetToolHintText("", "");
    }

}
