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
    private RectTransform pauseMenu,crosshair;
    // Start is called before the first frame update
    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
        Cursor.lockState = pauseState ? CursorLockMode.None : CursorLockMode.Locked;
        crosshair.gameObject.SetActive(!pauseState);
    }

}
