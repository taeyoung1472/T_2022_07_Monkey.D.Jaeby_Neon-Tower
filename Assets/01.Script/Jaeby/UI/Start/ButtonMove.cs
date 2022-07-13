using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Events;

public class ButtonMove : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    RectTransform rect;

    [field: SerializeField]
    private UnityEvent OnEnterSound = null;
    [field: SerializeField]
    private UnityEvent OnExitSound = null;


    public void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (StartUIManager.instance.IsClicked)
            return;

        rect.DOAnchorPosX(100f, 0.25f);
        OnEnterSound?.Invoke();
        print("ENTER!");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (StartUIManager.instance.IsClicked)
            return;

        rect.DOAnchorPosX(0, 0.25f);
        OnExitSound?.Invoke();
        print("EXIT!");
    }
}
