using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectImage : MonoBehaviour
{
    public Image _image;

    public void SetData(AbilitySO so)
    {
        _image.sprite = so.sprite;
    }

}
