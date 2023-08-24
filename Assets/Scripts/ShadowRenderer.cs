using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ShadowRenderer : MonoBehaviour
{

    public Shader shadowShader;
    public RenderTexture ShadowTexture;
    private Material ShadowMat;

    private void Start()
    {
        ShadowMat = new Material(shadowShader);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        
        ShadowMat.SetTexture("_ShadowTex", ShadowTexture, RenderTextureSubElement.Color);
        Graphics.Blit(src,dest,ShadowMat);
        
    }
}
