using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class OptionUI : MonoBehaviour, IUserInterface
{
    public UnityEvent OnOpenUI { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    [field:SerializeField]
    public UnityEvent OnCloseUI { get; set; }

    [SerializeField]
    private GameObject _EscUI = null;
    [SerializeField]
    private GameObject _ContinueUI = null;

    private Sequence _seq = null;
    private Vector3 _originPos = Vector3.zero;

    private CanvasGroup _canvasGroup = null;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        _originPos = new Vector3(0f, -Screen.width, 0f);
        transform.localPosition = _originPos;
    }

    public void CloseUI()
    {
        if (_seq != null)
            _seq.Kill();
        _ContinueUI.GetComponent<IUserInterface>().OpenUI();
        _seq = DOTween.Sequence();
        _seq.Append(transform.DOLocalMoveY(_originPos.y, 0.3f)).SetUpdate(true);
        //_seq.AppendCallback(() => { _ContinueUI.GetComponent<IUserInterface>().OpenUI(); });
        
    }

    public void OpenUI()
    {
        if (_seq != null)
            _seq.Kill();

        _canvasGroup.interactable = false;
        _canvasGroup.interactable = false;

        Time.timeScale = 0f;
        Debug.Log("¾¾´í¾Æ");
        _EscUI.GetComponent<IUserInterface>().CloseUI();
        _seq = DOTween.Sequence();
        _seq.AppendInterval(0.3f);
        _seq.Append(transform.DOLocalMoveY(0f, 0.5f)).SetUpdate(true);
        _seq.AppendCallback(() =>
        {
            _canvasGroup.interactable = true;
            _canvasGroup.interactable = true;
        });
    }
}
