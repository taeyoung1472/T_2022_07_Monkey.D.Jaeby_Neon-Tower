using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDisplay : MonoBehaviour
{
    public static UpgradeDisplay instance;
    List<AbilitySO> soList = new List<AbilitySO>();
    public GridUpgrade gridUp;
    public Transform gridDisplayer;
    public void Awake()
    {
        instance = this;
    }
    public void Regist(AbilitySO so)
    {
        if (!soList.Contains(so))
        {
            GridUpgrade upgrade = Instantiate(gridUp, gridDisplayer);
            upgrade.Set(so.sprite);
            upgrade.gameObject.SetActive(true);
            soList.Add(so);
        }
    }
}
