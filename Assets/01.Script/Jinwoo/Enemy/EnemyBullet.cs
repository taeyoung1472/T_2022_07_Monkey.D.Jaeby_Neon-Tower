using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : EnemyBulletBase
{
    protected Rigidbody _rigidbody;
    protected float _timeToLive;

    protected bool _isDead = false; //�Ѱ��� �Ѿ��� �������� ���� �����ִ� ���� ���� ����.

    public override BulletDataSO BulletData
    {
        get => _bulletData;
        set
        {
            base.BulletData = value;

            if (_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }
            _rigidbody.drag = _bulletData.friction;
        }
    }

    protected virtual void FixedUpdate()
    {
        _timeToLive += Time.fixedDeltaTime;

        if (_timeToLive >= _bulletData.lifeTime)
        {
            _isDead = true;

            //���⼭ Ǯ
            PoolManager.instance.Push(PoolType.EnemyBulletImpact, gameObject);
            //PoolManager.Instance.Push(this);
        }

        if (_rigidbody != null && _bulletData != null)
        {
            _rigidbody.MovePosition(
                transform.position +
                _bulletData.bulletSpeed * transform.forward * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (_isDead) return;  //���� ����ź�̸� ���⼭ ���� �ٸ� �۾��� �ؾ� �Ѵ�.

        //���⿡�� �ǰ��ؼ� �������� �ְ� �˹��Ű�� �ڵ尡 ���⿡ ���ߵȴ�.

        if (collision.CompareTag("Player"))
        {
            Define.Instance.controller.Damaged();
        }

        ImpactScript impact = PoolManager.instance.Pop(PoolType.EnemyBulletImpact).GetComponent<ImpactScript>();
        Quaternion rot = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360f)));
        impact.SetPositionAndRotation(transform.position + transform.forward * 1.5f, rot);

        _isDead = true;

        PoolManager.instance.Push(PoolType.EnemyBullet, gameObject);
    }

    public override void Init_Pop()
    {
        damageFactor = 1;
        _timeToLive = 0;
        _isDead = false;
    }

    public override void Init_Push()
    {

    }

}
