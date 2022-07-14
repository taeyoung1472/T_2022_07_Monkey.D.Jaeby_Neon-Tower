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

    public int[] expTable;
    int curExp = 0;
    int curLevel = 0;

    public TextMeshProUGUI expPerTxt;
    public TextMeshProUGUI levelTxt;

    public Slider expSlider;

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
        if (curExp >= expTable[curLevel])
        {
            curExp = 0;
            curLevel++;
            LevelUdateText();
            slotMachineManager.Make();
            slotMachine.StartRolling();
            slotMachine.gardImage.raycastTarget = true;
            StartCoroutine(RaycastCotroll());
            han.OpenLevelMenu();
        }
    }
    public void LevelUdateText()
    {
        levelTxt.text = ($"LV.{curLevel + 1}");
        expImage.fillAmount = 0f;
        //expSlider.value = 0;
        //expSlider.maxValue = expTable[curLevel];
        //levelFillAmontImage.fillAmount = 0;
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

        DOTween.To(() => expImage.fillAmount, x => expImage.fillAmount = x, expPer / 100f, 0.3f);
        //expSlider.value = Mathf.Lerp(expSlider.value, curExp, Time.deltaTime * 10);
        expPerTxt.text = ($"{Mathf.Ceil(expPer)}");
    }
}
