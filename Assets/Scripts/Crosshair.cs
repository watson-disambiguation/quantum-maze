using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private Sprite offTarget, onTarget;

    private Image crosshair;

    private void Start()
    {
        crosshair = GetComponent<Image>();
        crosshair.sprite = offTarget;
    }

    public void SetOnTarget(bool isOnTarget)
    {
        crosshair.sprite = isOnTarget ? onTarget : offTarget;
    }
    
    
}
