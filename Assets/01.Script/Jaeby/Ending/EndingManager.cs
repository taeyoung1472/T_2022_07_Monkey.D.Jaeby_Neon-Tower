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

    private void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(4f);
        seq.Append(_playerObj.transform.DOMove(_movePos[i], 3f));
        seq.Join(_earthObj.transform.DOMove(_earthPos[i], 3f));
        i++;
        j++;
    }


}
