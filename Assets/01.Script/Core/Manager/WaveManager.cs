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
    public int realcurWave = 1;
    public int curFloor = 1;

    [SerializeField] private TextMeshProUGUI nextWaveText; //�ְ� ���̺� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI floorText;  //�갡 �÷ξ� �ؽ�Ʈ
    [SerializeField] private Background background;

    [SerializeField] private TextMeshProUGUI wave;
    [SerializeField] private TextMeshProUGUI floor;

    public void Awake()
    {
        instance = this;

        DisplayFloor();
    }
    private void Start()
    {
        wave.text = $"Wave 1-{curFloor}";
        wave.gameObject.SetActive(true);
        StartCoroutine(DisableText());
    }
    public void Update()
    {
        curWaveTime += Time.deltaTime;

        if(curWaveTime > waveTime)
        {
            curWaveTime = 0;
            curWave++;
            curFloor++;
            if (curWave % 3 == 0)
            {
                realcurWave++;
                curFloor = 1;
                background.FloorChange();
                EnemySubject.instance.NotifyObserver();
                CameraManager.instance.CameraShake(1, 1, 4);
                //����ٰ� ���̺� �ö󰡴� �ؽ�Ʈ
                //wave.text = $"Wave {curWave}-{curFloor}";
                //wave.gameObject.SetActive(true);

                Sequence seq = DOTween.Sequence();
                Define.Instance.controller.GodMode(8);
                seq.AppendCallback(() => ExpManager.instance.isCanLevelup = false);
                seq.AppendCallback(() => EnemyGenerator.Instance.isCanGenerated = false);
                seq.AppendInterval(8);
                seq.AppendCallback(() => ExpManager.instance.isCanLevelup = true);
                seq.AppendCallback(() => EnemyGenerator.Instance.isCanGenerated = true);
            }

            wave.text = $"Wave {realcurWave}-{curFloor}";
            wave.gameObject.SetActive(true);
            StartCoroutine(DisableText());

            DisplayFloor();
        }

        nextWaveText.text = $"Next Wave : {waveTime - curWaveTime:0.0} Sec";
    }
    public int GetFloor()
    {
        return curWave / 3 + 1;
    }
    IEnumerator DisableText()
    {
        yield return new WaitForSeconds(3f);
        wave.gameObject.SetActive(false);
    }
    private void DisplayFloor()
    {
        string str = "";
        str += $"{curWave / 3 + 1} Floor";
        if(curWave / 3 + 1 == 6)
        {
            UIManager.Instance.GameEnding();
        }
        /*for (int i = 0; i < 3; i++)
        {
            if ((curWave + 1) % 3 > i || (curWave + 1) % 3 == 0)
            {
                str += " </color><#ff0000>��</color>";
            }
            else
            {
                str += " ��";
            }
        }*/
        floorText.text = str;
    }
}
