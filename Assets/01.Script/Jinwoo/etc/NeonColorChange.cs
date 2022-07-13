using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonColorChange : MonoBehaviour
{
    public Light myLight;
    public float smoothness = 0.02f;
    public float duration = 5;
    public float changetime = 5f;
    public float currentTime = 5f;
    public Color currentColor = Color.white;
    public Color[] changeColor;
    private void Awake()
    {
        myLight = GetComponent<Light>();
        currentColor = myLight.color;
        StartCoroutine(LerpColor());
    }
    private void Update()
    {
        if(Time.time >= changetime + currentTime)
        {
            currentTime = Time.time;
            StartCoroutine(LerpColor());
        }
    }

    public void ShuffleColor()
    {
        int random1;
        int random2;

        Color tmp;
        for (int i = 0; i < changeColor.Length; i++)
        {
            random1 = Random.Range(0, changeColor.Length);
            random2 = Random.Range(0, changeColor.Length);

            tmp = changeColor[random1];
            changeColor[random1] = changeColor[random2];
            changeColor[random2] = tmp;

        }
    }
    IEnumerator LerpColor()
    {
        ShuffleColor();
        float progress = 0;
        float increment = smoothness / duration;
        int idx = Random.Range(0, changeColor.Length);
        if (currentColor == changeColor[idx])
        {
            idx = Random.Range(0, changeColor.Length);
        }
        while (progress < 1)
        {
            currentColor = Color.Lerp(currentColor, changeColor[idx], progress);
            myLight.color = currentColor;
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }

    }
    
}
