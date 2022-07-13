using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;

public class ContinueUI : MonoBehaviour, IUserInterface
{
    public UnityEvent OnOpenUI { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    [field:SerializeField]
    public UnityEvent OnCloseUI { get; set; }

    private Sequence _seq = null;
    [SerializeField]
    private TextMeshProUGUI _text = null;
    [SerializeField, Range(0f, 100f)]
    private float _shakePower = 50f;
    [SerializeField]
    private GameObject _ESCUI = null;

    private Vector2 _initPos = Vector2.zero;

    [SerializeField]
    private float _secTime = 1f;

    [SerializeField]
    private AudioClip _continueNumberChangeClip = null;
    [SerializeField]
    private AudioClip _continueEndClip = null;


    private void Start()
    {
        _initPos = new Vector2(0f, -700f);
        transform.localPosition = _initPos;
    }

    public void CloseUI()
    {
        if (_seq != null)
            _seq.Kill();

        _seq = DOTween.Sequence();
        _seq.Append(transform.DOLocalMoveY(_initPos.y, 0.3f)).SetUpdate(true);
        _seq.AppendCallback(() =>
        {
            OnCloseUI?.Invoke();
            Time.timeScale = 1f;
        });

    }

    public void OpenUI()
    {
        if (_seq != null)
            _seq.Kill();

        Time.timeScale = 0f;
        Debug.Log("¾¾´í¾Æ");
        _ESCUI.GetComponent<IUserInterface>().CloseUI();
        _seq = DOTween.Sequence();
        _seq.AppendInterval(0.3f);
        _seq.Append(transform.DOLocalMoveY(0f, 0.5f)).SetUpdate(true);
        _seq.AppendCallback(() =>
        {
            StartCoroutine(ContinueCoroutine());
        });
    }

    private IEnumerator ContinueCoroutine()
    {
        for(int i = 3; i>0; i--)
        {
            _text.SetText(i.ToString());

            if (_seq != null)
                _seq.Kill();

            _text.transform.localScale = Vector3.one;
            _seq = DOTween.Sequence();
            _seq.Append(_text.transform.DOShakePosition(_secTime, _shakePower, 15, 90, false, true)).SetUpdate(true);
            _seq.Join(_text.transform.DOScale(1.5f, _secTime).SetUpdate(true));
            PoolManager.instance.Pop(PoolType.Sound).GetComponent<AudioPoolObject>().Play(_continueNumberChangeClip);

            yield return new WaitForSecondsRealtime(_secTime);
        }

        PoolManager.instance.Pop(PoolType.Sound).GetComponent<AudioPoolObject>().Play(_continueEndClip);
        _text.SetText("");
        CloseUI();
    }

}
