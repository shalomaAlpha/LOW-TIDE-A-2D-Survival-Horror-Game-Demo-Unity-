using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffComponent : MonoBehaviour
{
    public List<BuffInstance> activeBuffs = new List<BuffInstance>();
    private Dictionary<BuffInstance, Coroutine> activeCoroutines = new Dictionary<BuffInstance, Coroutine>(); 
    // store the buff coroutines control their duration

    public int satietyBuffCount;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) 
        {
            AddBuff(new Buff { name = "Satiety", buffType = BuffType.satiety, duration = 5, value = 2, effect = new SatietyEffect(), allowedStacking = true });
        }
    }
    public void AddBuff(Buff newBuff)
    {
        if(!newBuff.allowedStacking)
        {
            BuffInstance existingBuff = activeBuffs.Find(b => b.buffType == newBuff.buffType); 
            //find each item in the list that buffType == newBuff.buffType
            if (existingBuff != null)
            {
                existingBuff.duration = newBuff.duration; // reset the time
                return;
            }
        }
        //else if the newBuff is not allowed to stack
        BuffInstance buffInstance = new BuffInstance(newBuff);
        activeBuffs.Add(buffInstance);
        Coroutine buffCoroutine = StartCoroutine(HandleBuffDuration(buffInstance));
        activeCoroutines[buffInstance] = buffCoroutine; //record coroutine
    }

    private IEnumerator HandleBuffDuration(BuffInstance buff)
    {
        while (buff.duration > 0)
        {
            yield return new WaitForSeconds(buff.interval);
            buff.effect.ApplyEffect(gameObject, buff.value);  
            buff.duration -= buff.interval;
        }

        buff.effect.RemoveEffect(gameObject, buff.value); 
        activeBuffs.Remove(buff);
        activeCoroutines.Remove(buff);
        if(buff.buffType==BuffType.satiety)
        {
            satietyBuffCount--;
        }
    }

    public void ClearBuff(BuffType type) //can't remove the satiety buff
    {
        List<BuffInstance> buffsToRemove = activeBuffs.FindAll(b => b.buffType == type);

        foreach (BuffInstance buff in buffsToRemove)
        {
            if (activeCoroutines.ContainsKey(buff))
            {
                StopCoroutine(activeCoroutines[buff]); 
                activeCoroutines.Remove(buff); 
            }

            buff.effect.ClearEffect(gameObject, type); 
            activeBuffs.Remove(buff); 
        }
    }

    public bool CheckBuffSatietyCondition()
    {
        if (satietyBuffCount >= 3)
        {
            Debug.Log("Satiety reached the maximum, can't be added");
            return false;
        }
        else
        {
            satietyBuffCount++;
            return true;
        }
    }

    public bool CheckBuffExistOrNot(BuffType buffType)
    {
        foreach (BuffInstance buff in activeBuffs)
        {
            if(buff.buffType == buffType)
            { return true; }
        }
        return false;
    }
}
