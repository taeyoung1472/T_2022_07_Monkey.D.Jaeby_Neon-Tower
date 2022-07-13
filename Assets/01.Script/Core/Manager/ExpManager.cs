using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpManager : MonoBehaviour
{
    public static ExpManager instance;
    public SlotMachineMg slotMachine;
    public UIManagerHan han;

    public int[] expTable;
    int curExp = 0;
    int curLevel = 0;

    [ContextMenu("Init")]
    public void Init()
    {
        expTable = new int[45];
        int dif = 12;
        for (int i = 0; i < expTable.Length; i++)
        {
            expTable[i] = dif;
            dif += 2;
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
    }

    public void AddExp()
    {
        curExp++;
        if(curExp >= expTable[curLevel])
        {
            curExp = 0;
            curLevel++;
            slotMachine.StartRolling();
            han.OpenLevelMenu();
        }
    }
}
