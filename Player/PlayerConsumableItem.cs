using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConsumableItem : MonoBehaviour
{
    public bool allowToUse;
    private Inventory playerInventory;
    private BuffComponent buffComponent;
    void Start()
    {
        playerInventory = GetComponent<Inventory>();
        buffComponent = GetComponent<BuffComponent>();
    }

    public void UseSupply(ItemData EquipedConsumable)
    {
        int ID = EquipedConsumable.id;
        switch (ID)
        {
            case 8:
                EatGrilledFish(EquipedConsumable);
                break;
            case 11:
                StopBleed(EquipedConsumable);
                break;
        }
    }

    void EatGrilledFish(ItemData EquipedConsumable)
    {
        if (buffComponent.CheckBuffSatietyCondition())
        {
            buffComponent.AddBuff(new Buff
            {
                name = "Satiety",
                buffType = BuffType.satiety,
                duration = 5,
                value = 2,
                effect = new SatietyEffect(),
                allowedStacking = true
            });
            playerInventory.RemoveItemByID(EquipedConsumable.id, 1);
            SFXManager.Instance.PlaySFX("Eat");
            Debug.Log("Removed item");
        }
        else
        {
            string content = "I can't eat anymore.";
            UIManager.Instance.DisplayBottomHint(2f,content);
        }
    }

    void StopBleed(ItemData EquipedConsumable)
    {
        if(buffComponent.CheckBuffExistOrNot(BuffType.bleed))
        {
            buffComponent.ClearBuff(BuffType.bleed);
            playerInventory.RemoveItemByID(EquipedConsumable.id, 1);

        }
    }
}
