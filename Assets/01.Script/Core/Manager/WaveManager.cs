using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [SerializeField] private float waveTime = 40;
    private float curWaveTime;
    public int curWave;

    [SerializeField] private TextMeshProUGUI nextWaveText;
    [SerializeField] private TextMeshProUGUI floorText;
    [SerializeField] private Background background;

    public void Awake()
    {
        instance = this;

        DisplayFloor();
    }

    public void Update()
    {
        curWaveTime += Time.deltaTime;

        if(curWaveTime > waveTime)
        {
            curWaveTime = 0;
            curWave++;

            if (curWave % 3 == 0)
            {
                background.FloorChange();
                EnemySubject.instance.NotifyObserver();
                CameraManager.instance.CameraShake(1, 1, 4);
                Sequence seq = DOTween.Sequence();
                Define.Instance.controller.GodMode(8);
                seq.AppendCallback(() => ExpManager.instance.isCanLevelup = false);
                seq.AppendCallback(() => EnemyGenerator.Instance.isCanGenerated = false);
                seq.AppendInterval(8);
                seq.AppendCallback(() => ExpManager.instance.isCanLevelup = true);
                seq.AppendCallback(() => EnemyGenerator.Instance.isCanGenerated = true);
            }

            DisplayFloor();
        }

        nextWaveText.text = $"다음 웨이브 까지 : {waveTime - curWaveTime:0.0}초";
    }

    private void DisplayFloor()
    {
        string str = "";
        str += $"{curWave / 3 + 1}층 |";
        for (int i = 0; i < 3; i++)
        {
            if ((curWave + 1) % 3 > i || (curWave + 1) % 3 == 0)
            {
                str += " </color><#ff0000>●</color>";
            }
            else
            {
                str += " ●";
            }
        }
        floorText.text = str;
    }
}
