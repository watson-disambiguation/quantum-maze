using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float distanceThreshold = 5;
    

    public void OnHit()
    {
        Debug.Log("Hit");
        float distance = (Player.player.transform.position - transform.position).magnitude;
        if(distance < distanceThreshold)
        {
            OpenText();
        }
    }

    public void OpenText()
    {
        TextManager.instance.scrollList.SetActive(true);
        TextManager.instance.Initialize(TravellerController.instance.currentTraveller);
        TravellerController.instance.currentTraveller.hasMetCurrentSpawn = true;

    }


}
