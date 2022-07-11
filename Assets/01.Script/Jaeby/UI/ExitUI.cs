using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class ExitUI : MonoBehaviour, IUserInterface
{
    public UnityEvent OnOpenUI { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public UnityEvent OnCloseUI { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    [SerializeField]
    private CanvasGroup _optionCanvasGroup = null;

    private Sequence _seq = null;
    private Vector3 _originPos = Vector3.zero;


    private void Start()
    {
        _originPos = new Vector3(0f, -700f, 0f);
        transform.localPosition = _originPos;
    }

    public void CloseUI()
    {
        if (_seq != null)
            _seq.Kill();

        _seq = DOTween.Sequence();
        _seq.Append(transform.DOLocalMoveY(_originPos.y, 0.3f)).SetUpdate(true);
        _seq.AppendCallback(() =>
        {
            _optionCanvasGroup.interactable = true;
            _optionCanvasGroup.blocksRaycasts = true;
        });
    }

    public void OpenUI()
    {
        if (_seq != null)
            _seq.Kill();

        _optionCanvasGroup.interactable = false;
        _optionCanvasGroup.blocksRaycasts = false;

        _seq = DOTween.Sequence();
        _seq.Append(transform.DOLocalMoveY(0f, 0.5f)).SetUpdate(true);
    }
}
