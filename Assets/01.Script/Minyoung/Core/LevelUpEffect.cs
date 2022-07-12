using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUpEffect : MonoBehaviour
{
    string name = "LEVEL UP! ";
    [SerializeField] private TextMeshProUGUI pp;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Chat());
        }
    }

    IEnumerator Chat()
    {
        for(int i=0;i<name.Length;i++)
        {
            pp.text = string.Format(name.Substring(0, i));
            yield return new WaitForSeconds(0.08f); 
        }
    }
}
