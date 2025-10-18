using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInstance : MonoBehaviour
{
    public static CameraInstance Instance;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }
}
