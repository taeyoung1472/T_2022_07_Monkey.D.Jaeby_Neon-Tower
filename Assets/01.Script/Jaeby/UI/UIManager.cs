using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _escUI = null;

    private bool _openUI = false;

    private void Update()
    {
        if (_openUI) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _escUI.GetComponent<IUserInterface>().OpenUI();
            _openUI = true;
        }
    }

    public void OpenUI(CanvasGroup canvasGroup)
    {
        _openUI = true;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void CloseUI(CanvasGroup canvasGroup)
    {
        _openUI = false;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }


    public void OpenUIEnable ()
    {
        _openUI = true;
    }
    public void OpenUIDisable()
    {
        _openUI = false;
    }
}
