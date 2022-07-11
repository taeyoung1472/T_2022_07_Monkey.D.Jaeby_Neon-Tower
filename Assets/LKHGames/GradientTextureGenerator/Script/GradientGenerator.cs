using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GradientGenerator : MonoBehaviour
{
    public Gradient gradient;
    public string savingPath = "/LKHGames/GradientTextureGenerator/GeneratedTexture/";

    [Tooltip("Width of the gradient texture, 256 by default")]
    public float width = 256;
    [Tooltip("Height of the gradient texture, 64 by default")]
    public float height = 64;

    private Texture2D _gradientTexture;
    private Texture2D _tempTexture;


    public enum OnPlayMode {Off, UpdateOnStart, UpdateEveryFrame};
    [Header("Material Properties")]
    public OnPlayMode onPlayMode;
    public string propertiesName;
    public Renderer materialRenderer;

    void Start()
    {
        switch (onPlayMode)
        {
            case OnPlayMode.Off:
                break;
            case OnPlayMode.UpdateOnStart:
                UpdateGradientTexture();
                break;
            case OnPlayMode.UpdateEveryFrame:
                break;
        }
    }

    void Update()
    {
        switch (onPlayMode)
        {
            case OnPlayMode.Off:
                break;
            case OnPlayMode.UpdateOnStart:
                break;
            case OnPlayMode.UpdateEveryFrame:
                UpdateGradientTexture();
                break;
        }
    }

    Texture2D GenerateGradientTexture(Gradient grad)
    {
        /*
        if (tempTexture == null)
        {
            tempTexture = new Texture2D((int)width, (int)height);
        }*/
        _tempTexture = new Texture2D((int)width, (int)height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = grad.Evaluate(0 + (x / width));
                _tempTexture.SetPixel(x, y, color);
            }
        }
        _tempTexture.wrapMode = TextureWrapMode.Clamp;
        _tempTexture.Apply();
        return _tempTexture;
    }

    public void UpdateGradientTexture()
    {
        if(materialRenderer!=null)
        {
            _gradientTexture = GenerateGradientTexture(gradient);
            materialRenderer.material.SetTexture(propertiesName, _gradientTexture);
        }
    }

    public void BakeGradientTexture()
    {
        _gradientTexture = GenerateGradientTexture(gradient);
        byte[] _bytes = _gradientTexture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + savingPath + "GradientTexture_" + Random.Range(0, 999999).ToString() + ".png", _bytes);
    }
}