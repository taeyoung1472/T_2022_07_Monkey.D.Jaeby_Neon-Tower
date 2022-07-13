using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalEnemySpawner : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    public void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, transform.position, Quaternion.identity);
        particle.Play();
    }
}
