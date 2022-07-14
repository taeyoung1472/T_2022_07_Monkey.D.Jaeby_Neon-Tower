using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float dashFixValue = 3;
    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private float steminaChargeTime = 1.5f;
    [SerializeField] private TextMeshProUGUI warringTMP;
    [SerializeField] private Transform mouseFocusObject;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private int maxStemina = 3;
    [SerializeField] private AudioClip dashClip;
    [SerializeField] private MeshRenderer meshRenderer;

    [Header("생명관련")]
    [SerializeField] private float ignoreTime = 0.25f;
    [SerializeField] private Color maxHpColor = Color.yellow;
    [SerializeField] private Color minHpColor = Color.red;
    [SerializeField] private AudioSource rollingSource;
    [SerializeField] private int autoHealDelay;
    [SerializeField] private float[] hpStealValue;
    [SerializeField] private HPUI hpUi;
    [SerializeField] private HPUI dashUi;
    [SerializeField] private DieEffect dieEffect;
    [SerializeField] private ParticleSystem dustParticle;
    [SerializeField] private GameObject volumeCollision;
    PlayerStat stat;
    float stealHp;
    float glitchTime;
    float grayTime;
    int curHp;
    bool isDamaged = false;
    float rollinVolumeGoal;
    bool isGod = true;
    float godTime = 0;

    #region Animator Hash
    readonly int moveHash = Animator.StringToHash("Move");
    #endregion

    int stemina;
    bool isDead;
    int Stemina { get { return stemina; } set { stemina = value; dashUi.DisplayValue(stemina, maxStemina); } }
    CharacterController controller;
    Vector3 moveDir;
    Camera cam;
    bool isInDeadZone = false;
    bool isCanControll = true;
    float deadClock = 2f;
    void Start()
    {
        stat = GameManager.Instance.playerStat;
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
        Stemina = maxStemina;
        curHp = stat.hp;

        hpUi.DisplayValue(curHp, stat.hp);

        Time.timeScale = 1;

        StartCoroutine(DashSystem());
        StartCoroutine(SteminaSystem());
        StartCoroutine(DamagedCor());
        StartCoroutine(AutoHealSystem());
    }
    void Update()
    {
        if (!isDead)
        {
            Move();
            Gravity();
            Rotate();
            OutRangeCheck();
        }
        Glitch();
        Gray();
        God();
    }

    private void Gray()
    {
        if(grayTime > 0)
        {
            grayTime -= Time.deltaTime;
            volumeCollision.gameObject.SetActive(true);
        }
        else
        {
            volumeCollision.gameObject.SetActive(false);
        }
    }

    private void God()
    {
        if(godTime > 0)
        {
            godTime -= Time.deltaTime;
            isGod = true;
        }
        else
        {
            isGod = false;
        }
    }

    private void Glitch()
    {
        if(glitchTime > 0)
        {
            glitchTime -= Time.deltaTime;
            Samples.SampleController.instance.StartSceneValue();
        }
        else if(!isInDeadZone)
        {
            Samples.SampleController.instance.ZeroValue();
        }
    }

    private void OutRangeCheck()
    {
        if (isInDeadZone)
        {
            deadClock -= Time.deltaTime;
            Samples.SampleController.instance.StartSceneValue();
            warringTMP.text = $"OUT OF AREA : {deadClock:0.0} SEC";
        }
        else
        {
            Samples.SampleController.instance.ZeroValue();
            deadClock = 2;
        }
        if (deadClock <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        dieEffect.PlayerDieEffect();
        isDead = true;
        ExpManager.instance.isCanLevelup = false;
        print("죽음!");
    }

    public void Damaged()
    {
        isDamaged = true;
    }

    IEnumerator AutoHealSystem()
    {
        yield return new WaitUntil(() => stat.autoHealDelay < 16);
        while (true)
        {
            yield return new WaitUntil(() => curHp != stat.hp);
            yield return new WaitForSeconds(stat.autoHealDelay);
            if(curHp != stat.hp)
            {
                curHp++;
            }
        }
    }

    public void StealHp()
    {
        if(stealHp < 1)
        {
            stealHp += hpStealValue[stat.stealHp];
        }
        if(stealHp >= 1)
        {
            if (curHp != stat.hp)
            {
                curHp++;
                stealHp -= 1;
                hpUi.DisplayValue(curHp, stat.hp);
            }
        }
    }

    IEnumerator DashSystem()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.LeftShift) && Stemina > 0);
            Stemina--;
            isCanControll = false;
            moveDir = moveDir.normalized * dashFixValue;
            PoolManager.instance.Pop(PoolType.Sound).GetComponent<AudioPoolObject>().Play(dashClip, 1, Random.Range(0.9f, 1.1f));
            yield return new WaitForSeconds(dashTime);
            isCanControll = true;
        }
    }

    IEnumerator SteminaSystem()
    {
        while (true)
        {
            yield return new WaitUntil(() => Stemina < maxStemina);
            yield return new WaitForSeconds(steminaChargeTime);
            Stemina++;
        }
    }

    IEnumerator DamagedCor()
    {
        bool isDead = false;
        while (!isDead)
        {
            yield return new WaitUntil(() => isDamaged);
            if (!isGod)
            {
                curHp--;
                glitchTime = 0.6f;
                grayTime = 0.2f;
                CameraManager.instance.CameraShake(2, 2, 0.2f);

                if (curHp <= 0)
                {
                    Dead();
                    isDead = true;
                }

                hpUi.DisplayValue(curHp, stat.hp);

                isDamaged = false;
                meshRenderer.material.color = Color.Lerp(minHpColor, maxHpColor, (float)curHp / (float)stat.hp);
                GodMode(0.3f);
            }
            yield return new WaitForSeconds(ignoreTime);
        }
        yield return null;
    }

    private void Rotate()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            Vector3 hitPos = hit.point;

            float angle = Mathf.Atan2(hitPos.x - transform.position.x, hitPos.z - transform.position.z) * Mathf.Rad2Deg;

            transform.eulerAngles = new Vector3(0, angle, 0);

            mouseFocusObject.position = hitPos;
        }
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (isCanControll)
        {
            moveDir = new Vector3(h, moveDir.y, v);
        }

        if(h != 0 || v != 0)
        {
            animator.SetBool(moveHash, true);
            animator.transform.rotation = Quaternion.LookRotation(new Vector3(h, 0, v));
            rollinVolumeGoal = 1;
            var e = dustParticle.emission;
            e.enabled = true;
        }
        else
        {
            animator.SetBool(moveHash, false);
            animator.transform.localRotation = Quaternion.identity;
            rollinVolumeGoal = 0;
            var e = dustParticle.emission;
            e.enabled = false;
        }

        rollingSource.volume = Mathf.Lerp(rollingSource.volume, rollinVolumeGoal, Time.deltaTime * 5);
        controller.Move(moveDir * stat.speed * Time.deltaTime);
    }

    private void Gravity()
    {
        if (!controller.isGrounded)
        {
            moveDir.y += Time.deltaTime * Physics.gravity.y;
        }
        else
        {
            moveDir.y = 0;
        }
    }
    public void GodMode(float dur)
    {
        if(godTime < dur)
        {
            godTime = dur;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeadZone"))
        {
            warringTMP.gameObject.SetActive(true);
            isInDeadZone = true;
        }
        if (other.CompareTag("Item"))
        {
            Item item = other.gameObject.GetComponent<Item>();
            switch (item.type)
            {
                case ItemType.HP:
                    curHp += 2;
                    if(curHp >= stat.hp)
                    {
                        curHp = stat.hp;
                    }
                    break;
                case ItemType.DMGBALL:
                    print("DMGBALL Item Use");
                    break;
                default:
                    break;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DeadZone"))
        {
            warringTMP.gameObject.SetActive(false);
            isInDeadZone = false;
        }
    }
}
