using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMove : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;

    private void Update()
    {
        GetComponent<MeshRenderer>().material.mainTextureOffset += Vector2.up * _speed * Time.deltaTime;    
    }
}
