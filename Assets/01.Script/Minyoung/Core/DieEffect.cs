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

    public void Start()
    {
        particle.Stop();
        particle1.Stop();
        particle2.Stop();
    }

    public void PlayerDieEffect()
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
        seq.AppendCallback(() => EnemySubject.instance.NotifyObserver());
        seq.AppendCallback(() => Samples.SampleController.instance.cantDoZero = true);
        seq.AppendInterval(0.25f);
        seq.AppendCallback(() => Samples.SampleController.instance.StartSceneValue());
        seq.Append(DOTween.To(() => Samples.SampleController.instance._intensity, x => Samples.SampleController.instance._intensity = x, 1f, 3f));
        seq.AppendInterval(1f);
        seq.AppendCallback(() => Time.timeScale = 0);
        seq.AppendCallback(() => UIManager.Instance.DieUISet());
        Samples.SampleController.instance.LoadGameCutScene();
    }
}
