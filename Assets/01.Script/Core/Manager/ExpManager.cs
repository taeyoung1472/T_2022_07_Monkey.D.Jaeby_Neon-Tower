using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExpManager : MonoBehaviour
{
    public static ExpManager instance;
    public SlotMachineMg slotMachine;
    public UIManagerHan han;

    public int[] expTable;
    int curExp = 0;
    int curLevel = 0;

    public TextMeshProUGUI expPerTxt;
    public TextMeshProUGUI levelTxt;

    [ContextMenu("Init")]
    public void Init()
    {
        expTable = new int[40];
        int dif = 20;
        for (int i = 0; i < expTable.Length; i++)
        {
            if(i == 39)
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
        if(curExp >= expTable[curLevel])
        {
            curExp = 0;
            curLevel++;
            levelTxt.text = curLevel.ToString();
            slotMachine.StartRolling();
            han.OpenLevelMenu();
        }
    }
    public void ExpPercent()
    {
        float expPer = expTable[curLevel] / (curExp + 1);
        expPerTxt.text = expPer.ToString();
    }
}
