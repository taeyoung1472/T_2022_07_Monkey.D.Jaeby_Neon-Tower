using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManagerHan : MonoBehaviour
{
    public RectTransform levelUPMenu;
    private void Start()
    {
    }

    public void OpenLevelMenu()
    {
        Time.timeScale = 0f;
        levelUPMenu.DOAnchorPos(Vector2.zero, 0.25f).SetUpdate(true);
        SlotMachineMg.instance.isShow = true;
    }
    public void CloseLevelMenu()
    {
        levelUPMenu.DOAnchorPos(new Vector2(0, 1070), 0.25f).SetUpdate(true);
        Time.timeScale = 1f;
    }
}
