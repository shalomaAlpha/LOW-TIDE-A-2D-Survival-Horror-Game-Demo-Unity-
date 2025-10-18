using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager : MonoBehaviour
{
    public static CraftManager Instance;
    public RecipeBook recipeBook;
    public GameObject interactUI;

    Inventory craftTableInventory;
    PlayerInventoryInteract playerInventoryInteract;
    bool allowToCraft;
    bool isOpen;

    private void Awake()
    {
        Instance = this;
    }
    public void Start()
    {
        craftTableInventory= GetComponent<Inventory>();
        playerInventoryInteract = GetComponent<PlayerInventoryInteract>();
        interactUI.SetActive(false);
        isOpen = false;
    }
    //availableItemID should be all Items ID in current player Inventory 

    public bool CanCraft(int[] availableItemIDs, int[] availableAmounts, Recipe recipe)
    {

        for (int i = 0; i < recipe.inputItemsID.Length; i++)
        {
            //check each required ingredient ID and Amount 
            int requiredItemID = recipe.inputItemsID[i];
            int requiredAmount = recipe.inputAmounts[i];

            int totalAmount = 0;

            for (int j = 0; j < availableItemIDs.Length; j++)
            {
                if (availableItemIDs[j] == requiredItemID)
                {
                    totalAmount += availableAmounts[j];
                    if (totalAmount >= requiredAmount) break;
                    //Ingredient Amount is adequant break the loop
                }
            }

            if (totalAmount < requiredAmount)
            {
                return false; 
            }
        }
        return true;   //Ingredient Amount is adequant return true
    }
    public void ClearCraftingSlots()
    {
        craftTableInventory.slots.Clear();
        Debug.Log("Crafting slots cleared.");
    }

    private void Update()
    {
        if (!allowToCraft) return;
        else
        {
            if(Input.GetKeyDown(KeyCode.E)|| Input.GetKeyDown(KeyCode.Tab))
            {
                if(!isOpen)
                {
                    isOpen = true;
                    ItemToolTip.Instance.SetVisibility(true);
                    GameManager.Instance.FreezePlayerMovementAndInteract(true);
                    playerInventoryInteract.OpenCanvasUI();
                    CraftRecipeDisplay.Instance.UpdateCraftPanelRecipes();
                }
                else
                {
                    playerInventoryInteract.CloseCanvasPanel();
                    ItemToolTip.Instance.SetVisibility(false);
                    GameManager.Instance.FreezePlayerMovementAndInteract(false);
                    isOpen = false;
                }
            }
        }
    }
    public bool Craft(Inventory inventory, Recipe recipe)
    {
        for (int i = 0; i < recipe.inputItemsID.Length; i++)
        {
            int requiredItemID = recipe.inputItemsID[i];
            int requiredAmount = recipe.inputAmounts[i];

            inventory.RemoveItemByID(requiredItemID, requiredAmount);
        }
        ItemData craftedItem = ItemDatabase.Instance.GetItemById(recipe.outputItemID);
        // Add new Item
        craftTableInventory.AddItem(craftedItem, recipe.outputAmount);
        playerInventoryInteract.UpdateSlotIconInformation();
        Debug.Log($"Crafted {recipe.outputAmount}x {recipe.outputItemID}");
        return true;

        
    }

    private bool HasSpaceFor(ItemData item, int amountToAdd)
    {
        foreach (var slot in craftTableInventory.slots)
        {
            if (slot == null)
            {
                Debug.Log("slot is null");
                return true;
            }
            else if (slot.item == null) 
            {
                return true;
            }
            else if (slot.item.id == item.id && slot.amount < item.maxStack)
            {
                int remainingSpace = item.maxStack - slot.amount;
                if (remainingSpace >= amountToAdd)
                    return true;
            }
            Debug.Log("Current Slot is full");
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            allowToCraft = true;
            interactUI.SetActive(true);
            PlayerInventoryUI.Instance.allowToOpenInventory = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            allowToCraft = false;
            interactUI.SetActive(false);
            PlayerInventoryUI.Instance.allowToOpenInventory = true;
        }
    }

    public bool CanCreateOrNot(Recipe recipe) //new custom craft condition check function
    {
        
        Inventory playerInventory= PlayerController.Instance.GetComponent<Inventory>();
        int[] availableItemIDs = playerInventory.GetAllItemIDs();
        int[] availableAmounts = playerInventory.GetAllItemAmounts();
        ItemData outputItem = ItemDatabase.Instance.GetItemById(recipe.outputItemID);
        if (outputItem == null) Debug.Log("Return Item is null");

        if (!HasSpaceFor(outputItem, recipe.outputAmount))
        {
            string content = "Crafting slots full or no stack space available.";
            CraftHintText.Instance.PlayHintAnimation(content, 1.5f);
            return false;
        }

        if (CanCraft(availableItemIDs, availableAmounts, recipe))
        {
            return true;
        }
        else
        {
            string content = "lacking material, unable to craft";
            CraftHintText.Instance.PlayHintAnimation(content, 1.5f);
            return false;
        }
    }
}
