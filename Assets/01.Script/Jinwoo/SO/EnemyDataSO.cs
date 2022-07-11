using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Agent/EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    public string enemyName;

    public int maxHealth = 3;
    public GameObject prefab;

    //���� ���� ������
    public int damage = 1;
    public float attackDelay = 1;
    public float attackRadius = 2f;
    public float attackDistance = 2f;

    public float dodgeRadius = 2f;

    public int bulletSpeed;

    //���� ���� ������
    [Range(1, 10)]
    public float maxSpeed = 3;
    [Range(1f, 10f)]
    public float stoppingDistance = 5f;
    //public float knockbackPower = 5f;


    //�뽬 ����
    public float dashDistance = 3.5f;
    public float dashSpeed = 4f;
    public float dashDamage = 3f;


    //���� ����
    public float explosionDistance = 2f;
    public float explosionDamage = 5f;
    public float explosionRange = 3f;

    public AudioClip hitClip; // �ǰݽ� ����� �Ҹ�
    public AudioClip deathClip; // ����� ����� �Ҹ�

}
