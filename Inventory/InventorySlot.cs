using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemData item;   
    public int amount;  

    public InventorySlot(ItemData item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public void AddAmount(int changeValue) //amount change in the slot
    {
        amount += changeValue;
    }
}
