using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonMove : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    RectTransform rect;

    public void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (StartUIManager.instance.IsClicked)
            return;

        rect.DOAnchorPosX(100f, 0.25f);
        print("ENTER!");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (StartUIManager.instance.IsClicked)
            return;

        rect.DOAnchorPosX(0, 0.25f);
        print("EXIT!");
    }
}
