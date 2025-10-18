using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotPointController : MonoBehaviour
{

    public Transform player; // 玩家角色
    float radius = 1f; // 准星与玩家的距离
    

    private void Update()
    {
        UpdatePivotPosition();
    }

    void UpdatePivotPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        Vector3 direction = (mousePosition - player.position).normalized;
        transform.position = player.position + direction * radius;
        transform.up = direction;
        DrawPivotPointDirection();

        if (mousePosition.x < player.position.x)
        {
           
        }
        else
        {

        }
    }

    void DrawPivotPointDirection()
    {
        Debug.DrawLine(transform.position, transform.position + transform.up * 1.5f, Color.blue);
        Debug.DrawLine(transform.position, transform.position + transform.right * 1.5f, Color.red);
    }
}

enum PlayerAttackDirection
{ 
    Left,
    Right,
    Up,
    Down,
}

