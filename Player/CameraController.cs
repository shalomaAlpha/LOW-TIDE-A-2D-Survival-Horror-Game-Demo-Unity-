using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    Animation parentAnimation;
    // Start is called before the first frame update
    private Vector3 originalPos;

    [Header("Variables")]
    public Transform player;
    public Vector3 offset;
    private void Awake()
    {
        Instance = this;
        originalPos = transform.localPosition;
    }
    void Start()
    {
        if (player != null)
        {
            offset = transform.position - player.position;
        }
        parentAnimation = GetComponentInParent<Animation>();
    }

    void Update()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }

    public void Shake()
    {
        parentAnimation.Play();
    }

    
}
