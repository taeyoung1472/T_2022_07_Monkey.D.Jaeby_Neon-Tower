using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class SlotMachineMg : MonoBehaviour
{
    public static SlotMachineMg instance;
    private Transform _playerSlotObj;
    private Transform _bulletSlotObj;
    private Transform _etcSlotObj;


    private bool _isSpin;
    private bool _isStop;
    private float _delay = 0;

    public Image selectImage;
    public Text selectTxt;

    float timerp = 0f;
    float timerb = 0f;
    float timere = 0f;

    public Button useUpgradeBtn;
    private void Awake()
    {
        instance = this;
        _playerSlotObj = transform.Find("Panel/PlayerButton/SlotObj");
        _bulletSlotObj = transform.Find("Panel/BulletButton/SlotObj");
        _etcSlotObj = transform.Find("Panel/ETCButton/SlotObj");

    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
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

    IEnumerator StartSpinPlayer(Transform target)
    {
        timerp += Time.time;
        Debug.Log(timerp);
        if (timerp > 3f)
        {
            _isStop = true;
        }

        if (_isStop)
        {
            _delay += 0.25f;
            if(_delay >= 1.5f)
            {
                yield break;
            }
        }

        _isSpin = true;
        for (int i = 0; i < target.childCount; i++)
        {
            RectTransform rect = target.GetChild(i).GetComponent<RectTransform>();

            Vector2 origin = rect.anchoredPosition;
            rect.DOAnchorPos(origin + new Vector2(0, -100), 0.2f + _delay).SetEase(Ease.Linear).OnComplete(() =>
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
        timerb += Time.time;
        if (timerb > 4f)
        {
            _isStop = true;
        }

        if (_isStop)
        {
            _delay += 0.25f;
            if(_delay >= 1.5f)
            {
                yield break;
            }
        }

        _isSpin = true;
        for (int i = 0; i < target.childCount; i++)
        {
            RectTransform rect = target.GetChild(i).GetComponent<RectTransform>();

            Vector2 origin = rect.anchoredPosition;
            rect.DOAnchorPos(origin + new Vector2(0, -100), 0.2f + _delay).SetEase(Ease.Linear).OnComplete(() =>
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
        timere += Time.time;
        if (timere > 20f)
        {
            _isStop = true;
        }

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
            rect.DOAnchorPos(origin + new Vector2(0, -100), 0.2f + _delay).SetEase(Ease.Linear).OnComplete(() =>
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

    public void Use(AbilitySO so)
    {
        useUpgradeBtn.onClick.AddListener(()=> Upgrade(so));
    }
    public void Upgrade(AbilitySO so)
    {
        GameManager.Instance.damage += so.damage;
        GameManager.Instance.hp += so.hp;
        GameManager.Instance.speed += so.speed;
        Debug.Log("Sex");
    }

}
