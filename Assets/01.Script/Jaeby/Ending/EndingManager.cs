using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndingManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _earthObj = null;
    [SerializeField]
    private GameObject _playerObj = null;
    [SerializeField]
    private GameObject _ButtonObj = null;

    [SerializeField]
    private Vector3[] _movePos = null;
    [SerializeField]
    private Vector3[] _earthPos = null;
    private int i = 0;
    private int j = 0;

    [SerializeField]
    private Animator _animator = null;
    [SerializeField]
    private AnimationClip _startAni = null;

    [SerializeField]
    private GameObject _button = null;

    private void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(_startAni.length);
        seq.AppendCallback(() =>
        {
            _animator.SetBool("Move", true);
        });
        seq.Append(_playerObj.transform.DOMove(_movePos[i], 3f));
        seq.Join(_earthObj.transform.DOMove(_earthPos[i], 3f));
        seq.AppendCallback(() =>
        {
            _animator.SetBool("Move", false);
        });
        i++;
        j++;
    }


}
