using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This component attached on the inventory element
/// to add items in advance when player enter the scene 
/// </summary>

[RequireComponent(typeof(Inventory))]
public class AddItemInadvance : MonoBehaviour
{
    [Header("Item To Add")]
    public int[] ItemIds;
    public int[] Amount;
    Inventory inventory;

    public void AddItem()
    {
        inventory = GetComponent<Inventory>();
        for (int i = 0; i < ItemIds.Length; i++)
        {
            addItemByID(ItemIds[i], Amount[i]);
        }
    }

    void addItemByID(int id, int amount)
    {
        ItemData itemToAdd = ItemDatabase.Instance.GetItemById(id);
        inventory.AddItem(itemToAdd, amount);
    }

}
