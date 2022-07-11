using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimescalePitch : MonoBehaviour
{
    AudioSource source;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    void Update()
    {
        source.pitch = 1 + (Time.timeScale - 1) * 0.5f;
    }
}
