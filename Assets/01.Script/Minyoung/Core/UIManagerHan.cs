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
        levelUPMenu.DOAnchorPos(Vector2.zero, 0.25f);
    }
    public void CloseLevelMenu()
    {
        levelUPMenu.DOAnchorPos(new Vector2(0, 1070), 0.25f);
    }
}
