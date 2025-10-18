using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot_FuelOnly : MonoBehaviour
{
    public static Slot_FuelOnly Instance;
    private PlayerInventoryUI inventoryUI;
    private InventoryUIInteract selfInventoryUIInteract;
    public Inventory FireplaceInventory;


    private Inventory playerInventory;
    public Inventory fireplaceInventory;

    Fireplace fireplace;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        selfInventoryUIInteract = GetComponent<InventoryUIInteract>();
        inventoryUI = PlayerInventoryUI.Instance;
        fireplace = Fireplace.Instance;
        fireplaceInventory = fireplace.GetComponent<Inventory>();
        playerInventory = PlayerController.Instance.GetComponent<Inventory>();
    }

    public bool CheckPutItemIsFuelOrNot(int SourceIndex, int TargetIndex, int PlayerSlotsAmount)
    {
        ItemData sourceItem;
        ItemData targetItem;
        if (SourceIndex>=PlayerSlotsAmount)
        {
            SourceIndex -= PlayerSlotsAmount;
            sourceItem=FireplaceInventory.GetItemByIndex(SourceIndex);
        }
        else
        {
            sourceItem = inventoryUI.returnItemDataByIndex(SourceIndex);
        }
        if(TargetIndex>=PlayerSlotsAmount)
        {
            TargetIndex -= PlayerSlotsAmount;
            targetItem=FireplaceInventory.GetItemByIndex(TargetIndex);
        }
        else
        {
            targetItem = inventoryUI.returnItemDataByIndex(TargetIndex);
        }

        int sourceItemID=0;
        int targetItemID=0;
        if (sourceItem!=null)
        {
            sourceItemID = sourceItem.id;
        }
        if(targetItem!=null)
        {
            targetItemID = targetItem.id;
        }
        //Debug.Log("Source ID: " + sourceItemID + "Target ID" + targetItemID);
        if (targetItem==null && sourceItem==null)
        {
            return false; //both slots are empty
        }    
        else if(targetItem==null && sourceItemID==14)
        {
            //Debug.Log("Source ID: " + sourceItemID + "Target ID" + targetItemID);
            fireplace.AddFuel(1);
            return true;
        }

        else if(targetItemID == 14 && sourceItem==null) 
        {
            fireplace.AddFuel(1);
            //fireplace.AddFirewood(120);
            return true;
        }
        else if(targetItemID!=14 && sourceItemID!=14)
        {
            Debug.Log("item is not fuel");
            return false;
        }
        return false;
    }

    public void RemoveFuelItem()
    {
        int playerSlotAmount =PlayerInventoryUI.Instance.GetPlayerSlotAmount();
        int index = selfInventoryUIInteract.slotIndex -playerSlotAmount;

        FireplaceInventory.RemoveItemByIndex(index, 1);
        selfInventoryUIInteract.playerInventoryInteract.UpdateSlotIconInformation();
    }

    public void AddFuelToSlot(int sourceIndex, int targetIndex, int PlayerSlotsAmount)
    {
        Debug.Log("Add fuel to slot");
        int playerSlotIndex = sourceIndex;
        int firePlaceSlotIndex = targetIndex - PlayerSlotsAmount;

        ItemData item = playerInventory.GetItemByIndex(playerSlotIndex);
        if (item != null)
        {
            int itemID = item.id;
            if (itemID ==14)  //fuel id is 14
            {
                playerInventory.RemoveItemByIndex(playerSlotIndex, 1);
                fireplaceInventory.AddItemToIndex(firePlaceSlotIndex, item, 1);
                fireplace.AddFuel(1);
            }
            else
            {
                string content = "Only fuel can be added to fuel slot";
                float duration = 1.5f;
                FireplaceUIManager.Instance.PlayerHintAnimation(content, duration);
            }
            selfInventoryUIInteract.playerInventoryInteract.UpdateSlotIconInformation();
        }
    }

    public void RemoveFishFromTheSlot(int sourceIndex, int targetIndex, int PlayerSlotsAmount)
    {
        Debug.Log("Remove fuel from fireplace");
        int playerSlotIndex = targetIndex;
        int FirePlaceSlotIndex = sourceIndex - PlayerSlotsAmount;
        
        ItemData item = fireplaceInventory.GetItemByIndex(FirePlaceSlotIndex);
        if (item != null)
        {
            fireplaceInventory.RemoveItemByIndex(FirePlaceSlotIndex, 1);
            playerInventory.AddItemToIndex(playerSlotIndex, item, 1);
            fireplace.RemoveFuel(1);
            selfInventoryUIInteract.playerInventoryInteract.UpdateSlotIconInformation();
        }
    }

}
