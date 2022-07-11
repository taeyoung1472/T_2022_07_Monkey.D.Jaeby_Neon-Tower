using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask layerMask;
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

        Debug.DrawRay(cam.transform.position, ray.direction * 100, Color.red, 0.5f);
        if(Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            Vector3 hitPos = hit.point;

            float angle = Mathf.Atan2(hitPos.x - transform.position.x, hitPos.z - transform.position.z) * Mathf.Rad2Deg;

            print(angle);

            transform.eulerAngles = new Vector3(0, angle, 0);
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
