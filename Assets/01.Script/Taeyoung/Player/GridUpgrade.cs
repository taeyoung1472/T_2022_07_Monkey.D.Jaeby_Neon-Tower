using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridUpgrade : MonoBehaviour
{
    [SerializeField] private Image image;
    public void Set(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
