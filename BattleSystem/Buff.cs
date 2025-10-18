using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Buff 
{
    public string name;
    public BuffType buffType;
    public int duration; 
    public int interval = 1; // settlement interval
    public int value; // buff effect value
                      // for instance, bleed 2 DMG/s
    public bool allowedStacking;
    public IBuffEffect effect; // buff effect
}

[System.Serializable]
public enum BuffType
{
    bleed,
    hemorrhage,
    satiety,
    Regeneration,
    fracture,
    posion
}

public class BuffInstance
{
    public BuffType buffType;
    public int duration;
    public int interval;
    public int value;
    public bool allowedStacking;
    public IBuffEffect effect;

    public BuffInstance(Buff buff)
    {
        this.buffType = buff.buffType;
        this.duration = buff.duration;
        this.interval = buff.interval;
        this.value = buff.value;
        this.allowedStacking = buff.allowedStacking;
        this.effect = buff.effect;
    }
}

