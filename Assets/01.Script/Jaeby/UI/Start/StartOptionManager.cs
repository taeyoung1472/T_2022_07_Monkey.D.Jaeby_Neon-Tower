using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StartOptionManager : MonoBehaviour
{
    [SerializeField]
    private Samples.SampleController _sampleController = null;
    [SerializeField]
    private RectTransform _optionUI = null;
    private Vector3 _origin = Vector3.zero;


    private void Start()
    {
        _sampleController.ZeroValue();
        _origin = _optionUI.position;
    }

    public void OpenOption()
    {
        _optionUI.DOLocalMoveY(0f, 0.5f);
    }
    public void ExitOption()
    {
        _optionUI.DOLocalMoveY(_origin.y, 0.5f);
    }
}
