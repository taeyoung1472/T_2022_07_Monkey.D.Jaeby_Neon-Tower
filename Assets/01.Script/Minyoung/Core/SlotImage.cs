using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class SlotImage : MonoBehaviour
{
    AbilitySO abilitySO;

    private Image _image;

    public Button _btn;

    float timer;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(() =>
        {
            Set();
        });
    }
    private void OnEnable()
    {

    }
   public void Push()
    {
        StartCoroutine(a());
    }
    IEnumerator a()
    {
        yield return new WaitForSeconds(4f);
    }
    private void A()
    {
        
    }
private void Update()
    {
    }
    public void SetData(AbilitySO so)
    {
        _image.sprite = so.sprite;
        abilitySO = so;
    }
    public void Set()
    {
        SlotMachineMg.instance.resultImageBtn.image.color = new Color(1, 1, 1, 1);
        SlotMachineMg.instance.resultButton.SetData(abilitySO);
    }
}
