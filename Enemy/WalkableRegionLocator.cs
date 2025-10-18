using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This component should be attached on walkable sprite 
/// To located the gameobject for AI 
/// </summary>

public class WalkableRegionLocator : MonoBehaviour
{
    public static WalkableRegionLocator Instance;

    private void Awake()
    {
        Instance = this;
    }
    
}
