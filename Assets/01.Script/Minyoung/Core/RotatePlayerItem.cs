using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayerItem : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            DamageMessage message;
            message.amount = 1;
            message.hitNormal = Vector3.zero;
            message.damager = gameObject;
            message.hitPoint = Vector3.zero;

            other.GetComponent<LivingEntity>().ApplyDamage(message);

            PoolManager.instance.Pop(PoolType.BulletImpact).GetComponent<ParticlePool>().Set(other.transform.position, Quaternion.identity);
        }
    }
}
