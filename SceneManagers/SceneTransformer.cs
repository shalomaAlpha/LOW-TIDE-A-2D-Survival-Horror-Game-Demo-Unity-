using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransformer : MonoBehaviour
{
    /// <summary>
    /// This handle the calling of scene transform functions 
    /// And the call back of scene transform functions
    /// </summary>
    public static SceneTransformer Instance;
    string currentScene;
    string targetScene;
    private void Awake()
    {
        Instance = this;
    }
    public void TransformScene(string _currentScene, string _targetScene)
    {
        currentScene = _currentScene;
        targetScene = _targetScene;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(targetScene);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (currentScene == "Captain1")
        {
            BeachSceneManager.Instance.SpawnPointID = 1;
            TaskUI.Instance.UpdateTaskContent("Task: Find Mahana's father's whereabouts", 0.05f);
        }
        else
        {
            if (currentScene == "BeachScene")
            {
                if (targetScene == "Shelter")
                {
                    ShelterSceneManager.Instance.ResetShelterInventory();
                    
                }

            }
            else if (currentScene == "Shelter")
            {
                if (targetScene == "BeachScene")
                {
                    Debug.Log("Set spawnPoint 2");
                    BeachSceneManager.Instance.SpawnPointID = 2;
                    BeachSceneData data=SceneDatabase.Instance.GetBeachSceneData();
                    ResourceManager.Instance.SpawnRecordedResource(data);
                    //BeachSceneManager.Instance.SetSceneData();
                    
                }
            }
        }

        ResourceDetector.Instance.CheckSpawnNewResourceOrNot();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
