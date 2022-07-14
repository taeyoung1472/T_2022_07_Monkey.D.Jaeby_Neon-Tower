using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Test1 : MonoBehaviour
{
    [field: SerializeField]
    private UnityEvent<Collider> OnTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnTrigger?.Invoke(GetComponent<Collider>());
        }
    }
}
