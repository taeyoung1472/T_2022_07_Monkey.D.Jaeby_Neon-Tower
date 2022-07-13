using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolAbleObject
{
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);

    [Header("·¹º§ ´ëÀÀ ¹è¿­")]
    public float[] knockBackForce;
    public float[] freezeTime;
    public float[] explosionDamage;
    public float[] bulletSpeedFixValue;
    int bumpCount;
    int penetrationCount;
    int damage;
    BulletStat stat;

    [Header("")]
    private Rigidbody rb;
    public GameObject[] Detached;
    public AudioClip bounceClip;
    public LayerMask enemyLayer;
    public float speed;
    float curSpeed;

    void OnCollisionEnter(Collision collision)
    {
        if (bumpCount > 0 && collision.gameObject.CompareTag("Wall"))
        {
            Vector3 reflectDir = Vector3.Reflect(transform.position, collision.contacts[0].normal);
            Vector3 calculDir = new Vector3(reflectDir.x, 0, reflectDir.z).normalized;
            rb.velocity = calculDir * curSpeed;
            bumpCount--;
            PoolManager.instance.Pop(PoolType.Sound).GetComponent<AudioPoolObject>().Play(bounceClip, 0.5f, Random.Range(0.9f, 1.1f));
        }
        else
        {
            if (explosionDamage[stat.explosion] > 0)
            {
                PopParticle(PoolType.BulletExplosionImpact);
                Collider[] cols = Physics.OverlapSphere(transform.position, 2, enemyLayer);
                foreach (Collider col in cols)
                {
                    if (col.gameObject.CompareTag("Enemy"))
                    {
                        DamageMessage message;
                        message.damager = gameObject;
                        message.amount = damage * explosionDamage[stat.explosion];
                        message.hitPoint = Vector3.zero;
                        message.hitNormal = Vector3.zero;

                        col.GetComponent<LivingEntity>().ApplyDamage(message);

                        if(stat.knockback != 0)
                        {
                            col.GetComponent<IEnemy>().KnockBack(col.transform.position - transform.position, knockBackForce[stat.knockback]);
                        }
                    }
                }
            }
            if (collision.gameObject.CompareTag("Enemy"))
            {
                DamageMessage message;
                message.damager = gameObject;
                message.amount = GameManager.Instance.bulletStat.damage;
                message.hitPoint = Vector3.zero;
                message.hitNormal = Vector3.zero;

                collision.gameObject.GetComponent<LivingEntity>().ApplyDamage(message);
                if (stat.freeze != 0)
                {
                    collision.gameObject.GetComponent<IEnemy>().Freeze(freezeTime[stat.freeze]);
                }
                if (stat.knockback != 0)
                {
                    collision.gameObject.GetComponent<IEnemy>().KnockBack(collision.transform.position - transform.position, knockBackForce[stat.knockback]);
                }
            }
            else
            {
                PoolManager.instance.Pop(PoolType.Sound).GetComponent<AudioPoolObject>().Play(bounceClip, 1f, Random.Range(1.9f, 2.1f));
            }
            #region ÃÑ¾Ë ÆÄ±« ÀÌÆåÆ®
            rb.constraints = RigidbodyConstraints.FreezeAll;

            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point + contact.normal * hitOffset;
            PopParticle(PoolType.BulletImpact);
            Push();
            #endregion
        }
    }

    IEnumerator TimePush()
    {
        yield return new WaitForSeconds(3f);
        Push();
    }

    void Push()
    {
        PoolManager.instance.Push(PoolType.Bullet, gameObject);
    }

    void PopParticle(PoolType type)
    {
        PoolManager.instance.Pop(type).GetComponent<ParticlePool>().Set(transform.position, transform.rotation);
    }
    public void Set(Vector3 pos, Quaternion rot)
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        transform.SetPositionAndRotation(pos, rot);

        if(stat == null)
            stat = GameManager.Instance.bulletStat;

        penetrationCount = stat.penetrationShot;
        bumpCount = stat.wallBounceCnt;
        damage = stat.damage;
        curSpeed = speed * bulletSpeedFixValue[stat.bulletSpd];

        PopParticle(PoolType.BulletMuzzleImpact);


        rb.velocity = transform.forward * curSpeed;
    }

    public override void Init_Pop()
    {
        StartCoroutine(TimePush());
    }

    public override void Init_Push()
    {
    }
}
