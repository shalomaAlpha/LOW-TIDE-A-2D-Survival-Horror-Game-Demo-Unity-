using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// This component handle the scene transform trigger 
/// </summary>
public class ScenePort : MonoBehaviour
{
    public string SceneToTransform;
    public GameObject textHint;
    private bool withinRange;
    string currentSceneName;
    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        textHint.SetActive(false);
    }
    private void Update()
    {
        if (!withinRange) return;
        else
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                StoreSceneData();
                SceneTransformer.Instance.TransformScene(currentSceneName, SceneToTransform);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            textHint.SetActive(true);
            withinRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        textHint.SetActive(false);
        withinRange = false;
    }

    private void StoreSceneData()
    {
        string name=SceneManager.GetActiveScene().name;
        switch (name)
        {
            case "BeachScene":
                SceneDatabase.Instance.StoreBeachSceneData();
                break;
            case "Shelter":
                SceneDatabase.Instance.StoreShelterSceneData();
                break;
            default:
                break;
        }
    }
}
