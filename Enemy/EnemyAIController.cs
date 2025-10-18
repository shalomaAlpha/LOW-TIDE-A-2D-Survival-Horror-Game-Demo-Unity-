using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// This component control the Enemy Behaviour
/// 1. Set the movement target and call movement funciton
/// 
/// </summary>
public class EnemyAIController : MonoBehaviour
{
    [Header("Components")]
    public EnemyAIMovement enemyAIMovement;
    public EnemyAttack enemyAttack;

    [Header("Range Parameters")]
    public float attackRange;
    public float chaseRange;
    public LayerMask attackLayer;

    public EnemyState enemyCurrentState;
    bool targetIsPlayer;
    Collider2D targetCollider;

    Coroutine PartalCoroutine;
    

    void Start()
    {
        
    }

    void Update()
    {
        if (!GameManager.Instance.EnemyAllowToMove) return;
        if (enemyAttack.isAttacking) return;
        if (enemyCurrentState == EnemyState.Dead) return;
        if(CheckTargetWithinAttackRange())  //Check whelther obstacle or player within the attack range
        {
            stopPatrol();
            enemyCurrentState = EnemyState.Attack;
            Attack();
        }
        else
        {
            if(CheckPlayerWithChaseRange())
            {
                stopPatrol();
                enemyCurrentState = EnemyState.Chase;
                enemyAIMovement.MoveToPlayer();
            }
            else
            {
                if ((enemyCurrentState == EnemyState.Patrol || enemyCurrentState == EnemyState.Idle)
                    & PartalCoroutine!=null)
                    return;
                else
                {
                    PartalCoroutine=StartCoroutine(IdleThenRandomMove());
                }
            }
        }
    }

    bool CheckTargetWithinAttackRange()
    {
        if (!enemyAttack.isAttacking)
        {
            Collider2D[] PossibleTargets = Physics2D.OverlapCircleAll(transform.position, attackRange, attackLayer);
            foreach (var target in PossibleTargets)
            {
                if (target.CompareTag("Player") )
                {
                    //Debug.Log("Target is player");
                    targetCollider = target;
                    targetIsPlayer = true;
                    return true;
                }
                else if(target.CompareTag("Obstacle"))
                {
                    //Debug.Log("Target is obstacle");
                    targetCollider = target;
                    targetIsPlayer = false;
                    return true;
                }
            }
            return false;
        }
        else return false;
    } //used in the attack()
    void Attack()
    {
        switch (targetIsPlayer)
        {
            case true:
                enemyAttack.attackPlayer(targetCollider);
                enemyAIMovement.stopMoving();
                break;
            case false:
                int ObstacleHP = targetCollider.GetComponent<ObstacleController>().HP;
                if (targetCollider.isTrigger)
                {
                    enemyAIMovement.MoveToPlayer();
                }
                else if (ObstacleHP > 0)
                {
                    enemyAttack.attackObstacle(targetCollider);
                    enemyAIMovement.stopMoving();
                }
                else
                {
                    enemyAIMovement.MoveToPlayer();
                }
                break;
        }
    }

    bool CheckPlayerWithChaseRange()
    {
        Transform player=PlayerController.Instance.transform;
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if(distanceToPlayer>chaseRange)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator IdleThenRandomMove()
    {
        while (true)
        {
            enemyAIMovement.stopMoving();
            float waitTime = Random.Range(2f, 3f);
            enemyCurrentState= EnemyState.Idle;
            yield return new WaitForSeconds(waitTime);

            // Step 2:Get random Point
            Vector3 targetPosition = GetRandomPointInMoveArea();
            enemyCurrentState = EnemyState.Patrol;
            enemyAIMovement.MoveToPosition(targetPosition);

            // Step 3: Move 3-4 seconds no matter reach or not
            float moveDuration = Random.Range(3f, 4f);

            yield return new WaitForSeconds(moveDuration);
        }
    }
    void stopPatrol()
    {
        if(PartalCoroutine != null)
        {
            StopCoroutine(PartalCoroutine);
            PartalCoroutine = null;
        }
    }

    Vector3 GetRandomPointInMoveArea()
    {
        SpriteRenderer spriteRenderer=WalkableRegionLocator.Instance.GetComponent<SpriteRenderer>();
        
        Bounds bounds = spriteRenderer.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(x,y,0);
    }

    public void OnDeadEnemy()
    {
        enemyCurrentState=EnemyState.Dead;
    }

    

}
public enum EnemyState
{
    Idle,
    Patrol,
    Chase,
    Attack,
    Dead
}
