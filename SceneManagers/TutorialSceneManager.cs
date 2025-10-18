using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TutorialSceneManager : MonoBehaviour
{
    public static TutorialSceneManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void TransformToNextScene()
    {
        Debug.Log("Try to move to next scene");
        SceneManager.LoadScene("BeachScene");
    }
}
