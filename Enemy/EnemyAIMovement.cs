using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Handle the movement of AI
/// </summary>
public class EnemyAIMovement : MonoBehaviour
{
    Transform target;
    public bool isMoving;
    public Vector3 destination;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (!GameManager.Instance.EnemyAllowToMove) return;
        //Debug.Log(destination);
        agent.SetDestination(destination);
    }

    public void stopMoving()
    {
        target = null;
        isMoving = false;
        agent.isStopped = true;
    }

    public void MoveToPlayer()
    {
        target = PlayerController.Instance.transform;
        destination=target.position;
        isMoving = true;
        agent.isStopped = false;
    }

    public void MoveToPosition(Vector3 Destination)
    {
        destination = Destination;
        isMoving = true;
        agent.isStopped = false;
    }
    
}