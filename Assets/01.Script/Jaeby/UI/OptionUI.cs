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

    [SerializeField]
    private GameObject[] _directerNames = null;
    private bool _directerNameOn = false;

    [SerializeField]
    private GameObject[] _menNames = null;
    private bool _menNameOn = false;

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

        if(_menNameOn)
        {
            _menNameOn = false;
            CloseMen();
        }
        if(_directerNameOn)
        {
            _directerNameOn = false;
            CloseName();
        }
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

    public void OpenName()
    {
        if (_directerNameOn)
        {
            CloseName();
            _directerNameOn = false;
            return;
        }
        _directerNameOn = true;

        for (int i = 0; i<_directerNames.Length; i++)
        {
            _directerNames[i].SetActive(true);
            _directerNames[i].transform.position += Vector3.up * 130f;
        }
    }


    public void CloseName()
    {
        for (int i = _directerNames.Length - 1; i >= 0; i--)
        {
            _directerNames[i].SetActive(false);
            _directerNames[i].transform.position += Vector3.up * -130f;
        }
    }

    public void OpenMen()
    {
        if(_menNameOn)
        {
            CloseMen();
            _menNameOn = false;
            return;
        }
        _menNameOn = true;

        for (int i = 0; i < _menNames.Length; i++)
        {
            _menNames[i].SetActive(true);
            _menNames[i].transform.position += Vector3.up * 130f;
        }
    }

    public void CloseMen()
    {
        for (int i = _menNames.Length -1 ; i >= 0; i--)
        {
            _menNames[i].SetActive(false);
            _menNames[i].transform.position += Vector3.up * -130f;
        }
    }

}
