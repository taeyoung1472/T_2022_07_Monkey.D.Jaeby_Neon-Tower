using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    protected Collider _collider;
    public bool _isDamage = false;

    private void OnDisable()
    {
        _isDamage = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isDamage = true;
        }
    }

}
