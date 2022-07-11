using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float dashFixValue = 3;
    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bullet;
    [SerializeField] private LayerMask layerMask;
    CharacterController controller;
    Vector3 moveDir;
    Camera cam;
    bool isCanControll = true;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main;

        StartCoroutine(DashSystem());
    }
    void Update()
    {
        Move();
        Gravity();
        Rotate();
        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(bullet, firePos.position, Quaternion.Euler(0,transform.eulerAngles.y,0));
        }
    }

    IEnumerator DashSystem()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.LeftShift));
            isCanControll = false;
            moveDir = moveDir.normalized * dashFixValue;
            yield return new WaitForSeconds(dashTime);
            isCanControll = true;
        }
    }

    private void Rotate()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(cam.transform.position, ray.direction * 100, Color.red, 0.5f);
        if(Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            Vector3 hitPos = hit.point;

            float angle = Mathf.Atan2(hitPos.x - transform.position.x, hitPos.z - transform.position.z) * Mathf.Rad2Deg;

            transform.eulerAngles = new Vector3(0, angle, 0);
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
}
