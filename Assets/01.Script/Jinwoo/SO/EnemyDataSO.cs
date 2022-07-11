using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Agent/EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    public string enemyName;

    public int maxHealth = 3;


    //공격 관련 데이터
    public int damage = 1;
    public float attackDelay = 1;

    public float knockbackPower = 5f;

}
