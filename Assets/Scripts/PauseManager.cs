using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PlayerLoop;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    public bool paused = false;
    
    [SerializeField]
    private RectTransform pauseMenu;
    // Start is called before the first frame update
    void Awake()
    {
        Cursor.visible = false;
        pauseMenu.gameObject.SetActive(false);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                SetPaused(false);
            }
            else
            {
                SetPaused(true);
            }
        }
    }

    void SetPaused(bool pauseState)
    {
        paused = pauseState;
        pauseMenu.gameObject.SetActive(pauseState);
        Cursor.visible = pauseState;
    }

}
