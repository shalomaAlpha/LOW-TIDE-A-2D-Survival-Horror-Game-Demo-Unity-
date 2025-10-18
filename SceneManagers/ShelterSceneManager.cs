using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This manager handle below functions:
/// 1. teleport player player to spawn point
/// </summary>
public class ShelterSceneManager : MonoBehaviour
{
    public static ShelterSceneManager Instance;
    GameObject player;
    public Transform sceneStartPoint;

    [Header("Inventory")]
    public Inventory chestInventory;
    public Inventory fireplaceInventory;
    public Inventory craftTableInventory;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        player = PlayerController.Instance.gameObject;
        if(player!=null)
        {
            player.transform.position = sceneStartPoint.position;
        }
    } 
    
    public ShelterSceneData StoreInventoryData()
    {
        ShelterSceneData sceneData = new ShelterSceneData();
        {
            foreach(InventorySlot slot in chestInventory.slots)
            {
                sceneData.ChestInventorySlots.Add(slot);
            }
            sceneData.ChestSlotNumber = chestInventory.slotNumber;

            foreach(InventorySlot slot in fireplaceInventory.slots)
            {
                sceneData.FirePlaceInventorySlots.Add(slot);
            }
            sceneData.FirePlaceSlotNumber = fireplaceInventory.slotNumber;

            foreach (InventorySlot slot in craftTableInventory.slots)
            {
                sceneData.CraftTableInventorySlots.Add(slot);
            }
            sceneData.CraftTableSlotNumber = craftTableInventory.slotNumber;
        }

        return sceneData;
    } //called in the sceneDatabase

    public void ResetShelterInventory()
    {
        ShelterSceneData sceneData = SceneDatabase.Instance.GetShelterData();
        if (sceneData == null)
        {
            Debug.Log("SceneData is null"); return;
        }

        chestInventory.slots.Clear();
        chestInventory.slotNumber = sceneData.ChestSlotNumber;
        foreach (var slot in sceneData.ChestInventorySlots)
        {
            chestInventory.slots.Add(new InventorySlot(slot.item, slot.amount));
        }

        fireplaceInventory.slots.Clear();
        fireplaceInventory.slotNumber = sceneData.FirePlaceSlotNumber;
        foreach (var slot in sceneData.FirePlaceInventorySlots)
        {
            fireplaceInventory.slots.Add(new InventorySlot(slot.item, slot.amount));
        }

        craftTableInventory.slots.Clear();
        craftTableInventory.slotNumber = sceneData.CraftTableSlotNumber;
        foreach (var slot in sceneData.CraftTableInventorySlots)
        {
            craftTableInventory.slots.Add(new InventorySlot(slot.item, slot.amount));
        }

    }
}
