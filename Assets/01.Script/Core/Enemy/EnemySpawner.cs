using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public LocalEnemySpawner[] localSpawners;
    public float[] spawnTime;

    public void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        int spawnerIndex = 0;
        while (true)
        {
            localSpawners[spawnerIndex].SpawnEnemy(EnemyGenerator.Instance.GetEnemy());
            spawnerIndex++;
            if(spawnerIndex >= localSpawners.Length)
            {
                spawnerIndex = 0;
            }
            yield return new WaitForSeconds(spawnTime[WaveManager.instance.curWave]);
        }
    }
    
}
