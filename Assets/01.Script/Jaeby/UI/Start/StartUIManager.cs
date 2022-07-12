using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Cinemachine;

public class StartUIManager : MonoBehaviour
{
    [SerializeField]
    private Vector3 _mainPosition = Vector3.zero;
    [SerializeField]
    private Transform _startInitPosition = null;
    [SerializeField]
    private CinemachineVirtualCamera _vCam = null;

    [SerializeField]
    private GameObject[] _texts = null;

    private void Start()
    {
        _mainPosition = transform.position;

        for(int i =0; i<_texts.Length; i++)
        {
            //_texts[i].transform.DOMoveZ(13f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            Sequence seq = DOTween.Sequence();
            seq.Append(_texts[i].transform.DOMoveZ(13f, 0.5f));
            seq.AppendInterval(0.25f);
            seq.Append(_texts[i].transform.DOMoveZ(14f, 0.5f));
            seq.AppendInterval(0.25f);
            seq.SetLoops(-1);
        }
    }

    [ContextMenu("¾Ó³¢¸ð¶ì")]
    public void StartInit()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_vCam.transform.DOMove(_startInitPosition.position, 1f));
        seq.AppendCallback(() =>
        {
            CameraManager.instance.ZoomCamera(45f, 1f);
        });
    }

}
