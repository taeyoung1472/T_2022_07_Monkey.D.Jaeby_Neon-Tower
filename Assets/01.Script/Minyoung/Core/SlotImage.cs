using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SlotImage : MonoBehaviour
{
    AbilitySO abilitySO;

    private Image _image;

    private Button _btn;


    private void Awake()
    {
        _image = GetComponent<Image>();
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(() => Set());
    }
    public void SetData(AbilitySO so)
    {
        _image.sprite = so.sprite;
        abilitySO = so;
    }
    public void Set()
    {
        Debug.Log(SlotMachineMg.instance.selectImage.sprite);
        SlotMachineMg.instance.selectImage.sprite = abilitySO.sprite; 
        SlotMachineMg.instance.selectTxt.text = abilitySO.abilTxt; 
    }
}
