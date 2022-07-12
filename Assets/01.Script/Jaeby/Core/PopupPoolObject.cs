using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class PopupPoolObject : PoolAbleObject
{
    [SerializeField]
    private TextMeshPro _text = null;

    private Sequence _seq = null;

    public override void Init_Pop()
    {
    }

    public override void Init_Push()
    {
        if(_seq != null)
        {
            _seq.Kill();
        }

        _text.fontSize = 8;

        _text.transform.position = Vector3.zero;
        _text.transform.localScale = Vector3.one;

        _text.color = Color.white;
    }

    public void PopupText(Vector3 startPos, Vector3 lastPos, Color color, float duration, int fontSize = 5) // ���� ������, ������ ������, ����, ��Ʈ������, ������
    {
        startPos.z += 0.5f;
        lastPos.z += 0.5f;
        startPos.y += 2f;
        lastPos.y += 2f;

        transform.position = startPos;
        _text.color = color;
        _text.fontSize = fontSize;

        _seq = DOTween.Sequence();
        _seq.Append(transform.DOMove(lastPos, duration));
        _seq.Join(_text.DOFade(0, duration));
        _seq.AppendCallback(() =>
        {
            PoolManager.instance.Push(PoolType, gameObject);
        });
    }

    public void PopupJumpWithRandomness(Vector3 startPos, float jumpPower, float randomXmove, Color color, float duration, int fontSize = 5)
    {
        startPos.z += jumpPower;
        startPos.y += 2f;

        float originPos = startPos.z;
        transform.position = startPos;
        _text.color = color;
        _text.fontSize = fontSize;

        _seq = DOTween.Sequence();
        _seq.Append(transform.DOMoveZ(0f, duration).SetEase(Ease.OutBounce));
        _seq.Join(transform.DOMoveX(transform.position.x + Random.Range(-randomXmove, randomXmove), duration));
        _seq.Join(_text.DOFade(0, duration));

        _seq.AppendCallback(() =>
        {
            PoolManager.instance.Push(PoolType, gameObject);
        });
    }
}