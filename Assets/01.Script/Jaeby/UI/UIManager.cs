using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _escUI = null;

    [SerializeField]
    private bool _openUI = false;

    [SerializeField]
    private AudioClip _lightClick = null;
    [SerializeField]
    private AudioClip _middleClick = null;
    [SerializeField]
    private AudioClip _HardClick = null;

    private void Update()
    {
        if (_openUI) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _escUI.GetComponent<IUserInterface>().OpenUI();
            _openUI = true;
        }
       // PoolManager.instance.Pop(PoolType.Sound).GetComponent<AudioPoolObject>().Play();
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

    public void GameExit()
    {
        Application.Quit();
    }


    public void OpenUIEnable ()
    {
        _openUI = true;
    }
    public void OpenUIDisable()
    {
        _openUI = false;
    }


    public void LightClickSoundPlay()
    {
        PoolManager.instance.Pop(PoolType.Sound).GetComponent<AudioPoolObject>().Play(_lightClick);
    }
    public void MiddleClickSoundPlay()
    {
        PoolManager.instance.Pop(PoolType.Sound).GetComponent<AudioPoolObject>().Play(_middleClick);
    }
    public void HardClickSoundPlay()
    {
        PoolManager.instance.Pop(PoolType.Sound).GetComponent<AudioPoolObject>().Play(_HardClick);
    }
}
