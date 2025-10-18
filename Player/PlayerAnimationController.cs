using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// currently abandon
/// </summary>
public class PlayerAnimationController : MonoBehaviour
{
    private PlayerAttack playerAttack;
    public GameObject WeaponToSwing;
    private void Awake()
    {
        WeaponToSwing.SetActive(false);
    }
    private void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
    }

    public void SwitchAnimation()
    {
        switch (playerAttack.currentWeapon.attackType)
        { 
            case WeaponAttackType.Swing:

                break;
        }
    }

    public void PlayAttackAnimation()
    {
        switch (playerAttack.currentWeapon.attackType)
        {
            case WeaponAttackType.Swing:

                break;
        }
    }
}
