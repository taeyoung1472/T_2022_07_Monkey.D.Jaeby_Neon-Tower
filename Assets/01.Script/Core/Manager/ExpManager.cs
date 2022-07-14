using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpManager : MonoBehaviour
{
    public static ExpManager instance;
    public SlotMachineMg slotMachine;
    public UIManagerHan han;
    public SlotMachineManager slotMachineManager;

    public int[] expTable;
    int curExp = 0;
    int curLevel = 0;

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
    }

    public void AddExp()
    {
        curExp++;
        if(curExp >= expTable[curLevel])
        {
            curExp = 0;
            curLevel++;
            slotMachineManager.Make();
            slotMachine.StartRolling();
            slotMachine.gardImage.raycastTarget = true;
            StartCoroutine(RaycastCotroll());
            han.OpenLevelMenu();
        }
    }

    IEnumerator RaycastCotroll()
    {
        yield return new WaitForSecondsRealtime(4);
        slotMachine.gardImage.raycastTarget = false;
    }
}
