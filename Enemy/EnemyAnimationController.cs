using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyAnimationController : MonoBehaviour
{
    public EnemyAIController AIController;
    public EnemyAIMovement enemyAIMovement;
    private Animator animator;
    private string currentAnimatonState;
    void Start()
    {
        animator= GetComponent<Animator>();
        animator.SetBool("Idle", true);
        currentAnimatonState = "Idle";
    }

    private void Update()
    {
        if(AIController!=null)
        {
            EnemyState currentState = AIController.enemyCurrentState;
            switch (currentState)
            { 
                case EnemyState.Idle:
                    SetAnimationKey("Idle");
                    break;
                case EnemyState.Chase:
                    SetAnimationKey("Move");
                    SetMoveDirection();
                    break;
                case EnemyState.Patrol:
                    SetAnimationKey("Move");
                    SetMoveDirection();
                    break;
                case EnemyState.Attack:
                    SetAttackDirection();
                    SetAnimationKey("Attack");
                    break;
                case EnemyState.Dead:
                    SetAnimationKey("Dead");
                    StartCoroutine(DeadAnimation());
                    break;
            }

        }
    }
    private void SetMoveDirection() 
    {
        Vector3 direction = enemyAIMovement.destination - transform.position;
        if (direction.x > 0)
        {
            animator.SetBool("MoveRight", true);
            animator.SetBool("MoveLeft", false);
        }
        else
        {
            animator.SetBool("MoveRight", false);
            animator.SetBool("MoveLeft", true);
        }
    }

    private void SetAnimationKey(string KeyName)
    {
        if (currentAnimatonState == KeyName) return;
        animator.SetBool (KeyName, true);
        animator.SetBool(currentAnimatonState, false);

        currentAnimatonState = KeyName;
        //record last key to be replaced next time
    }
    
    private void SetAttackDirection()
    {
        Vector3 playerPosition=PlayerController.Instance.transform.position;
        Vector3 direction = playerPosition - transform.position;
        if (direction.x > 0)
        {
            animator.SetBool("AttackRight", true);
            animator.SetBool("AttackLeft", false);
        }
        else
        {
            animator.SetBool("AttackRight", false);
            animator.SetBool("AttackLeft", true);
        }
    }

    IEnumerator DeadAnimation()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(AIController.gameObject);
    }
}
