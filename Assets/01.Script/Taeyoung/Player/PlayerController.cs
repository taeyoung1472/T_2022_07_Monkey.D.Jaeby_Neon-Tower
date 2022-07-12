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
    [SerializeField] private float speed = 5;
    [SerializeField] private float dashFixValue = 3;
    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private float steminaChargeTime = 1.5f;
    [SerializeField] private TextMeshProUGUI warringTMP;
    [SerializeField] private Transform mouseFocusObject;
    [SerializeField] private Animator animator;
    [SerializeField] private Slider steminaSlider;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private int maxStemina = 3;
    [SerializeField] private AudioClip dashClip;
    private MeshRenderer meshRenderer;

    [Header("생명관련")]
    [SerializeField] private int maxHp = 10;
    [SerializeField] private float ignoreTime = 0.25f;
    [SerializeField] private Color maxHpColor = Color.yellow;
    [SerializeField] private Color minHpColor = Color.red;
    [SerializeField] private AudioSource rollingSource;
    int curHp;
    bool isDamaged = false;
    float rollinVolumeGoal;

    #region Animator Hash
    readonly int moveHash = Animator.StringToHash("Move");
    #endregion

    int stemina;
    int Stemina { get { return stemina; } set { stemina = value; steminaSlider.value = (float)stemina / (float)maxStemina; } }
    CharacterController controller;
    Vector3 moveDir;
    Camera cam;
    bool isInDeadZone = false;
    bool isCanControll = true;
    float deadClock = 2f;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
        Stemina = maxStemina;
        curHp = maxHp;
        meshRenderer = GetComponent<MeshRenderer>();

        StartCoroutine(DashSystem());
        StartCoroutine(SteminaSystem());
        StartCoroutine(DamagedCor());
    }
    void Update()
    {
        Move();
        Gravity();
        Rotate();
        OutRangeCheck();
    }

    private void OutRangeCheck()
    {
        if (isInDeadZone)
        {
            deadClock -= Time.deltaTime;
            warringTMP.text = $"OUT OF AREA : {deadClock:0.0} SEC";
        }
        else
        {
            deadClock = 2;
        }
        if (deadClock <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        print("죽음!");
    }

    public void Damaged()
    {
        isDamaged = true;
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

            curHp--;

            if(curHp <= 0)
            {
                Dead();
                isDead = true;
            }

            isDamaged = false;
            meshRenderer.material.color = Color.Lerp(minHpColor, maxHpColor, (float)curHp / (float)maxHp);
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
        }
        else
        {
            animator.SetBool(moveHash, false);
            animator.transform.localRotation = Quaternion.identity;
            rollinVolumeGoal = 0;
        }

        rollingSource.volume = Mathf.Lerp(rollingSource.volume, rollinVolumeGoal, Time.deltaTime * 5);
        controller.Move(moveDir * speed * Time.deltaTime);
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
    private void OnTriggerEnter(Collider other)
    {
        warringTMP.gameObject.SetActive(true);
        isInDeadZone = true;
    }
    private void OnTriggerExit(Collider other)
    {
        warringTMP.gameObject.SetActive(false);
        isInDeadZone = false;
    }
}
