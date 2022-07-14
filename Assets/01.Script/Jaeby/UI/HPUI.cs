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

    public void DisplayValue(int value, int maxValue)
    {
        if (value < 0) return;

        print($"Value : {value}, MaxValue : {maxValue}");

        string str = "";
        str += $"<#{ColorUtility.ToHtmlStringRGBA(_damagedColor)}>";
        for (int i = value; i < maxValue; i++)
        {
            str += "бс ";
        }
        str += "</color>";

        str += $"<#{ColorUtility.ToHtmlStringRGBA(_normalColor)}>";
        for (int i = 0; i < value; i++)
        {
            str += "бс ";
        }
        str += "</color>";

        _text.text = str;
    }


}
