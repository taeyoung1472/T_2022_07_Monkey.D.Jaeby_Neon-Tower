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
    private RectTransform _startButton = null;
    [SerializeField]
    private RectTransform _exitButton = null;

    [SerializeField]
    private GameObject _textParent = null;

    [SerializeField]
    private GameObject[] _lights = null;

    public static StartUIManager instance = null;
    private bool _isClicked = false;
    public bool IsClicked { get => _isClicked; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;

        _mainPosition = transform.position;

        for(int i =0; i<_texts.Length; i++)
        {
            //_texts[i].transform.DOMoveZ(13f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            Sequence seq = DOTween.Sequence();
            seq.Append(_texts[i].transform.DOMoveZ(13f, 0.5f)).SetUpdate(true);
            seq.AppendInterval(0.25f).SetUpdate(true);
            seq.Append(_texts[i].transform.DOMoveZ(14f, 0.5f)).SetUpdate(true);
            seq.AppendInterval(0.25f).SetUpdate(true);
            seq.SetLoops(-1);
        }
    }

    public void FadeButton()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_startButton.DOAnchorPosX(-800f, 0.2f));
        seq.Append(_exitButton.DOAnchorPosX(-800f, 0.2f));
        seq.AppendCallback(() => StartInit());
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }

    public void StartInit()
    {
        //StartCoroutine(LightDown());

        Sequence seq = DOTween.Sequence();
        //seq.AppendInterval(0.2f * _lights.Length);
        seq.Append(_vCam.transform.DOMove(_startInitPosition.position, 1.5f));
        seq.AppendCallback(() =>
        {
            CameraManager.instance.ZoomCamera(45f, 0.5f);
        });
        seq.AppendInterval(1f);
        seq.AppendCallback(() =>
        {
            SceneManager.LoadScene(1);
        });
    }

    /*private IEnumerator LightDown()
    {
        for(int i = 0; i<_lights.Length; i++)
        {
            _lights[i].SetActive(false);
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }*/

    public void ExitInit()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_startButton.DOAnchorPosX(-800f, 0.2f));
        seq.Append(_exitButton.DOAnchorPosX(-800f, 0.2f));
        seq.AppendInterval(0.4f);
        seq.Append(_textParent.transform.DOMoveY(-14f, 0.5f));
        Application.Quit();
    }

    public void IsClick()
    {
        _isClicked = true;
    }
}
