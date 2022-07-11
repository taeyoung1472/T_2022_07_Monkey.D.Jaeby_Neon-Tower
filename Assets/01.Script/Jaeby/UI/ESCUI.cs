using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class ESCUI : MonoBehaviour, IUserInterface
{
    private Vector2 _initPos = Vector2.zero;

    private Sequence _seq = null;

    [field:SerializeField]
    public UnityEvent OnOpenUI { get; set; }
    [field: SerializeField]
    public UnityEvent OnCloseUI { get; set; }

    public void CloseUI()
    {
        if (_seq != null)
            _seq.Kill();

        Time.timeScale = 1f;
        _seq = DOTween.Sequence();
        _seq.Append(transform.DOLocalMove(_initPos, 0.5f));
        _seq.AppendCallback(() =>
        {
            Debug.Log("¾Ó ±â¸ð¶ì");
        });
        OnCloseUI?.Invoke();
    }

    public void OpenUI()
    {
        if (_seq != null)
            _seq.Kill();

        _seq = DOTween.Sequence();
        _seq.Append(transform.DOLocalMove(Vector3.zero, 0.5f));
        _seq.AppendCallback(()=> 
            { 
                Debug.Log("¾Ó ±â¸ð¶ì"); 
                Time.timeScale = 0f; 
            });
        OnOpenUI?.Invoke();
    }

    private void Start()
    {
        _initPos = new Vector2(-Screen.width, 0f);
        transform.localPosition = _initPos;
    }

    public void ReStart()
    {
        CloseUI();
    }

}
