using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// Handle the attack logic of enemy
/// </summary>
public class EnemyAttack : MonoBehaviour
{
    public float attackRange=1.5f;
    public float attackPreDelay;    
    public float attackPostDelay;   
    public int attackDamage;

    public bool isAttacking = false;

    private Coroutine AttackCoroutine;

    public void attackPlayer(Collider2D target)
    {
        if (AttackCoroutine == null)
        {
            HPComponent playerHPComponent = target.GetComponent<HPComponent>();
            if (playerHPComponent == null) Debug.Log("HPcomponent is null");
            AttackCoroutine = StartCoroutine(AttackRoutinePlayer(playerHPComponent));    
        }
        
    }

    public void attackObstacle(Collider2D target)
    {
        if (AttackCoroutine == null)
        {
            AttackCoroutine = StartCoroutine(AttackRoutineObstacle
                (target.GetComponent<ObstacleController>()));
        }
        
    }

    public IEnumerator AttackRoutinePlayer(HPComponent targetHP)
    {
        Debug.Log("Start to attack player");
        isAttacking = true;
        yield return new WaitForSeconds(attackPreDelay);

        if (targetHP != null && targetHP.currentHP > 0)
        {
            float distance = Vector2.Distance(transform.position, targetHP.transform.position);
            //Debug.Log("Distance: " + distance);
            if (distance <= attackRange)
            {
                targetHP.ReceiveDamage(attackDamage);
            }
        }
        yield return new WaitForSeconds(attackPostDelay);
        isAttacking = false;

        AttackCoroutine = null;
    }

    public IEnumerator AttackRoutineObstacle(ObstacleController obstacleController)
    {
        Debug.Log("Start to attack Obstacle");
        isAttacking = true;
        yield return new WaitForSeconds(attackPreDelay);

        if (obstacleController != null && obstacleController.HP > 0)
        {
            float distance = Vector2.Distance(transform.position, obstacleController.transform.position);
            Debug.Log("Distance: " + distance+" AttackRange: "+attackRange);
            if (distance <= attackRange)
            {
                obstacleController.ObstacleDamaged(attackDamage);
            }
        }
        yield return new WaitForSeconds(attackPostDelay);
        Debug.Log("Attacked Obstacle");
        isAttacking = false;

        AttackCoroutine = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}


