using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public bool isCanBump;
    public bool isCanExplosion;
    public int bumpCount = 3;
    public GameObject hit;
    public GameObject flash;
    public GameObject explosionEffect;
    private Rigidbody rb;
    public GameObject[] Detached;
    public LayerMask enemyLayer; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (flash != null)
        {
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
        rb.velocity = transform.forward * speed;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (isCanBump && bumpCount > 0 && collision.gameObject.CompareTag("Wall"))
        {
            Vector3 reflectDir = Vector3.Reflect(transform.position, collision.contacts[0].normal);
            Vector3 calculDir = new Vector3(reflectDir.x, 0, reflectDir.z).normalized;
            rb.velocity = calculDir * speed;
            bumpCount--;
        }
        else
        {
            if (isCanExplosion)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
                Collider[] cols = Physics.OverlapSphere(transform.position, 5, enemyLayer);
                foreach (Collider col in cols)
                {
                    if (col.gameObject.CompareTag("Enemy"))
                    {
                        //�� ���ݹޱ�
                    }
                }
            }
            if (collision.gameObject.CompareTag("Enemy"))
            {
                //�� ���ݹޱ�
            }
            #region �Ѿ� �ı� ����Ʈ
            rb.constraints = RigidbodyConstraints.FreezeAll;
            speed = 0;

            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point + contact.normal * hitOffset;

            if (hit != null)
            {
                var hitInstance = Instantiate(hit, pos, rot);
                if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
                else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
                else { hitInstance.transform.LookAt(contact.point + contact.normal); }

                var hitPs = hitInstance.GetComponent<ParticleSystem>();
                if (hitPs != null)
                {
                    Destroy(hitInstance, hitPs.main.duration);
                }
                else
                {
                    var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(hitInstance, hitPsParts.main.duration);
                }
            }
            foreach (var detachedPrefab in Detached)
            {
                if (detachedPrefab != null)
                {
                    detachedPrefab.transform.parent = null;
                }
            }

            Destroy(gameObject);
            #endregion
        }
    }
}
