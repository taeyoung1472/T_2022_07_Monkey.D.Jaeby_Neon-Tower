using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class DieUI : MonoBehaviour, IUserInterface
{
    public UnityEvent OnOpenUI { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public UnityEvent OnCloseUI { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private Vector3 _originPos = Vector3.zero;
    [SerializeField]
    private GameObject _panel = null;

    private void Start()
    {
        _originPos = new Vector3(0f, -Screen.height, 0f);
        _panel.transform.localPosition = _originPos;
    }

    public void CloseUI()
    {
        throw new System.NotImplementedException();
    }

    public void OpenUI()
    {
        Time.timeScale = 0f;
        _panel.transform.DOLocalMoveY(0f, 0.5f).SetUpdate(true);
    }
}
