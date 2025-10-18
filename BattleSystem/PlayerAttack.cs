using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public WeaponData currentWeapon;
    public Transform pivotPoint;

    public GameObject thrownWeaponPrefab; 
    public float throwForce = 10f;
    public Transform throwPoint;

    public LayerMask enemyLayer;
    private bool isAttacking;
    private float attackAngle;
    private PlayerController playerController;

    public LineRenderer attackArcRenderer;
    int arcSegments = 30;

    public bool allowToAttack=true;
    private void Start()
    {
        currentWeapon = null;
        playerController = GetComponent<PlayerController>();    
    }

    public void Attack(ItemData weaponItem)
    {
        if(weaponItem != null && !isAttacking)
        {
            currentWeapon = weaponItem as WeaponData;
            switch (currentWeapon.attackType)
            {
                case WeaponAttackType.Swing:
                    attackAngle = 120f;
                    MeleeAttackSwing();
                    break;
            }
        }
    }

    void MeleeAttackSwing()
    {
        isAttacking = true;
        Debug.Log("Attacking with: " + currentWeapon.name);
        StartCoroutine(MeleeAttack());
    }
    IEnumerator MeleeAttack()
    {
        playerController.allowToMove = false;
        PlayerAnimationManager.Instance.StartAttackAnimation();
        yield return new WaitForSeconds(currentWeapon.windUp);
        SFXManager.Instance.PlaySFX("Swing");
        PerformAttack();
        DrawAttackArc();
        yield return new WaitForSeconds(currentWeapon.recovery);
        attackArcRenderer.positionCount = 0;
        PlayerAnimationManager.Instance.EndAttackAnimation();
        isAttacking = false;
        playerController.allowToMove = true;
    }

    private void PerformAttack()
    {
        Vector3 playerPos = transform.position;
        Vector3 aimDirection = (pivotPoint.position - playerPos).normalized;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(playerPos, currentWeapon.range, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            Vector2 targetDirection = (enemy.transform.position - playerPos).normalized;
            float angleBetween = Vector2.Angle(aimDirection, targetDirection);
            Debug.Log("Hit enemy: " + enemy.name);
            SFXManager.Instance.PlaySFX("Punch");
            if (angleBetween <= attackAngle / 2)
            {
                HPComponent hPComponent = enemy.GetComponent<HPComponent>();
                hPComponent.ReceiveDamage(currentWeapon.damage);
            }

            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float knockbackForce = 40f;
                rb.AddForce(targetDirection * knockbackForce, ForceMode2D.Impulse);
                CameraController.Instance.Shake();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (currentWeapon == null) return;
        Gizmos.color = isAttacking ? Color.red : Color.white;
        Vector3 playerPos = transform.position;
        Vector3 aimDirection = (pivotPoint.position - playerPos).normalized;

        
        DrawSector(playerPos, aimDirection, currentWeapon.range, attackAngle);
    }

    private void DrawSector(Vector2 origin, Vector2 direction, float radius, float angle)
    {
        int segments = 20; 
        float halfAngle = angle / 2f;
        Vector2 startDir = Quaternion.Euler(0, 0, -halfAngle) * direction;
        Vector2 endDir = Quaternion.Euler(0, 0, halfAngle) * direction;

        Gizmos.DrawLine(origin, origin + startDir * radius);
        Gizmos.DrawLine(origin, origin + endDir * radius);

        for (int i = 0; i < segments; i++)
        {
            float step = angle / segments;
            Vector2 from = Quaternion.Euler(0, 0, -halfAngle + step * i) * direction * radius;
            Vector2 to = Quaternion.Euler(0, 0, -halfAngle + step * (i + 1)) * direction * radius;
            Gizmos.DrawLine(origin + from, origin + to);
        }
    }

    void DrawAttackArc()
    {
        if (attackArcRenderer == null) return;

        Vector3 playerPos = transform.position;
        Vector3 aimDirection = (pivotPoint.position - playerPos).normalized;
        float halfAngle = attackAngle / 2f;

        attackArcRenderer.positionCount = arcSegments + 2; 

        attackArcRenderer.SetPosition(0, playerPos);

        for (int i = 0; i <= arcSegments; i++)
        {
            float angle = -halfAngle + (attackAngle * i / arcSegments);
            Vector3 dir = Quaternion.Euler(0, 0, angle) * aimDirection;
            Vector3 point = playerPos + dir * currentWeapon.range;
            attackArcRenderer.SetPosition(i + 1, point);
        }
    }
}
