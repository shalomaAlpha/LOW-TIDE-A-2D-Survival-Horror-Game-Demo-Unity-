using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Control the animation of player
/// </summary>
public class PlayerAnimationManager : MonoBehaviour
{
    public static PlayerAnimationManager Instance;
    public Animator animatior;

    private string IdleParamterKey="Default";
    private string WalkParamterKey="Default";

    float previousHorizontal=0f;
    float previousVertical=0f;
    private void Awake()
    {
        Instance = this;
    }


    public void IdleAndWalkAniamtion(float horizontal, float vertical)
    {
        if(horizontal==0 && vertical==0)
        {
            animatior.SetBool("Walk", false);
            if(WalkParamterKey != "Default") animatior.SetBool(WalkParamterKey, false);
            animatior.SetBool("Idle", true);
            WalkParamterKey = "Default";
            SetIdleAnimationAccrodingToParameter();
        }
        else
        {
            previousVertical = vertical;
            previousHorizontal = horizontal;
            if(IdleParamterKey != "Default") animatior.SetBool(IdleParamterKey, false);
            animatior.SetBool("Walk", true);
            animatior.SetBool("Idle", false);
            IdleParamterKey = "Default";
            SetWalkAnimationAccordingToParameter(horizontal,vertical);
        }
    }

    #region idle
    private void SetIdleAnimation(string TargetIdleParamter)
    {
        if (TargetIdleParamter == IdleParamterKey)
            return;
        else
        {
            if (IdleParamterKey != "Default") animatior.SetBool(IdleParamterKey, false);

            IdleParamterKey = TargetIdleParamter;
            animatior.SetBool(IdleParamterKey, true);
        }
    }
    private void SetIdleAnimationAccrodingToParameter()
    {
        if (previousHorizontal > 0 && previousVertical > 0)
        {
            SetIdleAnimation("IdleRightUp");
        }
        else if (previousHorizontal < 0 && previousVertical < 0)
        {
            SetIdleAnimation("IdleLeftDown");
        }
        else if (previousHorizontal > 0 && previousVertical < 0)
        {
            SetIdleAnimation("IdleRightDown");
        }
        else if (previousHorizontal < 0 && previousVertical > 0)
        {
            SetIdleAnimation("IdleLeftUp");
        }
        else if (previousHorizontal == 0 && previousVertical > 0)
        {
            SetIdleAnimation("IdleUp");
        }
        else if (previousHorizontal == 0 && previousVertical < 0)
        {
            SetIdleAnimation("IdleDown");
        }
        else if (previousHorizontal > 0 && previousVertical == 0)
        {
            SetIdleAnimation("IdleRight");
        }
        else if (previousHorizontal < 0 && previousVertical == 0)
        {
            SetIdleAnimation("IdleLeft");
        }
        else
        {
            SetIdleAnimation("IdleDown");
        }
    }
    #endregion
    #region walking
    private void SetWalkAnimationAccordingToParameter(float horizontal, float vertical)
    {
        if (horizontal > 0 && vertical>0)
        {
            SetWalkAnimation("WalkRightUp");
        }
        else if(horizontal>0 && vertical<0)
        {
            SetWalkAnimation("WalkRightDown");
        }
        else if(horizontal<0 && vertical>0)
        {
            SetWalkAnimation("WalkLeftUp");
        }
        else if (horizontal < 0 && vertical < 0)
        {
            SetWalkAnimation("WalkLeftDown");
        }
        else if(horizontal>0 && vertical==0)
        {
            SetWalkAnimation("WalkRight");
        }
        else if(horizontal<0 && vertical==0)
        {
            SetWalkAnimation("WalkLeft");
        }
        else if(horizontal==0 && vertical>0)
        {
            SetWalkAnimation("WalkUp");
        }
        else if (horizontal == 0 && vertical < 0)
        {
            SetWalkAnimation("WalkDown");
        }

    }
    private void SetWalkAnimation(string TargetWalkParamter)
    {
        if (TargetWalkParamter == WalkParamterKey)
            return;
        else
        {
            if(WalkParamterKey!="Default") animatior.SetBool(WalkParamterKey, false);

            WalkParamterKey = TargetWalkParamter;
            animatior.SetBool(WalkParamterKey, true);
        }
    }
    #endregion
    #region fishing
    public void SetFishAnimation()
    {
        animatior.SetBool("Walk", false);
        if (WalkParamterKey != "Default") animatior.SetBool(WalkParamterKey, false);
        animatior.SetBool("Idle", true);
        WalkParamterKey = "Default";
        SetWalkAnimation("IdleDown");
        animatior.SetBool("Fish", true);
    }

    public void EndFishAnimation()
    {
        animatior.SetBool("Fish", false);
        animatior.SetBool("FishHooked", false);
        animatior.SetBool("FishCatch", false);
    }

    #endregion

    #region Attack
    public void StartAttackAnimation()
    {
        animatior.SetBool("Attack", true);
        Camera mainCamera = Camera.main;
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        float mouseX = mouseWorldPos.x;
        GameObject player=PlayerController.Instance.gameObject; 
        float playerX = player.transform.position.x;

        bool facingRight = mouseX > playerX;
        animatior.SetBool("AttackRight", facingRight);
    }
    public void EndAttackAnimation()
    {
        animatior.SetBool("Attack", false);
    }
    
    public void SetAnimator()
    {
        animatior=GetComponent<Animator>();
    }
    #endregion

}
