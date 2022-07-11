using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GunController : MonoBehaviour
{
    [SerializeField] private FirePos[] firePos;
    [SerializeField] private GameObject bullet;
    [SerializeField] private int fireUpgradeIndex = 0;
    void Update()
    {
        Shoot();
    }
    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            foreach (var pos in firePos[fireUpgradeIndex].firepos)
            {
                Instantiate(bullet, pos.position, Quaternion.Euler(0, pos.eulerAngles.y, 0));
            }
        }
    }
    [Serializable]
    class FirePos
    {
        public Transform[] firepos;
    }
}
