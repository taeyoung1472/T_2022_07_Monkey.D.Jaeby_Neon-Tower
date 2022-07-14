using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _escUI = null;
    [SerializeField]
    private GameObject _dieUI = null;
    [SerializeField]
    private GameObject _fadeUI = null;

    [SerializeField]
    private bool _openUI = false;

    [SerializeField]
    private AudioClip _lightClick = null;
    [SerializeField]
    private AudioClip _middleClick = null;
    [SerializeField]
    private AudioClip _HardClick = null;

    [Header("HPUI °ü·Ã")]
    [SerializeField]
    private Color _damagedColor = Color.white;
    [SerializeField]
    private Color _normalColor = Color.white;
    [SerializeField]
    private TextMeshProUGUI _text = null;

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
        _fadeUI.SetActive(true);
        Sequence seq = DOTween.Sequence();
        seq.Append(_fadeUI.GetComponent<Image>().DOFade(1f, 1f)).SetUpdate(true);
        seq.AppendCallback(() =>
        {
            SceneManager.LoadScene(0);
        });
    }

    public void GameReStart()
    {
        _fadeUI.SetActive(true);
        Sequence seq = DOTween.Sequence();
        seq.Append(_fadeUI.GetComponent<Image>().DOFade(1f, 1f)).SetUpdate(true);
        seq.AppendCallback(() =>
        {
            SceneManager.LoadScene(1);
        });
    }

    public void GameEnding()
    {
        Time.timeScale = 0f;

        _fadeUI.SetActive(true);
        _fadeUI.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        Sequence seq = DOTween.Sequence();
        seq.Append(_fadeUI.GetComponent<Image>().DOFade(1f, 1f)).SetUpdate(true);
        seq.AppendCallback(() =>
        {
            SceneManager.LoadScene(2);
        });
    }

    public void DieUISet()
    {
        _openUI = true;
        _dieUI.GetComponent<IUserInterface>().OpenUI();
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



    public void DisplayHP(int value, int maxValue)
    {
        if (value < 0) return;

        string str = string.Empty;
        str += $"<#{ColorUtility.ToHtmlStringRGBA(_normalColor)}>";
        for (int i = 0; i < value; i++)
        {
            str += "¡á ";
        }
        str += "</color>";
        //str += "<#720000>";
        str += $"<#{ColorUtility.ToHtmlStringRGBA(_damagedColor)}>";
        for (int i = 0; i < maxValue - value; i++)
        {
            str += "¡á ";
        }
        str += "</color>";

        _text.SetText(str);
    }
}
