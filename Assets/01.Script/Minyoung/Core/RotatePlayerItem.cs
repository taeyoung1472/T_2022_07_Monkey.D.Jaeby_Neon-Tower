using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayerItem : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("E"))
        {
            Debug.Log("µ¥¹ÌÁö ÁÜ");
            Debug.Log("Ä«¸Þ¶ó Èçµé¸² + Æø¹ß?");
        }
    }
}
