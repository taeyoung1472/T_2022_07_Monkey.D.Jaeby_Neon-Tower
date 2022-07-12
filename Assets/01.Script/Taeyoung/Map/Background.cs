using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Background : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private float speed;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            LevelUp();
        }
    }
    public void LevelUp()
    {
        mat.mainTextureOffset = Vector2.zero;
        Sequence seq = DOTween.Sequence();
        seq.Append(DOTween.To(() => mat.mainTextureOffset, x => mat.mainTextureOffset = x, new Vector2(0, 10), 5f));
        seq.AppendCallback(() => LevelUpCallback());
    }
    void LevelUpCallback()
    {

    }
}
