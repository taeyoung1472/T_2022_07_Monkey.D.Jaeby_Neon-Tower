using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class GunController : MonoBehaviour
{
    [SerializeField] private FirePos[] firePos;
    [SerializeField] private GameObject bullet;
    [SerializeField] private int fireUpgradeIndex = 0;
    [SerializeField] private float delay = 0.4f;
    [SerializeField] private Image reloadImage;
    private void Start()
    {
        StartCoroutine(ShootSystem());
    }
    IEnumerator ShootSystem()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.GetKey(KeyCode.Mouse0));
            foreach (var pos in firePos[fireUpgradeIndex].firepos)
            {
                Bullet bullet = PoolManager.instance.Pop(PoolType.Bullet).GetComponent<Bullet>();
                bullet.Set(pos.position, pos.rotation);
                reloadImage.rectTransform.sizeDelta = new Vector2(0, 20);
            }
            yield return new WaitForSeconds(delay);
            reloadImage.rectTransform.DOSizeDelta(new Vector2(350, 20), delay);
        }
    }
    [Serializable]
    class FirePos
    {
        public Transform[] firepos;
    }
}
