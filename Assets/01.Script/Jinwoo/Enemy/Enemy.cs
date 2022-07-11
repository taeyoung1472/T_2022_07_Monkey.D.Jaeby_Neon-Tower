using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : IHittable, IKnockback,IAgent
{
    public int Health => throw new System.NotImplementedException();

    public UnityEvent OnDie { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public UnityEvent OnGetHit { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public bool IsEnemy => throw new System.NotImplementedException();

    public Vector3 HitPoint => throw new System.NotImplementedException();

    public void GetHit(int damage, GameObject damageDealer)
    {
        throw new System.NotImplementedException();
    }

    public void Knockback(Vector3 direction, float power, float duration)
    {
        throw new System.NotImplementedException();
    }


}
