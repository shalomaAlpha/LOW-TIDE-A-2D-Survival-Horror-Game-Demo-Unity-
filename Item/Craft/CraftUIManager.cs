using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


/// <summary>
/// Current function is unknown
/// </summary>
public class CraftUIManager : MonoBehaviour
{
    public static CraftUIManager Instance;
    public IconDatabase iconDatabase;
    public GameObject inventoryUI;
    public GameObject itemSlotPrefab;
    public Transform itemSlotParent;
    public List<GameObject> itemSlots;

    public List<Image> iconList = new List<Image>();
    public List<TMP_Text> AmountTextList = new List<TMP_Text>();

    int InventorySlotNumber=5;  //quick slots are already exisit
    Inventory PlayerInventory;
    private int selectedSlotIndex = -1;

    bool Initilized = false;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        inventoryUI.SetActive(false);
    }
    private void Update()
    {
        if (Initilized) return;
        if(PlayerInventory==null)
        {
            PlayerInventory = PlayerController.Instance.GetComponent<Inventory>();
            Debug.Log("GetPlayerInventory");
        }
        else
        {
            Initilized = true;
            PlayerInventoryUI playerInventoryUI = PlayerInventoryUI.Instance;
            for(int i=0;i<5;i++)
            {
                GameObject slot = playerInventoryUI.itemSlots[i];
                itemSlots.Add(slot);
                iconList.Add(slot.GetComponentInChildren<Image>());
                AmountTextList.Add(slot.GetComponentInChildren<TMP_Text>());

                int index = slot.GetComponent<InventoryUIInteract>().slotIndex;
                Button slotButton = slot.GetComponent<Button>();
                slotButton.onClick.AddListener(() => OnSlotClicked(index));
            }
        }
    }

    public void OpenCraftUI()
    {
        UpdateInventoryUI();
        inventoryUI.SetActive(true);
        PlayerInventory = PlayerController.Instance.GetComponent<Inventory>();
        if (PlayerInventory == null) return;
        else
        {
            if(InventorySlotNumber != PlayerInventory.slotNumber) //player inventory slot is increased, spawn new slots.
            {
                int numberToSpawn = PlayerInventory.slotNumber - InventorySlotNumber;
                InventorySlotNumber += numberToSpawn;
                SpawnPlayerInventroySlot(numberToSpawn);
                UpdateInventoryUI();
            }
            ResetQuickSlotButtonListener();
        }
    }

    private void ResetQuickSlotButtonListener()
    {
        if (itemSlots.Count == 0) return;
        for(int i=0;i<5;i++) //the first 5 slots are player quick slots
        {
            GameObject slot = itemSlots[i];
            int index = slot.GetComponent<InventoryUIInteract>().slotIndex;
            Button slotButton = slot.GetComponent<Button>();
            slotButton.onClick.RemoveAllListeners();
            slotButton.onClick.AddListener(() => OnSlotClicked(index));
        }
        Debug.Log("SetNewListener");
    }

    public void UpdateInventoryUI()
    {
        Debug.Log("Update called");
        for (int i = 0; i < itemSlots.Count; i++)
        {
            int inventoryIndex = i;
            Image icon = iconList[i];
            TMP_Text amountText = AmountTextList[i];
            if (PlayerInventory.slots == null)
            {
                Debug.Log("PlayerInventory.slots IS NOT VAILD");
            }
            if(PlayerInventory.slots[inventoryIndex]==null)
            {
                Debug.Log("PlayerInventory.slots[inventoryIndex] IS NOT VAILD");
                //Debug.Log("Index " + i + "Amount " + PlayerInventory.slots[inventoryIndex].amount);
            }
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
        for (int i = 0; i < number; i++)
        {
            GameObject slot = Instantiate(itemSlotPrefab, itemSlotParent);
            slot.GetComponent<InventoryUIInteract>().slotIndex = i + 5;
            itemSlots.Add(slot);
            iconList.Add(slot.GetComponentInChildren<Image>());
            AmountTextList.Add(slot.GetComponentInChildren<TMP_Text>());
            //add element to list to read and modify them though index

            int index = InventorySlotNumber + i;
            Button slotButton = slot.GetComponent<Button>();
            slotButton.onClick.AddListener(() => OnSlotClicked(index));
            //set interactive index
        }
    }

    private void OnSlotClicked(int index)
    {
        int inventoryIndex = index;
        if (selectedSlotIndex == -1)
        {
            selectedSlotIndex = inventoryIndex;
        }
        else
        {
            if (selectedSlotIndex != inventoryIndex)
            {
                PlayerInventory.SwapItems(selectedSlotIndex, inventoryIndex);
            }
            else
            {
                Debug.Log("Canceled selection of slot: " + inventoryIndex);
            }
            selectedSlotIndex = -1;
        }
    }

}
