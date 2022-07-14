using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LastMonster : MonoBehaviour
{
    [field: SerializeField]
    private UnityEvent OnLastMonster = null;

    private void OnDisable()
    {
        OnLastMonster?.Invoke();   
    }
}
