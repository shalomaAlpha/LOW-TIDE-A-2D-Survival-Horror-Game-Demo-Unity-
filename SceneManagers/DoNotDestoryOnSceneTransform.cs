using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestoryOnSceneTransform : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
