using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This manager should be placed in the scene
/// 1.Calling the resource spawner to spawn resources
/// </summary>
public class ResourceSpawnManager : MonoBehaviour
{
    public static ResourceSpawnManager Instance;

    public ResourceSpawner[] resourceSpawners;
    public bool spawn;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(spawn)
        {
            SpawnResource();
        }
    }
    public void SpawnResource()
    {
        if (resourceSpawners.Length!=0)
        {
            foreach (ResourceSpawner spawner in resourceSpawners)
            {
                spawner.SpawnResources();
            }
        }
        else
        {
            Debug.Log("array is null");
        }
    }
}

