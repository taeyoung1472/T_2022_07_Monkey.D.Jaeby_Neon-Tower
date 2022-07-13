using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HPUI : MonoBehaviour
{
    /*
    Display(value, maxvalue){
    string str;
    int idx = 0;
    str += Ȱ��ȭ �÷� �·� �±�
    for(idx < value) str += ��
    str += Ȱ��ȭ �÷� �·� �±�
    str += ��Ȱ��ȭ �÷� �±�
    for(idx < max) str += ��
    str += ��Ȱ��ȭ �÷� �±�
    } 
     */
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
        for(int i = 0; i<value; i++)
        {
            str += "�� ";
        }
        str += "<#720000>";
        for (int i =0; i<maxValue-value; i++)
        {
            str += "�� ";
        }
        str += "</color>";

        _text.SetText(str);
    }


}
