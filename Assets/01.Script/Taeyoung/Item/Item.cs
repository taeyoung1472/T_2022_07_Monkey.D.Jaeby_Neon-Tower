using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float offset;
    public float size;
    public ItemType type;
    float sinValue;
    void Update()
    {
        sinValue += Time.deltaTime;
        transform.position = new Vector3(transform.position.x, (Mathf.Sin(sinValue) * size) + offset, transform.position.z);
        transform.Rotate(new Vector3(0, 90 * Time.deltaTime, 0));
    }
}
public enum ItemType
{
    HP,
    DMGBALL,
}
