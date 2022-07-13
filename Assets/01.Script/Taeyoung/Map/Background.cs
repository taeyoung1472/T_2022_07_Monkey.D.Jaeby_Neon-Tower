using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Background : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private float speed;
    [SerializeField] private AudioClip clip;

    public void FloorChange()
    {
        mat.mainTextureOffset = Vector2.zero;
        Sequence seq = DOTween.Sequence();
        seq.Append(DOTween.To(() => mat.mainTextureOffset, x => mat.mainTextureOffset = x, new Vector2(0, 30), 4f));
        seq.AppendCallback(() => LevelUpCallback());
        PoolManager.instance.Pop(PoolType.Sound).GetComponent<AudioPoolObject>().Play(clip, 1, 1);
    }
    void LevelUpCallback()
    {

    }
}
