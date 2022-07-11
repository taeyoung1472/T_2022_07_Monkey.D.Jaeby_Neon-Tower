using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ResultButton : MonoBehaviour
{
    private Button _btn;

    private Image _image = null;
    private Text _text = null;
    private AbilitySO _so = null;

    private void Awake()
    {
        _btn = GetComponent<Button>();
        _image = GetComponent<Image>();
        _text = transform.Find("Text").GetComponent<Text>();

        _btn.onClick.AddListener(() =>
        {
            SlotMachineMg.instance.InvokeAction(_so);
        });
    }

    public void SetData(AbilitySO so)
    {
        _so = so;
        _image.sprite = so.sprite;
        _text.text = so.abilTxt;
        
    }

}
