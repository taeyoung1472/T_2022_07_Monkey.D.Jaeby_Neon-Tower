using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLineEffect : MonoBehaviour
{
    public Material mat;
    public float thickness = 1f;

    [ColorUsage(true, true)]
    public Color colorOutline;

    private Renderer rend;
    void Start()
    {
        GameObject outlineObj = Instantiate(this.gameObject, transform.position, transform.rotation);
            outlineObj.transform.localScale = new Vector3(1, 1, 1);
        Renderer rend = outlineObj.GetComponent<Renderer>();
        rend.material = mat;
        rend.material.SetFloat("_Thickness", thickness);
        rend.material.SetColor("_OutlineColor", colorOutline);
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        rend.enabled = false;
        outlineObj.GetComponent<Collider>().enabled = false;
        outlineObj.GetComponent<OutLineEffect>().enabled = false;
        this.rend = rend;
    }
    private void OnMouseExit()
    {
        rend.enabled = false;
    }
    private void OnMouseEnter()
    {
        rend.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
