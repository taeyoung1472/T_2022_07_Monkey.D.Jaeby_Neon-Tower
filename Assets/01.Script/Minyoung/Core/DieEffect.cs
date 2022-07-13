using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DieEffect : MonoBehaviour
{

    public GameObject dieParticlePrefab;
    public GameObject dieEffectPrefab;
    public GameObject pregmantPrefab;
    public ParticleSystem particle;
    public ParticleSystem particle1;
    public ParticleSystem particle2;

    public Transform playerTrm;
    void Start()
    {
        PlayerDieEffect();
    }
    void PlayerDieEffect()
    {
        particle.Stop();
        particle1.Stop();
        particle2.Stop();
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => particle.Play());
        seq.AppendInterval(3f);
        seq.AppendCallback(() => particle1.Play());
        seq.AppendInterval(0.01f);
        seq.AppendCallback(() => CameraManager.instance.CameraShake(6f, 2f, .5f));
        seq.AppendCallback(() => particle2.Play());
        seq.AppendCallback(() => particle.Stop());
    }
}
