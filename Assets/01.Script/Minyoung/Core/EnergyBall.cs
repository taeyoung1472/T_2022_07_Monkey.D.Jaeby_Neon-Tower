using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    [SerializeField]
    private Transform targetTrm;
    [SerializeField] private Transform[] fireBalls;
    [SerializeField] private float sinSpeed = 1;
    [SerializeField] private float sinValue = 1;
    [SerializeField] private float offset = 3;
    private float time;
    public void OnEnable()
    {
        StartCoroutine(ItemRotate());
    }
    private void Update()
    {
        transform.position = targetTrm.position;
        Rotate();
    }
    void Rotate()
    {
        transform.Rotate(new Vector3(0, 180f, 0) * Time.deltaTime);

        time += Time.deltaTime * sinSpeed;

        foreach (Transform fireBall in fireBalls)
        {
            float sin = Mathf.Sin(time);
            fireBall.localPosition = new Vector3(0, 0, offset) + (Vector3.forward * sin) * sinValue;
        }
    }
    IEnumerator ItemRotate()
    {
        yield return new WaitForSeconds(30f);
        transform.gameObject.SetActive(false);
    }
}
