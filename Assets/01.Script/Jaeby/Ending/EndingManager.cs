using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndingManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _earthObj = null;
    [SerializeField]
    private GameObject _playerObj = null;
    [SerializeField]
    private GameObject _ButtonObj = null;
    [SerializeField]
    private GameObject _lightObj = null;

    [SerializeField]
    private Vector3[] _movePos = null;
    [SerializeField]
    private Vector3[] _earthPos = null;
    [SerializeField]
    private Vector3[] _lightPos = null;
    private int i = 0;
    private int j = 0;
    private int k = 0;

    [SerializeField]
    private Animator _animator = null;
    [SerializeField]
    private AnimationClip _startAni = null;

    [SerializeField]
    private GameObject _button = null;

    [SerializeField]
    private GameObject _boomEffect = null;
    [SerializeField]
    private GameObject _buttonEffect = null;

    [SerializeField]
    private Image _fade = null;
    [SerializeField]
    private GameObject _text = null;
    [SerializeField]
    private Transform _lastPos = null;
    [SerializeField]
    private float _textSpeed = 30f;
    [SerializeField]
    private TextMeshProUGUI _tooltipText = null;
    private float _originSpeed = 0f;

    private bool _look = false;

    private bool _isEnding = false;

    private void Awake()
    {
        Time.timeScale = 1f;
        _text.transform.localPosition = new Vector3(0f, -Screen.height + 200f, 0f);
    }

    private void Start()
    {
        _originSpeed = _textSpeed;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(_startAni.length);
        seq.AppendCallback(() =>
        {
            _animator.SetBool("Move", true);
            _look = true;
        });
        seq.Append(_playerObj.transform.DOMove(_movePos[i], 3f));
        seq.Join(_earthObj.transform.DOMove(_earthPos[j], 3f));
        seq.Join(_lightObj.transform.DOMove(_lightPos[k], 3f));
        seq.AppendCallback(() =>
        {
            _animator.SetBool("Move", false);
            _look = false;
        });
        i++;
        j++;
        k++;

        seq.AppendInterval(1f);
        seq.Append(_playerObj.transform.DOMoveY(_playerObj.transform.position.y + 1f, 0.7f));
        seq.Join(_playerObj.transform.DOMoveX(_button.transform.position.x, 0.7f));
        seq.Join(_playerObj.transform.DOMoveZ(_button.transform.position.z, 0.7f));
        seq.Append(_playerObj.transform.DOMoveY(_playerObj.transform.position.y + -0.2f, 0.3f));
        seq.AppendCallback(() =>
        {
            for(int i = 0; i<3; i++)
            {
                Instantiate(_buttonEffect, _playerObj.transform.position + Vector3.up * 0.8f, Quaternion.identity);
            }
        });


        seq.Append(_button.transform.DOMoveY(_button.transform.position.y - 0.5f, 2f));
        seq.Append(_earthObj.transform.DOMove(_earthPos[j], 2.5f));
        seq.AppendCallback(() =>
        {
            StartCoroutine(OnBoom());
        });

        seq.AppendInterval(2f);
        seq.Append(_fade.DOFade(1f, 1f));
        seq.AppendCallback(() =>
        {
            _tooltipText.enabled = true;
            _isEnding = true;
        });
    }

    private IEnumerator OnBoom()
    {
        for (int i = 0; i < 5; i++)
        {
            Instantiate(_boomEffect, _playerObj.transform.position + Vector3.forward * 20f + Random.insideUnitSphere * 10f, Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void Update()
    {
        if(_look)
        {
            _playerObj.transform.LookAt(_button.transform);
        }

        if(Input.GetKey(KeyCode.Space))
        {
            _textSpeed = _originSpeed * 3f;
        }
        else
        {
            _textSpeed = _originSpeed;
        }

        if(_isEnding)
        {
            _text.transform.Translate(Vector3.up * _textSpeed * Time.deltaTime);
            if(_text.transform.position.y >= _lastPos.position.y)
            {
                SceneManager.LoadScene(0);
            }
        }
    }


}
