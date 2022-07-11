using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AM : MonoBehaviour
{
    public Image image;

    private void Awake()
    {
    }

    public void Set(AbilitySO abilitySO)
    {
      image.sprite = abilitySO.sprite;
    }
    public void Use()
    { 
    }

}
