using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This component is used in test resource
/// it will should be attached on the resurouce
/// </summary>
public class ResourceTest : MonoBehaviour
{
    Resource resource;
    void Start()
    {
        resource = GetComponent<Resource>();
        resource.InitiliseResourceInSpawner();
    }

    
    
}
