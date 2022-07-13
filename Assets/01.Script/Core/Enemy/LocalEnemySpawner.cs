using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalEnemySpawner : MonoBehaviour
{
    public void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, transform.position, Quaternion.identity);
    }
}
