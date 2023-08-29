using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class TravellerController : MonoBehaviour
{
    public TravellerData currentTraveller;

    public List<TravellerData> travellerData;

    public static TravellerController instance;

    [Range(0, 1)] public float SpawnProbability = 0.01f;
    public List<SpriteRenderer> travellers;
    private Tile locationTile;
    private bool alreadySpawned = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                int index = 3 * i + j;
                GameObject current = travellers[index].gameObject;
                current.transform.position = this.transform.position + new Vector3((i-1) * TileManager.instance.Width() * 2, (j-1) * TileManager.instance.Height() * 2);
            }
        }
        Despawn();
    }

    public void SetTileLocation(Tile newLocation)
    {
        SelectCurrentTraveller();
        alreadySpawned = true;
        locationTile = newLocation;
        newLocation.occupied = true;
        transform.position = newLocation.transform.position;
    }

    public void DecideSpawn(Tile potentialTile)
    {
        float rand = UnityEngine.Random.value;
        if (!alreadySpawned && rand < SpawnProbability)
        {
            SetTileLocation(potentialTile);
        }
    }

    public void Despawn()
    {
        alreadySpawned = false;
        locationTile = null;
        transform.position = new Vector3(TileManager.instance.Width(),TileManager.instance.Height()) * 8;
    }

    private void SelectCurrentTraveller()
    {
        currentTraveller = travellerData[UnityEngine.Random.Range(0, travellerData.Count)];
        foreach (var traveller in travellers)
        {
            traveller.sprite = currentTraveller.sprite;
        }

        if (currentTraveller.hasMetCurrentSpawn)
        {
            currentTraveller.timesMet++;
        }

        currentTraveller.hasMetCurrentSpawn = false;
    }
    
}
