using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFishingComponent : MonoBehaviour
{
    public static PlayerFishingComponent Instance;
    public bool isInFishingZone = false;   // Check Whether player is in fishingZone
    public bool isFishing = false;         // Check whether player is fishing
    Inventory inventory;
    PlayerQuickSlot playerQuickSlot;
    private ItemDatabase itemDatabase;
    private Coroutine fishCoroutine;

    public float miniBiteTime;
    public float maxBiteTime;

    public bool allowToFish;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        inventory = GetComponent<Inventory>();
        playerQuickSlot = GetComponent<PlayerQuickSlot>();
    }
    private void Update()
    {
        if (!allowToFish) return;
        else
        {
            if (playerQuickSlot != null && playerQuickSlot.getCurrentSelectedItemData() != null)
            {
                if (playerQuickSlot.getCurrentSelectedItemData().id != 7) //fishing rod id is 7
                {
                    return;
                }

                if (isInFishingZone && !isFishing &&
                Input.GetKeyDown(KeyCode.F)
                )
                {
                    PlayerController.Instance.allowToMove = false;
                    PlayerAnimationManager.Instance.SetFishAnimation();
                    fishCoroutine=StartCoroutine(FishRoutine());
                }
                else if (isInFishingZone && isFishing
                    && Input.GetKeyDown(KeyCode.F)
                    ) //if player is fishing, end fishing 
                {
                    PlayerController.Instance.allowToMove = true;
                    StopCoroutine(fishCoroutine);
                    PlayerAnimationManager.Instance.EndFishAnimation();
                }
            }
        }
    }

    private IEnumerator FishRoutine()
    {
        isFishing = true;
        SFXfishing.Instance.PlayClip(0);
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ModifyGuideText("When the fish bite the hook, Press [Space] to pull up the fish");
        }
        yield return new WaitForSeconds(1f);
        SFXfishing.Instance.PlayClip(1);
        // Randomly wait 10-15 seconds fish bite the hook
        float biteTime = Random.Range(miniBiteTime, maxBiteTime);
        yield return new WaitForSeconds(biteTime);
        SFXfishing.Instance.PlayClip(2);
        UIManager.Instance.ModifyGuideText("The fish has hooked, Press the [Space] to pull up");
        PlayerAnimationManager.Instance.animatior.SetBool("FishHooked", true);
        while (!Input.GetKeyDown(KeyCode.Space))  
        {
            yield return null;
            
        }
        SFXfishing.Instance.PlayClip(3);
        // Choose a fish randomely
        ItemData caughtFish = GetRandomFish();
        inventory.AddItem(caughtFish,1);
        PlayerAnimationManager.Instance.animatior.SetBool("FishCatch", true);
        yield return new WaitForSeconds(1f);
        SFXfishing.Instance.StopPlay();
        PlayerController.Instance.allowToMove = true;
        PlayerAnimationManager.Instance.EndFishAnimation();
        yield return new WaitForSeconds(1f);
        {
            UIManager.Instance.ModifyGuideText("Equip Fishing rod and Press [F] to fish");
        }
        isFishing = false;

    }

    private ItemData GetRandomFish()
    {
        if(itemDatabase==null)
        {
            itemDatabase = FindObjectOfType<ItemDatabase>();
        }
        int randomFishID = Random.Range(1, 5);
        ItemData fish = itemDatabase.GetItemById(randomFishID);
        return fish;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FishingZone"))
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.AllowToDisplayOtherHint = false;
                UIManager.Instance.ModifyGuideText("Equip Fishing rod and Press [F] to fish");
            }
            isInFishingZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("FishingZone"))
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.AllowToDisplayOtherHint = true;
                UIManager.Instance.ModifyGuideText("");
            }
            isInFishingZone = false;
        }
    }

    public void QuitFishing()
    {
        isFishing = false;
        PlayerController.Instance.allowToMove = true;
        StopCoroutine(FishRoutine());
    }
}
