using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

/// <summary>
/// Attached on the player
/// check whether resource need spawn in each scene
/// </summary>
public class ResourceDetector : MonoBehaviour
{
    public static ResourceDetector Instance;

    [Header("SceneSpawnersParameter")]
    int DayToSpawn_BeachScene = 1;

    private void Awake()
    {
        Instance = this;
    }
    
    public void CheckSpawnNewResourceOrNot() //be called when enter the scene
    {
        bool spawnOrNot=false;
        int currentDay = TimeSystem.Instance.day;
        string currentScene=SceneManager.GetActiveScene().name;
        switch (currentScene)
        {
            case "BeachScene":
                if (DayToSpawn_BeachScene == currentDay)
                {
                    spawnOrNot = true;
                    DayToSpawn_BeachScene++;
                } 
                break;
            case "Shelter":

                break;
            
        }
        if (spawnOrNot) 
        {
            ResourceSpawnManager.Instance.spawn=true;
        }
    }
}
