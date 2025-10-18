using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// This component handle the player inventory interact in normal condition
/// Player press the tap to open the bag panel
/// </summary>
public class PlayerInventoryUI : MonoBehaviour
{
    public static PlayerInventoryUI Instance;
    public IconDatabase iconDatabase;
    public GameObject inventoryUI;
    public GameObject itemSlotPrefab;
    public Transform itemSlotParent;
    public List<GameObject> itemSlots; 

    List<Image> iconList = new List<Image>();
    List<TMP_Text> AmountTextList = new List<TMP_Text>();

    int PlayerInventoryMaxSlots;
    Inventory PlayerInventory;
    public bool allowToOpenInventory=true;
    public bool isOpen=false;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        inventoryUI.SetActive(false);
        PlayerInventory = PlayerController.Instance.GetComponent<Inventory>();
        PlayerInventoryMaxSlots = PlayerInventory.slotNumber;
        foreach(GameObject slot in itemSlots)
        {
            iconList.Add(slot.GetComponentInChildren<Image>());
            AmountTextList.Add(slot.GetComponentInChildren<TMP_Text>());

            int index = slot.GetComponent<InventoryUIInteract>().slotIndex;
            Button slotButton = slot.GetComponent<Button>();
            //slotButton.onClick.AddListener(() => OnSlotClicked(index));
        }
        SpawnPlayerInventroySlot(5);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && allowToOpenInventory)
        {
            restQuickSlotsListener();
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            if (inventoryUI.activeSelf)
            {
                SFXManager.Instance.PlaySFX("OpenBag");
                isOpen = true;
                UpdateInventoryUI();
                ItemToolTip.Instance.SetVisibility(true);
                GameManager.Instance.FreezePlayerMovementAndInteract(true);
            }
            else
            {
                isOpen = false;
                SFXManager.Instance.PlaySFX("CloseBag");
                ItemToolTip.Instance.SetVisibility(false);
                GameManager.Instance.FreezePlayerMovementAndInteract(false);
            }
        }
    }

    public void restQuickSlotsListener()
    {
        if (itemSlots.Count == 0) return;
        for (int i = 0; i < 5; i++) //the first 5 slots are player quick slots
        {
            GameObject slot = itemSlots[i];
            int index = slot.GetComponent<InventoryUIInteract>().slotIndex;
            Button slotButton = slot.GetComponent<Button>();
            slotButton.onClick.RemoveAllListeners();
        }
    }

    public void UpdateInventoryUI()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            int inventoryIndex = i;
            Image icon = iconList[i];
            TMP_Text amountText = AmountTextList[i];
            if (PlayerInventory.slots[inventoryIndex] == null)
            {
                icon.sprite = null;
                icon.enabled = false;
                amountText.text = "";
                amountText.enabled = false;
            }
            else
            {
                if (
                    PlayerInventory.slots[inventoryIndex].amount!=0)
                {
                    InventorySlot slot = PlayerInventory.slots[inventoryIndex];
                    ItemData item = slot.item;
                    if(item!=null)
                    {
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
                }
                else
                {
                    //Debug.Log("Slot " + i + " is empty");
                    // if the slot is empty
                    icon.sprite = null;
                    icon.enabled = false;
                    amountText.text = "";
                    amountText.enabled = false;
                }
            }
           
        }
    }
    public void SpawnPlayerInventroySlot(int number)
    {
        for(int i=0;i<number;i++)
        {
            GameObject slot = Instantiate(itemSlotPrefab, itemSlotParent);
            slot.GetComponent<InventoryUIInteract>().slotIndex = i + 5;
            itemSlots.Add(slot);

            iconList.Add(slot.GetComponentInChildren<Image>());
            AmountTextList.Add(slot.GetComponentInChildren<TMP_Text>());
        }
    }

    public void SwapItems(int index1, int index2)
    {

        if (index1 < 0 || index1 >= PlayerInventoryMaxSlots ||
       index2 < 0 || index2 >= PlayerInventoryMaxSlots)
        {
            Debug.LogError("Invalid slot index for swapping");
            return;
        }

        if (index1 != index2) 
        {
            PlayerInventory.SwapItems(index1, index2);
        }
        
        UpdateInventoryUI();
    }

    public ItemData returnItemDataByIndex(int index)
    {
        if (PlayerInventory.slots[index] != null)
            return PlayerInventory.slots[index].item;
        else
            return null;
    }

    public Inventory GetPlayerInventory()
    {
        return PlayerInventory;
    }

    public int GetPlayerSlotAmount()
    {
        return PlayerInventory.slotNumber;
    }

    public void OpenInventoryPanel()
    {
        if(allowToOpenInventory)
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            if (inventoryUI.activeSelf)
            {
                isOpen = true;
                UpdateInventoryUI();
                ItemToolTip.Instance.SetVisibility(true);
                GameManager.Instance.FreezePlayerMovementAndInteract(true);
            }
            else
            {
                isOpen = false;
                ItemToolTip.Instance.SetVisibility(false);
                GameManager.Instance.FreezePlayerMovementAndInteract(false);
            }
        }
    }
}
