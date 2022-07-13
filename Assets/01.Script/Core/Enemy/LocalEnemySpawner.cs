using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LocalEnemySpawner : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    public void SpawnEnemy(GameObject enemy)
    {
        Sequence seq = DOTween.Sequence();

        seq.AppendCallback(() => particle.Play());
        seq.AppendInterval(1f);
        seq.AppendCallback(() => Instantiate(enemy, transform.position, Quaternion.identity));
    }
}
