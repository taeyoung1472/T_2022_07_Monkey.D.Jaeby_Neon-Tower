using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Weapon/BulletData")]
public class BulletDataSO : ScriptableObject
{
    public GameObject prefab;
    [Range(1, 10)] public int damage = 1;
    [Range(1, 100f)] public float bulletSpeed = 1;

    [Range(0, 5f)] public float explosionRadius = 3f; //���߹ݰ�


    public float friction = 0f;

    //�̱��������� �׳� ���巯��
    public bool bounce = false; //�Ѿ��� ���� �ε����� �� ƨ�� ���ΰ�.
    public bool goThroughHit = false; //������ ���ΰ�.
    public bool isRayCast = false; //����ĳ��Ʈ �Ѿ��ΰ� 
    public bool isCharging = false; //��¡ �ҷ��̳�?

    public GameObject impactObstaclePrefab; //��ֹ��� �ε������� ���� ����Ʈ
    public GameObject impactEnemyPrefab; //���� �ε������� ���� ����Ʈ

    [Range(1, 50)] public float knockBackPower = 5f;
    [Range(0.01f, 1f)] public float knockBackDelay = 0.1f;

    public Material bulletMat;
    
    public float lifeTime = 2f; //�Ѿ��� ���� �ð�

}
