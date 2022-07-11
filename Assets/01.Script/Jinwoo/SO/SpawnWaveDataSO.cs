using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Spawn/WaveData")]
public class SpawnWaveDataSO : ScriptableObject
{
    public int waveName;
    public int wave;

    public List<EnemyDataSO> _enemyList;

    public List<Enemy> enemies;

    public int enemySpawnCount;
    public int enemySpawnDelay;


}
