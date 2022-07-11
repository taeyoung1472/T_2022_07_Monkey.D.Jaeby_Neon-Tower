using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float dashFixValue = 3;
    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private float steminaChargeTime = 1.5f;
    [SerializeField] private TextMeshProUGUI warringTMP;
    [SerializeField] private Transform firePos;
    [SerializeField] private Transform mouseFocusObject;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Slider steminaSlider;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private int maxStemina = 3;
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

        StartCoroutine(DashSystem());
        StartCoroutine(SteminaSystem());
    }
    void Update()
    {
        Move();
        Gravity();
        Rotate();
        Dead();
    }

    private void Dead()
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
            print("DEAD!");
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
        print("너 죽어!");
        warringTMP.gameObject.SetActive(true);
        isInDeadZone = true;
    }
    private void OnTriggerExit(Collider other)
    {
        print("너 살어!");
        warringTMP.gameObject.SetActive(false);
        isInDeadZone = false;
    }
}
