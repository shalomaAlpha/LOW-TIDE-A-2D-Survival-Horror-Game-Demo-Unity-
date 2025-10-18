using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// 1.handle the add and remove of the fish_only slot;
/// 2.handle the coroutine of cooking fish
/// 3.handle the UI element of cooking fish 
/// </summary>
public class Slot_FishOnly : MonoBehaviour
{
    public static Slot_FishOnly Instance;
    private Inventory playerInventory;
    public Inventory fireplaceInventory;
    public Image prograssBar;
    public GameObject outputSlot;

    public TMP_Text hintText;

    InventoryUIInteract selfInventoryUIInteract;
    InventoryUIInteract outputInventoryUIInteract;
    Coroutine grillFishCoroutine;
    private PlayerInventoryUI inventoryUI;

    int timeValue;
    int requiredTime=10;
    int fishAmount=0;
    public bool isGrilling;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        fishAmount = 0;
        selfInventoryUIInteract = GetComponent<InventoryUIInteract>();
        inventoryUI = PlayerInventoryUI.Instance;
        outputInventoryUIInteract = outputSlot.GetComponent<InventoryUIInteract>();
        prograssBar.fillAmount = 0;
    }

    private void Update()
    {
        if (playerInventory == null)
        {
            playerInventory = PlayerController.Instance.GetComponent<Inventory>();
        }
        else
        {
            if (isGrilling) return;
            else
            {
                int playerSlotAmount = playerInventory.slotNumber;
                int index = selfInventoryUIInteract.slotIndex - playerSlotAmount;
                ItemData item = fireplaceInventory.GetItemByIndex(index);

                if (item == null) return;
                else if(item.id>=1 && item.id<=4)
                {
                    if(Fireplace.Instance.isBurning &&fishAmount!=0)
                    {
                        if(CheckOutputSlotIsAvailableOrNot(playerSlotAmount))
                        {
                            startToCookFish();
                        }
                        
                    }
                }
                else
                {
                    return;
                }
            }
        }
    }

    void startToCookFish()
    {
        int playerSlotAmount = playerInventory.slotNumber;
        isGrilling = true;
        grillFishCoroutine = StartCoroutine(GrillFish(playerSlotAmount));
    }

    public void AddFishToTheSlot(int sourceIndex,int targetIndex, int PlayerSlotsAmount)
    {
        Debug.Log("Add fish to slot");
        int playerSlotIndex = sourceIndex;
        int firePlaceSlotIndex = targetIndex - PlayerSlotsAmount;

        ItemData fishData = playerInventory.GetItemByIndex(playerSlotIndex);
        if (fishData != null)
        {
            int itemID = fishData.id;
            if(itemID>=1&&itemID<=4)
            {
                playerInventory.RemoveItemByIndex(playerSlotIndex, 1);
                fireplaceInventory.AddItemToIndex(firePlaceSlotIndex, fishData,1);
                fishAmount++;
            }
            else
            {
                string content = "Only fish can be added to this slot";
                float duration = 1.5f;
                FireplaceUIManager.Instance.PlayerHintAnimation(content,duration);
            }
            selfInventoryUIInteract.playerInventoryInteract.UpdateSlotIconInformation();
        }
    }

    public void RemoveFishFromTheSlot(int sourceIndex, int targetIndex, int PlayerSlotsAmount)
    {
        Debug.Log("Remove fish from container");
        int playerSlotIndex=targetIndex;
        int FirePlaceSlotIndex=sourceIndex-PlayerSlotsAmount;
        fishAmount--;
        ItemData fishData = fireplaceInventory.GetItemByIndex(FirePlaceSlotIndex);
        if(fishData!=null)
        {
            fireplaceInventory.RemoveItemByIndex(FirePlaceSlotIndex, 1);
            playerInventory.AddItemToIndex(playerSlotIndex, fishData, 1);
            StopGrilledFish();
            selfInventoryUIInteract.playerInventoryInteract.UpdateSlotIconInformation();
        }
    }

    IEnumerator GrillFish(int PlayerSlotsAmount)  //modify it to hold more fish
    {
        Debug.Log("Start To Grill Fish");
        prograssBar.fillAmount = 0;
        while(timeValue<=requiredTime)
        {
            yield return new WaitForSeconds(1f);
            timeValue += 1;
            float fillAmount = (float)timeValue / requiredTime;
            //Debug.Log("Current percentage:" + fillAmount);
            prograssBar.fillAmount = fillAmount;
        }

        int SelfIndex = selfInventoryUIInteract.slotIndex;
        SelfIndex -= PlayerSlotsAmount;
        int OutputIndex = outputInventoryUIInteract.slotIndex;
        OutputIndex -= PlayerSlotsAmount;


        fireplaceInventory.RemoveItemByIndex(SelfIndex,1);
        ItemData grilledFish = ItemDatabase.Instance.GetItemById(8); 
        fireplaceInventory.AddItemToIndex(OutputIndex, grilledFish, 1);

        selfInventoryUIInteract.playerInventoryInteract.UpdateSlotIconInformation();
        timeValue = 0;  
        prograssBar.fillAmount = 0;
        fishAmount--;   //reduce fish amount
        isGrilling = false;
    }

    public void StopGrilledFish()
    {
        Debug.Log("Stop grill fish");
        if(grillFishCoroutine!=null)
            StopCoroutine(grillFishCoroutine);
        timeValue = 0;
        isGrilling = false;
        prograssBar.fillAmount = 0;
    }

    public void AddTimeValueToCorotinue(int timeValueChange)
    {
        if (!isGrilling) return;
        else
        {
            timeValue += timeValueChange;
        }
    }

    private bool CheckOutputSlotIsAvailableOrNot(int playerSlotAmount)
    {
        int index = outputInventoryUIInteract.slotIndex - playerSlotAmount;
        ItemData item = fireplaceInventory.GetItemByIndex(index);
        InventorySlot slot = fireplaceInventory.slots[index];
        if (item == null) return true;
        else if(item.id == 8 && slot.amount < item.maxStack)
        {
            return true;
        }
        else
        {
            string content = "Please empty the output slot first";
            FireplaceUIManager.Instance.hintText.text=content;
            return false;
        }
    }
}
