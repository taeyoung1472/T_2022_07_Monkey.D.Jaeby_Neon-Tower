using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    CharacterController controller;
    Vector3 moveDir;
    Camera cam;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
    }
    void Update()
    {
        Move();
        Rotate();
    }

    private void Rotate()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray, out hit))
        {
            Vector3 hitPos = hit.point;

            float angle = MathF.Atan2(transform.position.x - hitPos.x, transform.position.z - hitPos.z) * Mathf.Rad2Deg;

            transform.eulerAngles = new Vector3(0, angle, 0);
            //transform.rotation = Quaternion.LookRotation(hit.point);
        }
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        moveDir = new Vector3(h, 0, v);
        moveDir = transform.TransformDirection(moveDir);

        controller.Move(moveDir * speed * Time.deltaTime);
    }
}
