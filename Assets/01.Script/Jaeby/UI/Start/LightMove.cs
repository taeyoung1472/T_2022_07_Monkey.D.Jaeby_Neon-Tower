using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMove : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private float _max = 0f;
    [SerializeField]
    private float _min = 0f;


    private void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if(transform.position.y >= _max)
        {
            Vector3 pos = transform.position;
            pos.y = _min;
            transform.position = pos;
            _speed = Random.Range(1f, 4f);
        }
    }
}
