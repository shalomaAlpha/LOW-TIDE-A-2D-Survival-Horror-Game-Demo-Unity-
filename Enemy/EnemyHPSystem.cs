using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPSystem : MonoBehaviour
{
    [Header("Variables")]
    private int currentHP;
    public int MaxHP; 

    void Start()
    {
        currentHP = MaxHP;
    }

    // Update is called once per frame

    public void OnDamage(int damage)
    {
        currentHP -= damage;
        if(currentHP<=0)
        {
            onDead();
        }
    }

    public void onDead()
    {
        Destroy(gameObject);
    }
}
