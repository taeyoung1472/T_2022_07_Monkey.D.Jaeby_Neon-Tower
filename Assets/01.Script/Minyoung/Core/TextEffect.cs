using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class TextEffect : MonoBehaviour
{
    public TextMeshProUGUI floorTxt;
    public float dur;
    public float str;
    public int vib;
    private void OnEnable()
    {
        StartCoroutine(ShakeText(dur, str, vib));
    }
    private IEnumerator ShakeText(float _dur, float _str, int _vib)
    {
        yield return new WaitForSeconds(1f);
        floorTxt.rectTransform.DOShakeRotation(_dur, _str, _vib);
    }
}
