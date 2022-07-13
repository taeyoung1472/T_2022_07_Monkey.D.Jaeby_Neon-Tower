using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpBall : PoolAbleObject
{
    Transform player;
    float speed;

    public void Start()
    {
        player = Define.Instance.controller.transform;
    }
    public void Update()
    {
        speed += Time.deltaTime * 2.5f;
        transform.position = Vector3.Slerp(transform.position, player.position, Time.deltaTime * speed);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ExpManager.instance.AddExp();
            PoolManager.instance.Push(PoolType.ExpBall, gameObject);
        }
    }
    public override void Init_Pop()
    {
        speed = 0;
        player = Define.Instance.controller.transform;
    }

    public override void Init_Push()
    {

    }
}
