using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitUIManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject ExitMenu;
    public void OnPressedExitButton()
    {
        Application.Quit();
    }

    public void OnPressedCancelButton()
    {
        ExitMenu.SetActive(false);
        MainMenu.SetActive(true);
    }
}
