using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    public void KnockBack(Vector3 dir, float force);
    public void Freeze(float duration);
}
