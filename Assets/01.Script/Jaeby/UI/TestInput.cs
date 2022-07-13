using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            CameraManager.instance.CameraShake(10f, 10f, 1f);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            CameraManager.instance.ZoomCamera(30f, 1f);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            CameraManager.instance.ZoomCamera(60f, 1f);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            PoolManager.instance.Pop(PoolType.Popup).GetComponent<PopupPoolObject>().PopupText(_target.transform.position, _target.transform.position + Vector3.forward * 0.5f, Color.green, 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            PoolManager.instance.Pop(PoolType.Popup).GetComponent<PopupPoolObject>().PopupTextCritical(_target.transform.position, _target.transform.position + Vector3.forward * 0.5f, Color.red, 0.5f);
        }
    }
}
