using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : EnemyBulletBase
{
    protected Rigidbody _rigidbody;
    protected float _timeToLive;

    protected bool _isDead = false; //한개의 총알이 여러명의 적에 영향주는 것을 막기 위함.

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

            //여기서 풀
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
        if (_isDead) return;  //만약 관통탄이면 여기서 뭔가 다른 작업을 해야 한다.

        //여기에는 피격해서 데미지를 주고 넉백시키는 코드가 여기에 들어가야된다.

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
