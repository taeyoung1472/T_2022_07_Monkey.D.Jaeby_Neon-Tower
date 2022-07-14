using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class ExpManager : MonoBehaviour
{
    public static ExpManager instance;
    public SlotMachineMg slotMachine;
    public UIManagerHan han;
    public SlotMachineManager slotMachineManager;
    public bool isCanLevelup = true;

    [SerializeField] private GameObject levelUpEffect;

    public int[] expTable;
    int curExp = 0;
    int curLevel = 0;

    public TextMeshProUGUI expPerTxt;
    public TextMeshProUGUI levelTxt;


    public Image expImage;

    [ContextMenu("Init")]
    public void Init()
    {
        expTable = new int[40];
        int dif = 20;
        for (int i = 0; i < expTable.Length; i++)
        {
            if (i == 39)
            {
                expTable[39] = 999999;
                return;
            }
            expTable[i] = dif;
            dif += 5;
        }
    }

    public void Awake()
    {
        instance = this;
        LevelUdateText();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            AddExp();
        }
        ExpPercent();
    }

    public void AddExp()
    {
        curExp++;
        if (curExp >= expTable[curLevel] )//&& isCanLevelup)
        {
            curExp = 0;
            curLevel++;
            LevelUdateText();
            Samples.SampleController.instance.ZeroValue();
            slotMachineManager.Make();
            slotMachine.StartRolling();
            slotMachine.gardImage.raycastTarget = true;
            StartCoroutine(RaycastCotroll());
            han.OpenLevelMenu();


            Sequence seq = DOTween.Sequence();

            GameObject e = null;

            seq.AppendInterval(0.25f);

            seq.AppendCallback(() => e = Instantiate(levelUpEffect, Define.Instance.controller.transform.position, Quaternion.identity));

            seq.AppendCallback(() => Destroy(e, 6));

            Define.Instance.controller.GodMode(2f);
        }
    }
    public void LevelUdateText()
    {
        levelTxt.text = ($"LV.{curLevel + 1}");
        expImage.fillAmount = 0f;
    }
    IEnumerator RaycastCotroll()
    {
        yield return new WaitForSecondsRealtime(4);
        slotMachine.gardImage.raycastTarget = false;
    }

    public void ExpPercent()
    {

        float expPer = ((float)curExp / (float)expTable[curLevel]) * 100;
        Sequence sequence = DOTween.Sequence();
        float a = expPer / 100f;

        DOTween.To(() => expImage.fillAmount, x => expImage.fillAmount = x, expPer / 100f, 0.3f).SetUpdate(true);
        expPerTxt.text = ($"{Mathf.Ceil(expPer)}");
    }
}
