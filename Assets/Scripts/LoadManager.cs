using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public static LoadManager instance;

    [SerializeField] private string gameScene, menuScene;
    
    // Start is called before the first frame update
    void Start()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(menuScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
