using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public void MenuButton()
    {
        LoadManager.instance.LoadMenu();
    }

    public void RestartButton()
    {
        LoadManager.instance.LoadGame();
    }

    public void QuitButton()
    {
        LoadManager.instance.QuitGame();
    }
}
