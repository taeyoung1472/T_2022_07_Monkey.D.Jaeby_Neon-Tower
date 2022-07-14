using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class WaringTextEffect : MonoBehaviour
{
    public TextMeshProUGUI floorTxt;

    private void OnEnable()
    {
        TextEff();
    }
    void TextEff()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.3f);
        seq.Append(floorTxt.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f));
        seq.AppendInterval(0.5f);
        seq.Append(floorTxt.transform.DOScale(new Vector3(1, 1, 1), 0.5f));
    }

}
