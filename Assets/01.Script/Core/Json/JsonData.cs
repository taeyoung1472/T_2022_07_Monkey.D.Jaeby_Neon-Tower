using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class JsonData
{
    #region Best
    [Header("Best Rank")]
    public int bestTime = 0;
    public int bestKill = 0;
    #endregion

    #region Total
    [Header("Total Rank")]
    public int totalKill = 0;
    public int totalTrain = 0;
    public int totalTime = 0;
    #endregion

    #region Setting
    [Header("Setting")]
    public bool isZoomToggle = true;
    public bool isTimeFreeze = true;
    public bool isPostFx = true;
    #endregion

    public List<ModingData> modingDatas = new List<ModingData>();
}