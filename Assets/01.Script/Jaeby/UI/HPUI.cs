using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HPUI : MonoBehaviour
{
    [SerializeField]
    private Color _damagedColor = Color.white;
    [SerializeField]
    private Color _normalColor = Color.white;

    private TextMeshProUGUI _text = null;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private int _hp = 10;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            _hp--;
            DisplayHP(_hp, 10);
        }
    }

    public void DisplayHP(int value, int maxValue)
    {
        if (value < 0) return;

        string str = string.Empty;
        str += $"<#{ColorUtility.ToHtmlStringRGBA(_normalColor)}>";
        for (int i = 0; i<value; i++)
        {
            str += "бс ";
        }
        str += "</color>";
        //str += "<#720000>";
        str += $"<#{ColorUtility.ToHtmlStringRGBA(_damagedColor)}>";
        for (int i =0; i<maxValue-value; i++)
        {
            str += "бс ";
        }
        str += "</color>";

        _text.SetText(str);
    }


}
