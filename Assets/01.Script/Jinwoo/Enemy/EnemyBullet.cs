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

    protected bool _isDead = false; //한개의 총알이 여러명의 적에 영향주는 것을 막기 위함.

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

        if (collision.gameObject.layer == _obstacleLayer)
        {
            HitObstacle(collision);
        }

        if (collision.gameObject.layer == _enemyLayer)
        {
            HitEnemy(collision);
        }
        _isDead = true;

        //여기서 풀
        PoolManager.instance.Push(PoolType.EnemyBullet, gameObject);
        //PoolManager.Instance.Push(this);
    }

    private void HitEnemy(Collider collider)
    {
        //IKnockback kb = collider.GetComponent<IKnockback>();
        //kb?.Knockback(transform.forward, _bulletData.knockBackPower, _bulletData.knockBackDelay);

        //피격시 총알이펙트 만들어야 해

        //IHittable hittable = collider.GetComponent<IHittable>();
        //if (hittable != null && hittable.IsEnemy == IsEnemy)
        //{
        //    return; //총알과 피격체의 피아식별이 같을 경우 아군피격
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

        //Hit 이펙트 재생
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
                //벽에 맞았을 때 랜덤한 회전값으로 회전된 ImpactObject 생성되서 충돌위치에 정확하게 나타나고 사라진다.
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
