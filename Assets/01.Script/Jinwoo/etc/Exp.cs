using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp : MonoBehaviour
{
    public ParticleSystem _expParticle;
    
    public float rValue = 0f; 
    public float gValue = 0f; 
    public float duration = 0.5f;
    public float curTime = 0f;

    private void Awake()
    {
        _expParticle = GetComponent<ParticleSystem>();
    }

    void Start()
    {
        _expParticle.startColor = Color.blue;
    }
    IEnumerator ChangeColor()
    {
        yield return new WaitForSeconds(2f);
    }
    void Update()
    {
        
    }
}
