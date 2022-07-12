using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
   
    public void OnEnable()
    {
        StartCoroutine(ItemRotate());
    }
    private void Update()
    {
        Rotate();
    }
    void Rotate()
    {
        transform.Rotate(new Vector3(0, 3f, 0));
    }
    IEnumerator ItemRotate()
    {
        yield return new WaitForSeconds(30f);
        transform.gameObject.SetActive(false);
    }
}
