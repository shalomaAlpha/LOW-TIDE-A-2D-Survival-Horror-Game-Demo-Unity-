using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUICanvas : MonoBehaviour
{
    public static PlayerUICanvas Instance;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
    }
}
