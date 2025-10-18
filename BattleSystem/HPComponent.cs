using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPComponent : MonoBehaviour
{
    public int maxHP=100;
    public int currentHP;

    public bool IsDead=false;

    private void Start()
    {
        currentHP = maxHP;
    }
    
    public void ReceiveDamage(int damageAmount)
    {
        currentHP -= damageAmount;
        //Debug.Log("ReceiveDamage"+damageAmount);
        CheckDeath();
    }

    public void Healing(int healingAmount)
    {
        currentHP += healingAmount;
        if (currentHP >= maxHP)
        {
            currentHP = maxHP;
        }
    }

    private void CheckDeath()
    {
        if (currentHP <= 0)
        {
            currentHP = 0;
            PlayerController playerController = GetComponent<PlayerController>();
            if(playerController != null)
            {
                playerController.OnPlayerDead();
            }
            else
            {
                EnemyAIController enemyAI=GetComponent<EnemyAIController>();
                enemyAI.OnDeadEnemy();
            }
            Debug.Log($"{gameObject.name} has died!");
            
        }
    }

    
}
