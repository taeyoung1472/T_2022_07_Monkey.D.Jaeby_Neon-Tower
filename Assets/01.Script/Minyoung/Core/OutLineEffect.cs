using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLineEffect : MonoBehaviour
{
    public Material orignmat;
    public Material damagemat;
    public float thickness = 1f;

    [ColorUsage(true, true)]
    public Color colorOutline;

    private Renderer rend;
    void Start()
    {
       
    }

    public  void OnDamage()
    {
        StartCoroutine(DamageRendSet());
    }
    IEnumerator DamageRendSet()
    {
        yield break;
        //GameObject outlineObj = Instantiate(this.gameObject, transform.position, transform.rotation);
        ////outlineObj.transform.localScale = new Vector3(1, 1, 1);
        //Renderer rend = outlineObj.GetComponent<Renderer>();
        //rend.material = damagemat;
        //rend.material.SetFloat("_Thickness", thickness);
        //rend.material.SetColor("_OutlineColor", colorOutline);
        ////rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        ////rend.enabled = false;
        ////outlineObj.GetComponent<OutLineEffect>().enabled = false;
        //this.rend = rend;
        //rend.enabled = true;
        //yield return new WaitForSeconds(0.1f);
        //rend.enabled = false;
    }
}
