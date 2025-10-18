using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    //Control play movement
    [Header("Player movement setting")]
    public float speed;
    float speedFix=1f; //Used to modify the situation where the rigid body moves too slowly
    public bool allowToMove = true;
    [Header("Player Comonent")]
    private Rigidbody2D playerRig;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
    }
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        playerRig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(allowToMove)
        {
            playerMovement();
        }
        else
        {
            PlayerAnimationManager.Instance.IdleAndWalkAniamtion(0, 0);
        }
        
    }
    void playerMovement()
    {
        if (PlayerAnimationManager.Instance.animatior == null)
        {
            Debug.Log("Animation is null");
            PlayerAnimationManager.Instance.SetAnimator();  
            return;
        } 
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        PlayerAnimationManager.Instance.IdleAndWalkAniamtion(horizontal, vertical);
        Vector2 inputDirection = new Vector2(horizontal, vertical);
        if (inputDirection.magnitude > 1f)
        {
            inputDirection.Normalize();
        }
        Vector2 currentPosition = transform.position;
        currentPosition += inputDirection * Time.deltaTime * speed * speedFix;
        playerRig.MovePosition(currentPosition);
    }

    public Vector2 GetInputVector()
    {
        if (allowToMove)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            return new Vector2(horizontal, vertical);
        }

        else return new Vector2(0, 0);
    }

    public void OnPlayerDead()
    {

    }
}
