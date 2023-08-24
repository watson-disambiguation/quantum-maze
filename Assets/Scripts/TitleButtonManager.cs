using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButtonManager : MonoBehaviour
{
    public void StartButton()
    {
        LoadManager.instance.LoadGame();
    }
    
    public void QuitButton()
    {
        LoadManager.instance.QuitGame();
    }
}
