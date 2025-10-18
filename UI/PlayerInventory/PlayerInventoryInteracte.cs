using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// This script handles Panel Slots UI interact
/// Including: 
/// 1. Slots UI element spawn
/// 2. Assign New listener evenet to UI button
/// 3. Panel Close and Open
/// 4. Update Item Icons
/// </summary>

// Container index should larger than player Inventory index.
// However, scripts will automatically find its index in the list by using -.
[RequireComponent(typeof(Inventory))]
public class PlayerInventoryInteract : MonoBehaviour
{
    public IconDatabase iconDatabase;
    public GameObject inventoryUICanvas; //The whole canvas need to set active boolen  
    public GameObject itemSlotPrefab; 
    public Transform GridLayoutPanel; //where spawn item slot prefabs 
    public List<GameObject> PlayeritemSlots; //store player item slot prefabs, this will be set in the start function
    public List<GameObject> InventorySlots; //store container item slot prefabs, this will be set in the start function
    public Transform parentTransform; //the parent of crafttable/trade inventoryslot

    List<Image> PlayericonList = new List<Image>();
    List<TMP_Text> PlayerItemAmountTextList = new List<TMP_Text>();

    List<Image> ContainericonList = new List<Image>();
    List<TMP_Text> ContainerAmountTextList = new List<TMP_Text>();

    int InventorySlotNumber = 5;  //quick slots are already exisit
    Inventory PlayerInventory;
    Inventory ContainerInventory;

    bool Initilized = false;

    void Start()
    {
        foreach (Transform child in parentTransform)
        {
            InventorySlots.Add(child.gameObject);
        }
        inventoryUICanvas.SetActive(false);
        ContainerInventory = GetComponent<Inventory>();
    }

    void Update()
    {
        if (Initilized) return;
        if(PlayerInventory==null)
        {
            PlayerInventory = PlayerController.Instance.GetComponent<Inventory>();
        }
        else
        {
            Initilized = true;
            InitilizePlayerQuickSlotUI();
            InitilizedInventorySlotUI();
        }
    }

    private void InitilizePlayerQuickSlotUI()
    {
        PlayerInventoryUI playerInventoryUI = PlayerInventoryUI.Instance;
        for (int i = 0; i < 5; i++)
        {
            GameObject slot = playerInventoryUI.itemSlots[i];
            InventoryUIInteract inventoryUIInteract = slot.GetComponent<InventoryUIInteract>();
            inventoryUIInteract.playerInventoryInteract = this;
            PlayeritemSlots.Add(slot);
            PlayericonList.Add(slot.GetComponentInChildren<Image>());
            PlayerItemAmountTextList.Add(slot.GetComponentInChildren<TMP_Text>());
        }
    }

    public void SwapItem(int draggedSlotIndex, int targetSlotIndex)
    {
        int playerInventorySlotNumber = PlayerInventory.slotNumber;
        if (targetSlotIndex < playerInventorySlotNumber
            && draggedSlotIndex < PlayerInventory.slotNumber)
        {
            //selet item is included in player inventory
            PlayerInventory.SwapItems(targetSlotIndex, draggedSlotIndex);
        }
        else if (
            (targetSlotIndex >= playerInventorySlotNumber
            && draggedSlotIndex < playerInventorySlotNumber)
            || (targetSlotIndex < playerInventorySlotNumber
            && draggedSlotIndex >= playerInventorySlotNumber)
            )
        {
            Debug.Log("TransformCall is slore not solely within player inventory");
            int playerSlotIndex;
            int containerSlotIndex;
            //select item is not only in the player's inventory
            if (targetSlotIndex > draggedSlotIndex)
            {
                containerSlotIndex = targetSlotIndex - playerInventorySlotNumber;
                playerSlotIndex = draggedSlotIndex;
            }
            else
            {
                containerSlotIndex = draggedSlotIndex - playerInventorySlotNumber;
                playerSlotIndex = targetSlotIndex;
            }
            //Debug.Log("ContainerSlot index: " + containerSlotIndex +
             //   " PlayerInventorySlot index:" + playerSlotIndex);
            ItemData ContainerItem = ContainerInventory.GetItemByIndex(containerSlotIndex);
            int ContainerItemAmount = ContainerInventory.GetItemAmountByIndex(containerSlotIndex);

            ItemData PlayerInventoryitem = PlayerInventory.GetItemByIndex(playerSlotIndex);
            int PlayerItemAmount = PlayerInventory.GetItemAmountByIndex(playerSlotIndex);

            if (ContainerItem == null && PlayerInventoryitem == null) return;
            else if (ContainerItem == null)
            {
                PlayerInventory.RemoveItemByIndex(playerSlotIndex, PlayerItemAmount);
                ContainerInventory.AddItemToIndex(containerSlotIndex, PlayerInventoryitem, PlayerItemAmount);
            }
            else if (PlayerInventoryitem == null)
            {
                ContainerInventory.RemoveItemByIndex(containerSlotIndex, ContainerItemAmount);
                PlayerInventory.AddItemToIndex(playerSlotIndex, ContainerItem, ContainerItemAmount);
            }
            else if(ContainerItem.id==PlayerInventoryitem.id)
            {
                int maxStack = ContainerItem.maxStack;
                if (targetSlotIndex > draggedSlotIndex)
                {
                   if((ContainerItemAmount + PlayerItemAmount) <=maxStack)
                   {
                        PlayerInventory.RemoveItemByIndex(playerSlotIndex, PlayerItemAmount);
                        ContainerInventory.AddItemToIndex(containerSlotIndex, PlayerInventoryitem, PlayerItemAmount);
                    }
                   else
                   {
                        int amountToMove = maxStack - ContainerItemAmount;
                        PlayerInventory.RemoveItemByIndex(playerSlotIndex, amountToMove);
                        ContainerInventory.AddItemToIndex(containerSlotIndex, PlayerInventoryitem, amountToMove);
                   }
                }
                else
                {
                    if ((ContainerItemAmount + PlayerItemAmount) <= maxStack)
                    {
                        ContainerInventory.RemoveItemByIndex(containerSlotIndex, ContainerItemAmount);
                        PlayerInventory.AddItemToIndex(playerSlotIndex, PlayerInventoryitem, ContainerItemAmount);
                    }
                    else
                    {
                        int amountToMove = maxStack - PlayerItemAmount;
                        ContainerInventory.RemoveItemByIndex(containerSlotIndex, amountToMove);
                        PlayerInventory.AddItemToIndex(playerSlotIndex, PlayerInventoryitem, amountToMove);
                    }
                }
            }
            else
            {
                PlayerInventory.RemoveItemByIndex(playerSlotIndex, PlayerItemAmount);
                ContainerInventory.RemoveItemByIndex(containerSlotIndex, ContainerItemAmount);
                PlayerInventory.AddItemToIndex(playerSlotIndex,ContainerItem, ContainerItemAmount);
                ContainerInventory.AddItemToIndex(containerSlotIndex, PlayerInventoryitem, PlayerItemAmount);
            }
        }
        else
        {
            Debug.Log("Swap between container");
            targetSlotIndex -= playerInventorySlotNumber;
            draggedSlotIndex -= playerInventorySlotNumber;
            ContainerInventory.SwapItems(targetSlotIndex, draggedSlotIndex);
        }
        UpdateSlotIconInformation();
    }

    public void CloseCanvasPanel()
    {
        inventoryUICanvas.SetActive(false);
    }

    public void OpenCanvasUI()
    {
        UpdateSlotIconInformation();
        inventoryUICanvas.SetActive(true);
        PlayerInventory = PlayerController.Instance.GetComponent<Inventory>();
        if (PlayerInventory == null) return;
        else
        {
            if (InventorySlotNumber != PlayerInventory.slotNumber) //player inventory slot is increased, spawn new slots.
            {
                int numberToSpawn = PlayerInventory.slotNumber - InventorySlotNumber;
                InventorySlotNumber += numberToSpawn;
                SpawnPlayerInventroySlot(numberToSpawn);
                AdjustContainerIndex();
                UpdateSlotIconInformation();
            }
            ResetQuickSlotButtonListener();
        }
    }

    private void ResetQuickSlotButtonListener()
    {
        if (PlayeritemSlots.Count == 0) return;
        for (int i = 0; i < 5; i++) //the first 5 slots are player quick slots
        {
            GameObject slot = PlayeritemSlots[i];
            int index = slot.GetComponent<InventoryUIInteract>().slotIndex;
            InventoryUIInteract inventoryUIInteract = slot.GetComponent<InventoryUIInteract>();
            inventoryUIInteract.playerInventoryInteract = this;
        }
    }

    public void SpawnPlayerInventroySlot(int number)
    {
        for (int i = 0; i < number; i++)
        {
            GameObject slot = Instantiate(itemSlotPrefab, GridLayoutPanel);
            slot.GetComponent<InventoryUIInteract>().slotIndex = i + 5;
            PlayeritemSlots.Add(slot);
            PlayericonList.Add(slot.GetComponentInChildren<Image>());
            PlayerItemAmountTextList.Add(slot.GetComponentInChildren<TMP_Text>());
            //add element to list to read and modify them though index
            InventoryUIInteract inventoryUIInteract = slot.GetComponent<InventoryUIInteract>();
            inventoryUIInteract.playerInventoryInteract = this;
            int index = InventorySlotNumber + i;

        }
    }
    public void UpdateSlotIconInformation()
    {
        UpdatePlayerInventoryUI();
        UpdateContainerInventory();
    }

    void UpdatePlayerInventoryUI()
    {
        for (int i = 0; i < PlayeritemSlots.Count; i++)
        {
            int inventoryIndex = i;
            Image icon = PlayericonList[i];
            TMP_Text amountText = PlayerItemAmountTextList[i];

            if (PlayerInventory.slots[inventoryIndex] == null)
            {
                icon.sprite = null;
                icon.enabled = false;
                amountText.text = "";
                amountText.enabled = false;
            }
            else
            {
                if (PlayerInventory.slots[inventoryIndex].amount != 0)
                {
                    InventorySlot slot = PlayerInventory.slots[inventoryIndex];
                    ItemData item = slot.item;
                    
                    if (item!=null)
                    {
                        Sprite iconResource = iconDatabase.GetIcon(item.id);
                        if(iconResource!=null)
                            icon.sprite = iconResource;
                        else
                        {
                            icon.sprite = iconDatabase.GetIcon(0);
                        }

                        icon.enabled = true;
                        amountText.text = "X" + slot.amount.ToString();
                        amountText.enabled = true;
                    }
                }
                else
                {
                    icon.sprite = null;
                    icon.enabled = false;
                    amountText.text = "";
                    amountText.enabled = false;
                }
            }

        }
    }

    void UpdateContainerInventory()
    {
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            int inventoryIndex = i;
            Image icon = ContainericonList[i];
            TMP_Text amountText = ContainerAmountTextList[i];
            if (ContainerInventory.slots[inventoryIndex] == null)
            {
                icon.sprite = null;
                icon.enabled = false;
                amountText.text = "";
                amountText.enabled = false;
            }
            else
            {
                if (ContainerInventory.slots[inventoryIndex].amount > 0)
                {
                    InventorySlot slot = ContainerInventory.slots[inventoryIndex];
                    ItemData item = slot.item;
                    if(item==null)
                    {
                        icon.sprite = null;
                        icon.enabled = false;
                        amountText.text = "";
                        amountText.enabled = false;
                        return;
                    }
                    Sprite iconResource = iconDatabase.GetIcon(item.id);
                    if (iconResource != null)
                    {
                        icon.sprite = iconResource;
                    }
                    else
                    {
                        icon.sprite = iconDatabase.GetIcon(0);
                    }
                    icon.enabled = true;
                    amountText.text = "X" + slot.amount.ToString();
                    amountText.enabled = true;
                }
                else
                {
                    icon.sprite = null;
                    icon.enabled = false;
                    amountText.text = "";
                    amountText.enabled = false;
                }
            }

        }
    }

    void AdjustContainerIndex()
    {
        for(int i=0; i< InventorySlots.Count;i++)
        {
            GameObject slot = InventorySlots[i];
            int index = InventorySlotNumber + i;
        }
    }

    void InitilizedInventorySlotUI()
    {
        int playerInventorySlotNumber = PlayerInventory.slotNumber;
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            GameObject slot = InventorySlots[i];

            ContainericonList.Add(slot.GetComponentInChildren<Image>());
            ContainerAmountTextList.Add(slot.GetComponentInChildren<TMP_Text>());

            int index = playerInventorySlotNumber+i;
            InventoryUIInteract inventoryUIInteract = slot.GetComponent<InventoryUIInteract>();
            if (inventoryUIInteract == null) Debug.Log("InventoryUIInteract is null");
            inventoryUIInteract.playerInventoryInteract = this;
            inventoryUIInteract.SetSlotIndex(index);
            //inventoryUIInteract.slotIndex = index;
            
        }
    }

    public int GetPlayerInventoryAmount()
    {
        return PlayerInventory.slotNumber;
    }

    public ItemData ReturnContainerItemDataByIndex( int index)
    {
        if (ContainerInventory.slots != null && ContainerInventory.slots[index]!=null)
        {
            return ContainerInventory.slots[index].item;
        }
        else
            return null;
    }

    public ItemData ReturnPlayerInventoryItemDataByIndex(int index)
    {
        if (PlayerInventory.slots != null && PlayerInventory.slots[index]!=null)
        {
            return PlayerInventory.slots[index].item;
        }
        else
            return null;
    }
}
