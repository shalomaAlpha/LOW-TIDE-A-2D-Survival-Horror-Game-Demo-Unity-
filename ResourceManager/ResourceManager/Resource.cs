using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

/// <summary>
/// Spawn by the resourceManager
/// </summary>
public class Resource : MonoBehaviour
{
    private bool isPlayerNearby=false;
    public int itemID;
    public int itemAmount;
    public IconDatabase database;
    
    public void SetPlayerNearby(bool nearby)
    {
        isPlayerNearby = nearby;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (isPlayerNearby)
        {
            UIManager.Instance.ModifyGuideText("[E] to loot");
            sr.color = Color.yellow;
        }
        else
        {
            sr.color = Color.white;
        }
    }

    public void InitilizeResourceInManager(ResourceData resourceData) //should manually initilise
    {
        itemID = resourceData.itemID;
        itemAmount=resourceData.amount;

        ItemData itemData = ItemDatabase.Instance.GetItemById(itemID);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite sprite = database.GetIcon(itemID);
        spriteRenderer.sprite = sprite;

        Vector3 position = gameObject.transform.position;
        ResourceManager.Instance.regisiterResourceInfo(this, position);
    }

    public void InitiliseResourceInSpawner()
    {
        ItemData itemData = ItemDatabase.Instance.GetItemById(itemID);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite sprite = database.GetIcon(itemID);

        spriteRenderer.sprite = sprite;
        Vector3 position = gameObject.transform.position;
        ResourceManager.Instance.regisiterResourceInfo(this, position);
    }

    public void lootReousrce(GameObject player)
    {
        Inventory playerInventory=player.GetComponent<Inventory>();
        ItemData item = ItemDatabase.Instance.GetItemById(itemID);
        playerInventory.AddItem(item, itemAmount);

        Vector3 location=gameObject.transform.position;
        ResourceManager.Instance.removeResourceInfo(this, location);
        SFXManager.Instance.PlaySFX("SFX_Pickup");

        Destroy(gameObject);
    }
}
