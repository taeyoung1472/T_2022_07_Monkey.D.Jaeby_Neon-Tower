using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBulletBase : PoolAbleObject
{
    [SerializeField]
    protected BulletDataSO _bulletData;

    protected bool _isEnemy;

    public bool IsEnemy
    {
        get => _isEnemy;
        set
        {
            _isEnemy = value;
        }
    }

    public float damageFactor = 1; //�Ѿ��� ������ ���

    public virtual BulletDataSO BulletData
    {
        get => _bulletData;
        set
        {
            _bulletData = value;
            damageFactor = _bulletData.damage;
        }
    }

    public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
    {
        transform.SetPositionAndRotation(pos, rot);
    }

    
}
