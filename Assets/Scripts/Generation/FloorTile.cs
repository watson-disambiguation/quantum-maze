using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
    public Tile.State state { get; private set; }

    [SerializeField]
    private GameObject[] wallStates;

    private void Awake()
    {
        for (int i = 0; i < wallStates.Length; i++)
        {
            wallStates[i].SetActive(false);
        }
    }
    public void SetState(Tile.State newState)
    {
        wallStates[(int)state].SetActive(false);
        state = newState;
        wallStates[(int)newState].SetActive(true);
    }
}
