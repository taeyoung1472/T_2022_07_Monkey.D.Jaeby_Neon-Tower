using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : EnemyBulletBase
{
    protected Rigidbody _rigidbody;
    //protected SpriteRenderer _spriterRenderer;
    protected float _timeToLive;

    protected int _enemyLayer;
    protected int _obstacleLayer;

    protected bool _isDead = false; //�Ѱ��� �Ѿ��� �������� ���� �����ִ� ���� ���� ����.

    public override BulletDataSO BulletData
    {
        get => _bulletData;
        set
        {
            //_bulletData = value;
            base.BulletData = value;

            if (_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }
            _rigidbody.drag = _bulletData.friction;

            //if(_spriterRenderer == null)
            //{
            //    _spriterRenderer = GetComponent<SpriteRenderer>();
            //}
            //_spriterRenderer.material = _bulletData.bulletMat;


            if (_isEnemy)
                _enemyLayer = LayerMask.NameToLayer("Player");
            else
                _enemyLayer = LayerMask.NameToLayer("Enemy");
        }
    }

    private void Awake()
    {
        //_obstacleLayer = LayerMask.NameToLayer("Obstacle");
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

        if (collision.gameObject.layer == _obstacleLayer)
        {
            HitObstacle(collision);
        }

        if (collision.gameObject.layer == _enemyLayer)
        {
            HitEnemy(collision);
        }
        _isDead = true;

        //���⼭ Ǯ
        PoolManager.instance.Push(PoolType.EnemyBullet, gameObject);
        //PoolManager.Instance.Push(this);
    }

    private void HitEnemy(Collider collider)
    {
        //IKnockback kb = collider.GetComponent<IKnockback>();
        //kb?.Knockback(transform.forward, _bulletData.knockBackPower, _bulletData.knockBackDelay);

        //�ǰݽ� �Ѿ�����Ʈ ������ ��

        //IHittable hittable = collider.GetComponent<IHittable>();
        //if (hittable != null && hittable.IsEnemy == IsEnemy)
        //{
        //    return; //�Ѿ˰� �ǰ�ü�� �Ǿƽĺ��� ���� ��� �Ʊ��ǰ�
        //}
        //hittable?.GetHit(damage: _bulletData.damage * damageFactor, damageDealer: gameObject);

        IDamageable hittable = collider.GetComponent<IDamageable>();
        if (hittable == null)
        {
            return;
        }
        var message = new DamageMessage();
        message.amount = BulletData.damage;
        message.damager = gameObject;
        message.hitPoint = collider.transform.TransformPoint(transform.position);
        message.hitNormal = collider.transform.TransformDirection(transform.position.normalized);
        hittable?.ApplyDamage(message);
        //Vector3 randomOffset = Random.insideUnitSphere * 0.5f;

        //Hit ����Ʈ ���
        ImpactScript impact = PoolManager.instance.Pop(PoolType.EnemyBulletImpact).GetComponent<ImpactScript>();

        Quaternion rot = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 359f)));
        impact.SetPositionAndRotation(collider.transform.position + new Vector3(0, 1.2f, 0), rot);
    }

    private void HitObstacle(Collider collider)
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.right, out hit, 10f, 1 << _obstacleLayer))
        {
            if (hit.collider != null)
            {
                //���� �¾��� �� ������ ȸ�������� ȸ���� ImpactObject �����Ǽ� �浹��ġ�� ��Ȯ�ϰ� ��Ÿ���� �������.
                ImpactScript impact = PoolManager.instance.Pop(PoolType.EnemyBulletImpact).GetComponent<ImpactScript>();
                Quaternion rot = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360f)));
                impact.SetPositionAndRotation(hit.point + transform.forward * 0.5f, rot);
            }
        }
        
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
