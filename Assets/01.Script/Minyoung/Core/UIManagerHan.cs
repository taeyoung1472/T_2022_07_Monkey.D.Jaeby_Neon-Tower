using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManagerHan : MonoBehaviour
{
    public RectTransform levelUPMenu;

    public RectTransform selectDetail;

    public Image rotateSelect;
    private void Start()
    {
    }

    public void OpenSelectMenu()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.6f);
        seq.Append(selectDetail.DOAnchorPos(new Vector2(0, 160), 0.5f).SetUpdate(true));
        seq.Append(selectDetail.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f)).SetUpdate(true);
        seq.Append(rotateSelect.transform.DORotate(new Vector3(0, 0, 180), 0.5f)).SetUpdate(true);
        seq.AppendInterval(0.5f).SetUpdate(true);
        seq.Append(selectDetail.DOScale(Vector3.one, 0.3f)).SetUpdate(true);
        seq.Append(rotateSelect.transform.DORotate(new Vector3(0, 0, 0), 0.3f)).SetUpdate(true);
        seq.AppendInterval(0.5f).SetUpdate(true);
        seq.Join(selectDetail.DOAnchorPos(new Vector2(0, -160), 0.5f).SetUpdate(true));
    }

    public void OpenLevelMenu()
    {
        Time.timeScale = 0f;
        levelUPMenu.DOAnchorPos(Vector2.zero, 0.5f).SetUpdate(true);
        SlotMachineMg.instance.isShow = true;
    }
    public void CloseLevelMenu()
    {
        levelUPMenu.DOAnchorPos(new Vector2(0, 1070), 0.5f).SetUpdate(true);
        Time.timeScale = 1f;
    }
}
