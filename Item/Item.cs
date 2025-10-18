using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public int id;              
    public string name;         
    public string description;  
    public int maxStack;
    public ItemType type;

    public ItemData(int id, string name, string description, int maxStack, ItemType type)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.maxStack = maxStack;
        this.type = type;
    }
}

public enum ItemType
{
    Material,
    Weapon,
    Consumable,
    Note,
    Tool
}

[System.Serializable]
public class WeaponData : ItemData
{
    public int damage;
    public float windUp;
    public float recovery;
    public float range;
    public WeaponAttackType attackType;

    public WeaponData(int id, string name, string description, 
        int maxStack, int damage, float windUp,float recovery, float range, WeaponAttackType attackType)
        : base(id, name, description, maxStack, ItemType.Weapon)  
    {
        this.damage = damage;
        this.windUp = windUp;
        this.recovery = recovery;
        this.range = range;
        this.attackType = attackType;
    }
}

[System.Serializable]
public enum WeaponAttackType
{
    Swing,    // For weapons that are swung (e.g., axes, swords)
    Stab,     // For weapons that are stabbed (e.g., spears, daggers)
    Ranged,    // For ranged weapons (e.g., bows, guns)
}
