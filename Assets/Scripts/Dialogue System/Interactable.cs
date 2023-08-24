using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float distanceThreshold = 2;
    public TextAsset inkJSON;

    private Player player;
    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnMouseOver()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            OnHit();
        }
        
    }

    public void OnHit()
    {
        Debug.Log("Hit");
        float distance = (player.transform.position - transform.position).magnitude;
        if(distance < distanceThreshold)
        {
            OpenText();
        }
    }

    public void OpenText()
    {
        TextManager.instance.scrollList.SetActive(true);
        TextManager.instance.Initialize(TravellerController.instance.inkJSON);
    }


}
