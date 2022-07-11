using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    [SerializeField] private Transform lookTarget;
    [SerializeField] private float offset;
    void Update()
    {
        transform.position = lookTarget.position + Vector3.up * offset;
    }
}
