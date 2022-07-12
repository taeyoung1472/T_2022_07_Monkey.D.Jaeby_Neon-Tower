using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    public static Define Instance;

    public PlayerController controller;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}