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
    public Image gardImage;
    float timere = 0f;

    public SlotImage[] slotImages;

    public SlotImage maxImage;

    public bool isShow = false;

    public Image selectImage;
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
                    StartCoroutine(StartSpin(_playerSlotObj));
                    StartCoroutine(StartSpin(_bulletSlotObj));
                    StartCoroutine(StartSpin(_etcSlotObj));
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
            StartCoroutine(StartSpin(_playerSlotObj));
            StartCoroutine(StartSpin(_bulletSlotObj));
            StartCoroutine(StartSpin(_etcSlotObj));
        }
        resultImageBtn.enabled = true;
    }

    IEnumerator StartSpin(Transform target)
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
                    StartCoroutine(StartSpin(target));
                }

            });

        }

        yield return null;
    }

    public void InvokeAction(AbilitySO so)
    {
        GameManager manager = GameManager.Instance;

        if (so.damage) manager.bulletStat.damage++;
        if (so.attackSpeed) manager.bulletStat.bulletDelay++;
        if (so.maxHp) manager.playerStat.hp++;
        if (so.autoHeal) manager.playerStat.autoHealDelay++;
        if (so.bulletSpeed) manager.bulletStat.bulletSpd++;
        if (so.multiShoot) manager.bulletStat.multiShotCount++;
        if (so.penetration) manager.bulletStat.penetrationShot++;
        if (so.bulletBounce) manager.bulletStat.wallBounceCnt++;
        if (so.knockback) manager.bulletStat.knockback++;
        if (so.bulletFreeze) manager.bulletStat.freeze++;
        if (so.bulletExplosion) manager.bulletStat.explosion++;
        if (so.stealHp) manager.playerStat.stealHp++;
        if (so.dashChance) manager.playerStat.dashChance++;
        if (so.dashGod) manager.playerStat.dashGod++;


        selectImage.sprite = so.sprite;

        Init();
    }
    public void Init()
    {
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
