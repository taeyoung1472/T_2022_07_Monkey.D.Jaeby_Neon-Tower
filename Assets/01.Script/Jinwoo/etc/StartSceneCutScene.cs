using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEngine.UI;

public class StartSceneCutScene : MonoBehaviour
{
    public Image img;


    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1f);
        Sequence seq = DOTween.Sequence();

        seq.Append(img.DOFade(0, 1f));
        
        Samples.SampleController.instance.ChangeRenderModeOne();
        Samples.SampleController.instance.StartGameCutScene();
        seq.AppendCallback(() =>
        {
            Samples.SampleController.instance.ZeroValue();
            //Samples.SampleController.instance.ChangeRenderModeZero();

        });

        yield return new WaitForSeconds(1f);
    }
}
