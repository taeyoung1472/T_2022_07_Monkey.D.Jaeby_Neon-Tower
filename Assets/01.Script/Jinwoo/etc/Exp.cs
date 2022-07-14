using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp : MonoBehaviour
{
    public ParticleSystem _exp;

    public float duration = 0.5f;

    private void Awake()
    {
        _exp = GetComponent<ParticleSystem>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
