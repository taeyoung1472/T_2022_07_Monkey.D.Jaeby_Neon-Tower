using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayerItem : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("E"))
        {
            Debug.Log("������ ��");
            Debug.Log("ī�޶� ��鸲 + ����?");
        }
    }
}
