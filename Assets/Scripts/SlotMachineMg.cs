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

    public ResultButton resultButton;

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
            //StartCoroutine(Spin(_playerSlotObj, 2f));
        }
    }

    IEnumerator Spin(Transform tgt, float spinTime)
    {
        #region 스핀
        for (int i = 0; i < tgt.childCount; i++)
        {
            RectTransform rect = tgt.GetChild(i).GetComponent<RectTransform>();

            Vector2 origin = rect.anchoredPosition;
            int randIdx = Random.Range(3, 6);
            rect.DOAnchorPos(origin + new Vector2(0, -100 * randIdx), 0.2f + spinTime).OnComplete(() =>
            {
                rect.anchoredPosition = origin + new Vector2(0, -100 * randIdx);
            });
        }
        #endregion

        #region 타이머
        float time = 0;
        while (time > spinTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        #endregion
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

    public void InvokeAction(AbilitySO so)
    {
        // if(so.머머머에 해당하는 함수 적용

        GameManager.Instance.damage += so.damage;
        GameManager.Instance.hp += so.hp;
        GameManager.Instance.speed += so.speed;
    }

}
