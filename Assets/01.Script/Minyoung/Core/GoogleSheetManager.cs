using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
public class GoogleSheetManager : MonoBehaviour
{
   
   // const string URL = "https://script.google.com/macros/s/AKfycbxgMGLMxYtjjgFOAuez6vwj_YSxrWMKin9SLEGEPIK_SIR9-yHgCCEnUleV_nh-1Iem/exec";
    const string URL = "https://script.google.com/macros/s/AKfycbx9_e8uxkS35bPdgdYI9wJ0LDnwz7ILMiXLRO_jS2kmgUaWWqO2fvgb7o9b68c7uiSm/exec";
    // const string URL = "https://docs.google.com/spreadsheets/d/1PEaR8O3cXRmhguQJtpWOW5RG06ClDj8Fysi0drSlYeU/export?format=tsv"; //�ǰ� ���ͷ� �������� tsv  �ƾ� csv�� ���� �Ľ��Ҷ� ���峯 Ȯ���� ����
    public TMP_InputField IDInput, PassInput;
    string id, pass;

    //IEnumerator Start()
    //{
    //    WWWForm form = new WWWForm();
    //    form.AddField("value", "��");


    //    UnityWebRequest www = UnityWebRequest.Post(URL, form);
    //    yield return www.SendWebRequest();

    //    string data = www.downloadHandler.text;
    //    print(data);
    //}
    bool SetIDPass()
    {
        id = IDInput.text.Trim();
        pass = PassInput.text.Trim();

        if (id == "" || pass == "")
        {
            return false;
        }
        else
            return true;
    }
    public void Register()
    {
        if(!SetIDPass())
        {
            print("���̵� �Ǵ� ��й�ȣ�� ����ֽ��ϴ�");
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("order", "register");
        form.AddField("id", id);
        form.AddField("pass", pass);

        StartCoroutine(Post(form));

    }

    IEnumerator Post(WWWForm form)
    {
        using(UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            if (www.isDone)
                print(www.downloadHandler.text);
            else
                print("���� ������ �����ϴ�");
        }
    }
}