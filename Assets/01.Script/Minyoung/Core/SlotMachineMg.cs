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
    float timere = 0f;

    public SlotImage[] slotImages;

    public bool isShow = false;   
    private void Awake()
    {
        instance = this;
        _playerSlotObj = transform.Find("Panel/PlayerButton/SlotObj");
        _bulletSlotObj = transform.Find("Panel/BulletButton/SlotObj");
        _etcSlotObj = transform.Find("Panel/ETCButton/SlotObj");
    }
    private void Start()
    {
         slotImages = transform.Find("Panel").GetComponentsInChildren<SlotImage>();
    }


    private void Update()
    {
        if (isShow)
        {
            timere += Time.unscaledDeltaTime;
            if (timere >= 2f)
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

    }

    public void StartRolling()
    {
        if (_isSpin == true)
        {
            _isStop = true;
        }
        else
        {
            //for (int i = 0; i < slotImages.Length; i++)
            //{
            //  slotImages[i].Push();
            //}
            StartCoroutine(StartSpinPlayer(_playerSlotObj));
            StartCoroutine(StartSpinBullet(_bulletSlotObj));
            StartCoroutine(StartSpinETC(_etcSlotObj));
        }
        resultImageBtn.enabled = true;
    }
    IEnumerator A()
    {
        yield return new WaitForSeconds(5f);
    }

    IEnumerator StartSpinPlayer(Transform target)
    {
        if (_isStop)
        {
            _delay += 0.25f;
            if (_delay >= 1.5f)
            {
                for (int i = 0; i < slotImages.Length; i++)
                {
                    slotImages[i]._btn.enabled = true;
                }
                yield break;
            }
        }

        _isSpin = true;
        for (int i = 0; i < target.childCount; i++)
        {
            RectTransform rect = target.GetChild(i).GetComponent<RectTransform>();

            Vector2 origin = rect.anchoredPosition;
            rect.DOAnchorPos(origin + new Vector2(0, -100), 0.1f + _delay).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
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
                for (int i = 0; i < slotImages.Length; i++)
                {
                    slotImages[i]._btn.enabled = true;
                }
                yield break;
            }
        }

        _isSpin = true;
        for (int i = 0; i < target.childCount; i++)
        {
            RectTransform rect = target.GetChild(i).GetComponent<RectTransform>();

            Vector2 origin = rect.anchoredPosition;
            rect.DOAnchorPos(origin + new Vector2(0, -100), 0.1f + _delay).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
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
                for (int i = 0; i < slotImages.Length; i++)
                {
                    slotImages[i]._btn.enabled = true;
                }
                yield break;
            }
        }

        _isSpin = true;
        for (int i = 0; i < target.childCount; i++)
        {
            RectTransform rect = target.GetChild(i).GetComponent<RectTransform>();

            Vector2 origin = rect.anchoredPosition;
            rect.DOAnchorPos(origin + new Vector2(0, -100), 0.1f + _delay).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
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

        //GameManagerHan.Instance.damage += so.damage;
        //GameManagerHan.Instance.hp += so.hp;
        //GameManagerHan.Instance.speed += so.speed;


        for (int i = 0; i < slotImages.Length; i++)
        {
            slotImages[i]._btn.enabled = false;
        }

        _isStop = false;
        _isSpin = false;
        _delay = 0f;
        resultImageBtn.enabled = false;
        resultImageBtn.image.color = new Color(1, 1, 1, 0);
        explainTxt.text = "";
        timere = 0f;
        isShow = false;
    }

}
