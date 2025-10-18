using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuUIManager : MonoBehaviour
{
    public GameObject[] Panels; //0.Main menu, 1.loading, 2.loadScene, 3.Setting, 4.Quit 

    void Start()
    {
        foreach(GameObject panel in Panels)
        {
            if(panel!=null)
                panel.SetActive(false);
        }
        Panels[0].SetActive(true);
    }

    public void OnStartGameButtonPressed()
    {
        //DisplayPanel(1);
        SceneManager.LoadScene("Captain1");
    }
    public void OnExitGameButtonPressed()
    {
        DisplayPanel(4);
    }
    public void OnSettingButtonPressed()
    {
        DisplayPanel(3);
    }
    public void OnLoadButtonPressed()
    {
        DisplayPanel(2);
    }
    void DisplayPanel(int MenuID)
    {
        foreach (GameObject panel in Panels)
        {
            if (panel != null)
                panel.SetActive(false);
        }
        Panels[MenuID].SetActive(true);
    }

    public void OnBackButtonPressed()
    {
        DisplayPanel(0);
    }
}
