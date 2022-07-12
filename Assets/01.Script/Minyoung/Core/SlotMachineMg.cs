using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SlotMachineMg : MonoBehaviour
{
    public static SlotMachineMg instance;
    private Transform _playerSlotObj;
    private Transform _bulletSlotObj;
    private Transform _etcSlotObj;


    private bool _isSpin;
    private bool _isStop;
    private float _delay = 0;

    public ResultButton resultButton;

    public Button resultImageBtn;
    public TextMeshProUGUI explainTxt;
    float timerp = 0f;
    float timerb = 0f;
    float timere = 0f;

    private void Awake()
    {
        instance = this;
        _playerSlotObj = transform.Find("Panel/PlayerButton/SlotObj");
        _bulletSlotObj = transform.Find("Panel/BulletButton/SlotObj");
        _etcSlotObj = transform.Find("Panel/ETCButton/SlotObj");
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_isSpin == true)
            {
                _isStop = true;
            }
            else
            {
                StartCoroutine(StartSpinPlayer(_playerSlotObj));
                StartCoroutine(StartSpinBullet(_bulletSlotObj));
                StartCoroutine(StartSpinETC(_etcSlotObj));
            }

        }
    }

    public void StartRolling()
    {
        if (_isSpin == true)
        {
            _isStop = true;
        }
        else
        {
            StartCoroutine(StartSpinPlayer(_playerSlotObj));
            StartCoroutine(StartSpinBullet(_bulletSlotObj));
            StartCoroutine(StartSpinETC(_etcSlotObj));
        }
        resultImageBtn.enabled = true;
    }
    IEnumerator StartSpinPlayer(Transform target)
    {


        if (_isStop)
        {
            _delay += 0.25f;
            if (_delay >= 1.5f)
            {
                yield break;
            }
        }

        _isSpin = true;
        for (int i = 0; i < target.childCount; i++)
        {
            RectTransform rect = target.GetChild(i).GetComponent<RectTransform>();

            Vector2 origin = rect.anchoredPosition;
            rect.DOAnchorPos(origin + new Vector2(0, -100), 0.1f + _delay).SetEase(Ease.Linear).OnComplete(() =>
            {
                rect.anchoredPosition = origin + new Vector2(0, -100);
                if (rect.anchoredPosition.y <= -50)
                {
                    rect.anchoredPosition = new Vector2(0, (target.childCount - 1) * 100);
                    StartCoroutine(StartSpinPlayer(target));
                }
            });

        }

        yield return null;

    }


    IEnumerator StartSpinBullet(Transform target)
    {
       

        if (_isStop)
        {
            _delay += 0.25f;
            if (_delay >= 1.5f)
            {
                yield break;
            }
        }

        _isSpin = true;
        for (int i = 0; i < target.childCount; i++)
        {
            RectTransform rect = target.GetChild(i).GetComponent<RectTransform>();

            Vector2 origin = rect.anchoredPosition;
            rect.DOAnchorPos(origin + new Vector2(0, -100), 0.1f + _delay).SetEase(Ease.Linear).OnComplete(() =>
            {
                rect.anchoredPosition = origin + new Vector2(0, -100);
                if (rect.anchoredPosition.y <= -50)
                {
                    rect.anchoredPosition = new Vector2(0, (target.childCount - 1) * 100);
                    StartCoroutine(StartSpinBullet(target));
                }
            });

        }

        yield return null;

    }


    IEnumerator StartSpinETC(Transform target)
    {
        if (_isStop)
        {
            _delay += 0.25f;
            if (_delay >= 1.5f)
            {
                yield break;
            }
        }

        _isSpin = true;
        for (int i = 0; i < target.childCount; i++)
        {
            RectTransform rect = target.GetChild(i).GetComponent<RectTransform>();

            Vector2 origin = rect.anchoredPosition;
            rect.DOAnchorPos(origin + new Vector2(0, -100), 0.1f + _delay).SetEase(Ease.Linear).OnComplete(() =>
            {
                rect.anchoredPosition = origin + new Vector2(0, -100);
                if (rect.anchoredPosition.y <= -50)
                {
                    rect.anchoredPosition = new Vector2(0, (target.childCount - 1) * 100);
                    StartCoroutine(StartSpinETC(target));
                }
            });

        }

        yield return null;
    }

    public void InvokeAction(AbilitySO so)
    {
        // if(so.머머머에 해당하는 함수 적용

        GameManagerHan.Instance.damage += so.damage;
        GameManagerHan.Instance.hp += so.hp;
        GameManagerHan.Instance.speed += so.speed;
        _isStop = false;
        _isSpin = false;
        _delay = 0f;
        resultImageBtn.enabled = false;
        resultImageBtn.image.color = new Color(1, 1, 1, 0);
        explainTxt.text = "";
    }

}
