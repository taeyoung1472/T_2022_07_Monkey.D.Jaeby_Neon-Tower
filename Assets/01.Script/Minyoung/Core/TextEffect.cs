using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class TextEffect : MonoBehaviour
{
    public TextMeshProUGUI floorTxt;
    private void OnEnable()
    {
        StartCoroutine(ShakeText(1, 30, 30));
    }
    private IEnumerator ShakeText(float _dur, float _str, int _vib)
    {
        yield return new WaitForSeconds(1f);
        floorTxt.rectTransform.DOShakeRotation(_dur, _str, _vib);
    }
}
