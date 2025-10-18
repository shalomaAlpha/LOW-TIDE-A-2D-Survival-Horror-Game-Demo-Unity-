using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    /// <summary>
    /// this conponent store the item in slots form
    /// SlotNumber should be set in advance, otherwise the initilization will casue error
    /// </summary>
    public int slotNumber; //SlotNumber should be set in advance
    public List<InventorySlot> slots = new List<InventorySlot>();
    private void Start()
    {
        for (int i = 0; i < slotNumber; i++)
        {
            slots.Add(new InventorySlot(null,0)); //initialize slot£¨item=null, amount=0£©
        }

        AddItemInadvance addItemComponent = GetComponent<AddItemInadvance>();
        if (addItemComponent != null)
            addItemComponent.AddItem();
    }
    
    public bool AddItem(ItemData item, int amount)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item != null ) 
            {
                if(slots[i].item.id == item.id && slots[i].amount < item.maxStack)
                {
                    int spaceLeft = item.maxStack - slots[i].amount;
                    int amountToAdd = Mathf.Min(amount, spaceLeft);

                    slots[i].AddAmount(amountToAdd);
                    amount -= amountToAdd;
                    if(i<5)
                    {
                        PlayerQuickSlot.Instance.SelectSlot(i);
                    }
                    //Debug.Log(item.name + "is added to slot" + i);
                    if (amount <= 0) return true; // add complete return function
                }
            }
        }

        // else find a empty slots
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item ==null ||slots[i].amount==0)  //empty slot id=0;
            {
                int amountToStore = Mathf.Min(amount, item.maxStack);
                slots[i].item=item;
                slots[i].amount = amount;
                amount -= amountToStore;
                //Debug.Log(item.name + "is added to slot" + i);
                if (amount <= 0)
                {
                    if (i < 5)
                    {
                        PlayerQuickSlot.Instance.SelectSlot(i);
                    }
                    return true;
                }  // add complete return function
            }
        }
        Debug.Log("Inventroy is full");
        return false; // slot is full 
    }

    public bool RemoveItemByID(int itemId, int amount)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item != null && slots[i].item.id == itemId)
            {
                if (slots[i].amount >= amount)
                {
                    slots[i].amount -= amount;
                    if(slots[i].amount==0) slots[i].item = null;

                    return true;
                }
                else
                {
                    amount -= slots[i].amount;
                    if(amount==0)
                        slots[i].item=null;

                    if (amount <= 0) return true;
                }
            }
        }
        return false; // not enough items
    }

    // swap item in the slot
    public void SwapItems(int slotIndex1, int slotIndex2)
    {
        if (slotIndex1 < 0 || slotIndex1 >= slots.Count || slotIndex2 < 0 || slotIndex2 >= slots.Count) return;
        else if(slots[slotIndex1].item!=null && slots[slotIndex2].item!=null)
        {
            if(slots[slotIndex1].item.id == slots[slotIndex2].item.id)
            {
                int amount1 = slots[slotIndex1].amount;
                int amount2 = slots[slotIndex2].amount;
                int maxStack = slots[slotIndex1].item.maxStack;
                if(amount1+amount2>maxStack)
                {
                    int valueChange = maxStack - amount2;
                    slots[slotIndex2].amount -= valueChange;
                    if (slots[slotIndex2].amount == 0) 
                        slots[slotIndex2].item = null;
                    slots[slotIndex1].amount += valueChange;
                    Debug.Log("Move" + valueChange + " to" + slotIndex2);
                    return;
                }
                else 
                {
                    int valueChange = amount2;
                    slots[slotIndex2].amount -= valueChange;
                    slots[slotIndex2].item = null;
                    slots[slotIndex1].amount += valueChange;
                    Debug.Log("Move" + valueChange +" "+ slotIndex1+ " to " + slotIndex2);
                    return;
                }
            }
            Debug.Log("Items are not same:" + slots[slotIndex1].item.id+" "+ slots[slotIndex2].item.id);
        }
        InventorySlot temp = slots[slotIndex1];
        slots[slotIndex1] = slots[slotIndex2];
        slots[slotIndex2] = temp;
    }

    #region Database SQL
    public int[] GetAllItemIDs()
    {
        List<int> itemIDs = new List<int>();

        foreach (InventorySlot slot in slots)
        {
            if (slot != null && slot.item != null)
            {
                itemIDs.Add(slot.item.id);
            }
        }

        return itemIDs.ToArray();
    }

    public int[] GetAllItemAmounts()
    {
        List<int> itemAmounts = new List<int>();

        foreach (InventorySlot slot in slots)
        {
            if (slot != null && slot.item != null)
            {
                itemAmounts.Add(slot.amount);
            }
        }

        return itemAmounts.ToArray();
    }

    public ItemData GetItemByIndex(int index)
    {
        if (slots[index] == null) return null;
        if (slots[index].item!= null) 
            return slots[index].item;
        else return null;
    }

    public int GetItemAmountByIndex(int index)
    {
        if (slots[index] == null) return 0;
        if (slots[index].item != null)
        {
            return slots[index].amount;
        }
           
        else return 0;
    }

    public void RemoveItemByIndex(int index, int amount)
    {
        slots[index].amount -= amount;
        if(slots[index].amount==0)
        {
            slots[index].item = null;
        }
    }

    public void AddItemToIndex(int index, ItemData item, int amount)
    {
        if(slots[index].item==null)
        {
            slots[index].item = item;
            slots[index].amount = amount;
        }
       else
        {
            slots[index].amount += amount;
        }
    }

    #endregion
}
