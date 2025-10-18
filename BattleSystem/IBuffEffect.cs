using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffEffect
{
    void ApplyEffect(GameObject target,int value); 
    void RemoveEffect(GameObject target, int value); //used when buff duration end
    void ClearEffect(GameObject target, BuffType type); //clear buff immediately
}

public class BleedEffect : IBuffEffect
{
    public void ApplyEffect(GameObject target,int value)
    {
        HPComponent hPComponent = target.GetComponent<HPComponent>();
        hPComponent.ReceiveDamage(value);
    }
    public void RemoveEffect(GameObject target,int value)
    {
        Debug.Log("Buff Bleed end"); //futural end elements could be added 
    }
    public void ClearEffect(GameObject target,BuffType type)
    {
        BuffComponent buffComponent = target.GetComponent<BuffComponent>();
        buffComponent.ClearBuff(type); 
    }
}

public class SatietyEffect : IBuffEffect
{
    public void ApplyEffect(GameObject target, int value)
    {
        HPComponent hPComponent = target.GetComponent<HPComponent>();
        hPComponent.Healing(value);
    }
    public void RemoveEffect(GameObject target, int value)
    {
        Debug.Log("Satiety Buff end");

    }
    public void ClearEffect(GameObject target, BuffType type)
    {
        BuffComponent buffComponent = target.GetComponent<BuffComponent>();
        buffComponent.ClearBuff(type);
    }
}






