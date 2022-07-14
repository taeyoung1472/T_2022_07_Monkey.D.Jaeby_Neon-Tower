using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class SlotImage : MonoBehaviour
{
    AbilitySO abilitySO;

    private Image _image;

    public Button _btn;

    
    private void Awake()
    {
        _image = GetComponent<Image>();
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(() =>
        {
            Set();
        });
    }

    int Index;
    public void SetData(int index, AbilitySO so)
    {
        Index = index;
        _image.sprite = so.sprite;
        abilitySO = so;
    }
    public void Set()
    {
        SlotMachineMg.instance.resultImageBtn.image.color = new Color(1, 1, 1, 1);
        SlotMachineMg.instance.resultButton.SetData(abilitySO);
       SlotMachineMg.instance.resultImageBtn.enabled = true;
        //this._image.material = SlotMachineMg.instance.selectMat;
        var list = new List<Image>
        {
            SlotMachineMg.instance.select1,
            SlotMachineMg.instance.select2,
            SlotMachineMg.instance.select3
        };

        for (int i = 0; i < 3; i ++)
        {
            if(Index ==i)
                list[i].material = SlotMachineMg.instance.selectMat;
            else
                list[i].material = SlotMachineMg.instance.orignMat;
        }
        
    }

}
