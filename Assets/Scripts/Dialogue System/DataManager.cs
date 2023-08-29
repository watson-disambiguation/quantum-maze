using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DataManager : MonoBehaviour
{
    public TextAsset sharedData;
    public VariablesState variablesState = null;
    public static DataManager instance;

    private void Awake()
    {
        if(instance == null || instance == this)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        { 
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        var story = new Story(sharedData.text);
        variablesState = story.variablesState;
    }

}
