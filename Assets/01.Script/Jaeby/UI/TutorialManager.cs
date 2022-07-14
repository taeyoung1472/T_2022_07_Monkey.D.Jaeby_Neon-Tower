using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _tutorialText = null;
    [SerializeField]
    private Transform _tutorialTextPos = null;
    [SerializeField]
    private Vector3 _initPos = Vector3.zero;
    #region 튜토1
    private int _amount = 0;
    #endregion

    private bool _tutorialStart = false;
    [SerializeField]
    private GameObject _colEffect = null;
    [SerializeField]
    private GameObject[] _test1Boundarys = null;
    [SerializeField]
    private GameObject[] _test2Boundarys = null;
    [SerializeField]
    private GameObject[] _test3Boundarys = null;
    [SerializeField]
    private GameObject[] _enemyPrefabs = null;

    [SerializeField]
    private Color _impactColor = Color.white;

    private void Start()
    {
        Time.timeScale = 1f;

        StartCoroutine(StartTutorial());
    }


    public void amountUp(Collider col)
    {
        if (_tutorialStart == false) return;

        Instantiate(_colEffect, col.transform.position, Quaternion.identity);
        col.gameObject.SetActive(false);

        _amount++;
        if(_amount >= 4)
        {
            TextPop("잘 하셨습니다!!");
            StartCoroutine(GoTutorialTwo());
            // 클리어
        }
    }

    public void TestTwoStart(Collider col)
    {
        StartCoroutine(TutorialTwoStart());
        col.gameObject.SetActive(false);
    }

    private void TextPop(string text)
    {
        DOTween.KillAll();

        _tutorialText.SetText(text);
        _tutorialText.rectTransform.anchoredPosition = _initPos;

        /*_tutorialText.transform.DOMove(_tutorialTextPos.position, 0.5f).OnComplete(()=>
        {
            _tutorialText.transform.DOShakePosition(150f);
        });*/

        _tutorialText.transform.DOLocalMoveY(400f, 0.5f).OnComplete(() =>
        {
            _tutorialText.transform.DOShakePosition(150f);
        });
    }

    private IEnumerator StartTutorial()
    {
        TextPop("네온 타워에 오신 것을 환영합니다.");
        yield return new WaitForSeconds(3f);
        TextPop($"캐릭터의 기본 조작은 <#{ColorUtility.ToHtmlStringRGBA(_impactColor)}>WASD</color>로 합니다.");
        yield return new WaitForSeconds(3f);
        TextPop("이동을 하여 노란 공에 닿아보세요.");
        yield return new WaitForSeconds(1f);
        TextPop("3");
        yield return new WaitForSeconds(0.5f);
        TextPop("2");
        yield return new WaitForSeconds(0.5f);
        TextPop("1");
        yield return new WaitForSeconds(0.5f);
        TextPop($"<#{ColorUtility.ToHtmlStringRGBA(_impactColor)}>시작!</color>");
        _tutorialStart = true;
    }

    private IEnumerator GoTutorialTwo()
    {
        yield return new WaitForSeconds(1f);
        TextPop("오른쪽으로 이동해주세요");
        for(int i = 0; i<_test1Boundarys.Length; i++)
        {
            _test1Boundarys[i].SetActive(false);
        }
    }

    private IEnumerator TutorialTwoStart()
    {
        TextPop("잘 하셨습니다!");
        yield return new WaitForSeconds(1f);
        TextPop($"<#{ColorUtility.ToHtmlStringRGBA(_impactColor)}>Shift</color> 키를 눌러서 <#{ColorUtility.ToHtmlStringRGBA(_impactColor)}>대시를</color> 사용할 수 있습니다.");
        yield return new WaitForSeconds(3f);
        TextPop($"대시는 여러 번 사용할 수 있는 <#{ColorUtility.ToHtmlStringRGBA(_impactColor)}>충전형 스킬</color>입니다.");
        yield return new WaitForSeconds(3f);
        TextPop("남은 충전 횟수는 오른쪽 아래의 UI에서 확인하실 수 있습니다.");
        yield return new WaitForSeconds(3f);
        TextPop("준비 되셨나요?");
        yield return new WaitForSeconds(1f);
        TextPop("3");
        yield return new WaitForSeconds(0.5f);
        TextPop("2");
        yield return new WaitForSeconds(0.5f);
        TextPop("1");
        yield return new WaitForSeconds(0.5f);
        TextPop("시작!!");
        _dashObj[0].SetActive(true);
    }

    [SerializeField]
    private GameObject[] _dashObj = null;
    private int i = 0;

    public void NextDashObjectEnable()
    {
        i++;

        if (i == _dashObj.Length)
        {
            TextPop($"<#{ColorUtility.ToHtmlStringRGBA(_impactColor)}>클리어 !!!</color>");
            StartCoroutine(Test2Clear());
            return;
        }

        _dashObj[i].SetActive(true);
    }
    public void DashObjectCollision(Collider col)
    {
        Instantiate(_colEffect, col.transform.position, Quaternion.identity);
        col.gameObject.SetActive(false);
    }

    private IEnumerator Test2Clear()
    {
        yield return new WaitForSeconds(1.5f);
        TextPop($"엄청 대단하시네요 !!");
        yield return new WaitForSeconds(3f);
        TextPop($"밑으로 내려가주세요");
        for (int i = 0; i < _test2Boundarys.Length; i++)
        {
            _test2Boundarys[i].SetActive(false);
        }
    }


    public void TestThreeStart(Collider col)
    {
        StartCoroutine(TutorialThreeStart());
        col.gameObject.SetActive(false);
    }
    private IEnumerator TutorialThreeStart()
    {
        TextPop("잘 하셨습니다!");
        yield return new WaitForSeconds(1f);
        TextPop($"<#{ColorUtility.ToHtmlStringRGBA(_impactColor)}>마우스 왼쪽</color> 키를 눌러서 <#{ColorUtility.ToHtmlStringRGBA(_impactColor)}>기본공격을</color> 할 수 있습니다.");
        yield return new WaitForSeconds(3f);
        TextPop("앞에 있는 표적을 때려보세요.");
        yield return new WaitForSeconds(3f);
        TextPop("준비 되셨나요?");
        yield return new WaitForSeconds(1f);
        TextPop("3");
        yield return new WaitForSeconds(0.5f);
        TextPop("2");
        yield return new WaitForSeconds(0.5f);
        TextPop("1");
        yield return new WaitForSeconds(0.5f);
        TextPop("시작!!");
        for(int i = 0; i<_enemyPrefabs.Length; i++)
        {
            _enemyPrefabs[i].SetActive(true);
            yield return new WaitForSeconds(1.5f);
        }
    }

    public void Test3Clear()
    {
        StartCoroutine(Test3ClearCoroutine());
    }

    private IEnumerator Test3ClearCoroutine()
    {
        TextPop($"잘 하셨습니다 !!!");
        yield return new WaitForSeconds(3f);
        TextPop($"마지막 훈련만 남았습니다.");
        yield return new WaitForSeconds(3f);
        TextPop($"옆으로 가주세요");

        for (int i = 0; i < _test3Boundarys.Length; i++)
        {
            _test3Boundarys[i].SetActive(false);
        }
    }


    public void Test4Start(Collider col)
    {
        StartCoroutine(Test4Coroutine());
        col.gameObject.SetActive(false);
    }

    private IEnumerator Test4Coroutine()
    {
        TextPop($"정말 훌륭합니다 !!!!");
        yield return new WaitForSeconds(3f);
        TextPop($"이 게임에는 레벨업으로 추가기능을 부여할 수 있습니다.");
        yield return new WaitForSeconds(3f);
        TextPop($"테스트용으로 경험치를 드릴테니 업그레이드를 해보세요.");
        yield return new WaitForSeconds(3f);
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
        ExpManager.instance.AddExp();
    }

    [SerializeField]
    private Image _fadeImage = null;

    public void Ending()
    {
        StartCoroutine(EndingCoroutine());

    }
    private IEnumerator EndingCoroutine()
    {
        TextPop($"잘하셨습니다. 이제 게임을 시작할 준비가 되신 것 같네요.");
        yield return new WaitForSeconds(3f);
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.DOFade(1f, 3f);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }

}
