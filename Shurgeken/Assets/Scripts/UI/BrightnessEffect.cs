using System;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Custom/Brightness Effect")]
public class BrightnessEffect : ImageEffectBase
{
    [Range(0f, 2f)]
    public float _Brightness = 1f;
    [Range(0f, 2f)]
    public float _Contrast = 1f;

    public PlayerSettings pSet;

    // Called by camera to apply image effect
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetFloat("_Brightness", _Brightness);
        material.SetFloat("_Contrast", _Contrast);
        Graphics.Blit(source, destination, material);
    }
    public void SetBrightness(float value)
    {
        _Brightness = value*2;
    }
    public void SetContrast(float value)
    {
        _Contrast = value*2;
    }

    
    new void  Start()
    {
        pSet = GameObject.FindObjectOfType<PlayerSettings>();
        _Brightness = pSet.brightness * 2;
        _Contrast = pSet.contrast * 2;
        shader = Shader.Find("Custom/Brightness Effect");
    }
}
