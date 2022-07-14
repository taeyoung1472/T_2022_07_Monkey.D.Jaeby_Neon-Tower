using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
public class ResultButton : MonoBehaviour
{
    private Button _btn;

    private Image _image = null;
    public TextMeshProUGUI _text = null;
    public SlotMachineManager slotMachine;
    private AbilitySO _so = null;

    private void Awake()
    {
        _btn = GetComponent<Button>();
        _image = GetComponent<Image>();

        _btn.onClick.AddListener(() =>
        {
            SlotMachineMg.instance.InvokeAction(_so);
            slotMachine.DestroyUpgradeTable(_so);
        });
    }

    public void SetData(AbilitySO so)
    {
        _so = so;
        //_image.sprite = so.sprite;
        _text.text = so.descString;
    }

}
