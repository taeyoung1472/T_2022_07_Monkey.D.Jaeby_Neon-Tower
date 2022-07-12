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

    [SerializeField]
    private GameObject _startButton = null;
    [SerializeField]
    private GameObject _exitButton = null;

    [SerializeField]
    private GameObject _textParent = null;

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

    public void FadeButton()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_startButton.transform.DOMoveX(-800f, 0.2f));
        seq.Append(_exitButton.transform.DOMoveX(-800f, 0.2f));
        seq.AppendCallback(() => StartInit());
    }

    public void StartInit()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_vCam.transform.DOMove(_startInitPosition.position, 1f));
        seq.AppendCallback(() =>
        {
            CameraManager.instance.ZoomCamera(45f, 1f);
        });
        seq.AppendInterval(1f);
        seq.AppendCallback(() =>
        {
            SceneManager.LoadScene("");
        });
    }

    public void ExitInit()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_startButton.transform.DOMoveX(-800f, 0.2f));
        seq.Append(_exitButton.transform.DOMoveX(-800f, 0.2f));
        seq.Append(_textParent.transform.DOMoveY(-14f, 0.5f));
        Application.Quit();
    }
}
