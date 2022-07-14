using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Background : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private float speed;
    [SerializeField] private AudioClip clip;

    [field: SerializeField]
    private UnityEvent OnEnemyDie = null;
    [field: SerializeField]
    private UnityEvent OnStart = null;
    [field: SerializeField]
    private UnityEvent OnExit = null;

    public void FloorChange()
    {
        OnStart?.Invoke();
        Sequence seq = DOTween.Sequence();
        CameraManager.instance.ZoomCamera(120f, 3f);
        seq.AppendInterval(3f);
        seq.AppendCallback(() =>
        {
            CameraManager.instance.CameraShake(4f, 4f, 4f);
            FloorMove();
            OnEnemyDie?.Invoke();
        });
    }

    private void FloorMove()
    {
        mat.mainTextureOffset = Vector2.zero;
        Sequence seq = DOTween.Sequence();
        seq.Append(DOTween.To(() => mat.mainTextureOffset, x => mat.mainTextureOffset = x, new Vector2(0, 30), 4f));
        seq.AppendCallback(() =>
        {
            CameraManager.instance.ZoomCamera(60f, 1f);
            LevelUpCallback();
            OnExit?.Invoke();

        });
        PoolManager.instance.Pop(PoolType.Sound).GetComponent<AudioPoolObject>().Play(clip, 1, 1);
    }

    void LevelUpCallback()
    {

    }
}
