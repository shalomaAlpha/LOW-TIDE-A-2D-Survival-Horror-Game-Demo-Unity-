using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BeachSceneData
{
    public List<ResourceData> resourceData = new List<ResourceData>();
    public List<Vector3> resourceLocation = new List<Vector3>();
}


[System.Serializable]
public class ShelterSceneData
{
    public List<InventorySlot> FirePlaceInventorySlots = new List<InventorySlot>();
    public List<InventorySlot> ChestInventorySlots = new List<InventorySlot>();
    public List<InventorySlot> CraftTableInventorySlots = new List<InventorySlot>();

    public int FirePlaceSlotNumber;
    public int ChestSlotNumber;
    public int CraftTableSlotNumber;
}

