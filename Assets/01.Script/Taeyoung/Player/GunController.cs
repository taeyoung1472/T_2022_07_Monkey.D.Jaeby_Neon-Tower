using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

public class GunController : MonoBehaviour
{
    [SerializeField] private FirePos[] firePos;
    [SerializeField] private GameObject bullet;
    [SerializeField] private int fireUpgradeIndex = 0;
    [SerializeField] private float[] delay;

    [Header("»ç¿îµå")]
    [SerializeField] private AudioClip shootClip;

    private void Start()
    {
        StartCoroutine(ShootSystem());
    }
    IEnumerator ShootSystem()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.GetKey(KeyCode.Mouse0));
            fireUpgradeIndex = GameManager.Instance.bulletStat.multiShotCount;
            foreach (var pos in firePos[fireUpgradeIndex].firepos)
            {
                Bullet bullet = PoolManager.instance.Pop(PoolType.Bullet).GetComponent<Bullet>();
                bullet.Set(pos.position, pos.rotation);
            }
            PoolManager.instance.Pop(PoolType.Sound).GetComponent<AudioPoolObject>().Play(shootClip, 1, Random.Range(0.9f, 1.1f));
            yield return new WaitForSeconds(delay[GameManager.Instance.bulletStat.bulletDelay]);
        }
    }
    [Serializable]
    class FirePos
    {
        public Transform[] firepos;
    }
}
