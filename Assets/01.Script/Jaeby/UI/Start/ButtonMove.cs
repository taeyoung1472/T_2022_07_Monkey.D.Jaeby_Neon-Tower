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
        rect.DOMoveX(150, 0.5f);
        print("ENTER!");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.DOMoveX(0, 0.5f);
        print("EXIT!");
    }
}
