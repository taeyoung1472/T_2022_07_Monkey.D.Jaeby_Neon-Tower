using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolAbleObject
{
    public float defaultSpeed = 15f;
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public bool isCanBump;
    public bool isCanExplosion;
    public int bumpCount = 3;
    public int explosionDamage = 1;
    public int damage = 1;
    private Rigidbody rb;
    public GameObject[] Detached;
    public AudioClip bounceClip;
    public LayerMask enemyLayer;
    float speed;

    void OnCollisionEnter(Collision collision)
    {
        if (isCanBump && bumpCount > 0 && collision.gameObject.CompareTag("Wall"))
        {
            Vector3 reflectDir = Vector3.Reflect(transform.position, collision.contacts[0].normal);
            Vector3 calculDir = new Vector3(reflectDir.x, 0, reflectDir.z).normalized;
            rb.velocity = calculDir * speed;
            bumpCount--;
            PoolManager.instance.Pop(PoolType.Sound).GetComponent<AudioPoolObject>().Play(bounceClip, 0.5f, Random.Range(0.9f, 1.1f));
        }
        else
        {
            if (isCanExplosion)
            {
                PopParticle(PoolType.BulletExplosionImpact);
                Collider[] cols = Physics.OverlapSphere(transform.position, 2, enemyLayer);
                foreach (Collider col in cols)
                {
                    if (col.gameObject.CompareTag("Enemy"))
                    {
                        DamageMessage message;
                        message.damager = gameObject;
                        message.amount = explosionDamage;
                        message.hitPoint = Vector3.zero;
                        message.hitNormal = Vector3.zero;

                        col.GetComponent<LivingEntity>().ApplyDamage(message);
                    }
                }
            }
            if (collision.gameObject.CompareTag("Enemy"))
            {
                DamageMessage message;
                message.damager = gameObject;
                message.amount = damage;
                message.hitPoint = Vector3.zero;
                message.hitNormal = Vector3.zero;

                collision.gameObject.GetComponent<LivingEntity>().ApplyDamage(message);
            }
            else
            {
                PoolManager.instance.Pop(PoolType.Sound).GetComponent<AudioPoolObject>().Play(bounceClip, 1f, Random.Range(1.9f, 2.1f));
            }
            #region ÃÑ¾Ë ÆÄ±« ÀÌÆåÆ®
            rb.constraints = RigidbodyConstraints.FreezeAll;
            speed = 0;

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
        bumpCount = 3;
        speed = defaultSpeed;

        PopParticle(PoolType.BulletMuzzleImpact);

        rb.velocity = transform.forward * speed;
        print($"Velo : {transform.forward * speed}");
    }

    public override void Init_Pop()
    {
        StartCoroutine(TimePush());
    }

    public override void Init_Push()
    {
    }
}
