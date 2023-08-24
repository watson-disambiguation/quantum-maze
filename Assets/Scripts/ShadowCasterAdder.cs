using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShadowCasterAdder : MonoBehaviour
{
    private void Awake()
    {
        gameObject.AddComponent<CompositeShadowCaster2D>();
    }
}
