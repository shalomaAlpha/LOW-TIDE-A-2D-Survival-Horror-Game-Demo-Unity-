using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    private Collider2D spawnArea;
    public List<GameObject> resourcePrefabs;
    public int minSpawnAmount;
    public int maxSpawnAmount;

    public void SpawnResources()
    {
        if ( resourcePrefabs.Count == 0)
        {
            Debug.LogWarning("Please set the resource prefabs");
            return;
        }
        else
        {
            spawnArea = GetComponent<Collider2D>();
            int spawnCount = Random.Range(minSpawnAmount, maxSpawnAmount + 1); 
            Bounds bounds = spawnArea.bounds;

            for (int i = 0; i < spawnCount; i++)
            {
                Vector2 randomPosition = new Vector2(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y)
                );

                GameObject randomPrefab = resourcePrefabs[Random.Range(0, resourcePrefabs.Count)];
                GameObject Instance= Instantiate(randomPrefab, randomPosition, Quaternion.identity);

                Resource resourceComponent = Instance.GetComponent<Resource>();
                resourceComponent.InitiliseResourceInSpawner();
            }
        }

    }
}
