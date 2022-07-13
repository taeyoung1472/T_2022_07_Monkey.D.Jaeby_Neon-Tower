using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class LevelUpEffect : MonoBehaviour
{
    public Transform endTrm;

    public Vector3 s = new Vector3(50f, 50f, 50f);
    private void Awake()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 1f));
        sequence.Join(transform.DOMove(endTrm.position, 1f));

        
        sequence.Append(transform.DOMove(endTrm.position + s, 0.5f));
        //sequence.Join(transform.DOScale(Vector3.one, 0.5f));

    }

    //[SerializeField] private TMP_Text levelUPTxt;
    //Mesh mesh;
    //Vector3[] vertices;


    //List<int> wordIndexes;
    //List<int> wordLengths;
    //[SerializeField] float timeBtwnChars;
    //[SerializeField] float timeBtwnWords;


    //public string[] stringArray;

    //private int i = 0;

    private void Start()
    {
        //levelUPTxt = GetComponent<TMP_Text>();
        //wordIndexes = new List<int> { 0 };
        //wordLengths = new List<int>();

        //string s = levelUPTxt.text;
        //for (int index = s.IndexOf(' '); index > -1; index = s.IndexOf(' ', index + 1))
        //{
        //    wordLengths.Add(index - wordIndexes[wordIndexes.Count - 1]);
        //    wordIndexes.Add(index + 1);
        //}
        //wordLengths.Add(s.Length - wordIndexes[wordIndexes.Count - 1]);
    }

    void Update()
    {
        //levelUPTxt.ForceMeshUpdate();
        //mesh = levelUPTxt.mesh;
        //vertices = mesh.vertices;

        //for (int w = 0; w < wordIndexes.Count; w++)
        //{
        //    int wordIndex = wordIndexes[w];
        //    Vector3 offset = Wobble(Time.time + w);
        //    for (int i = 0; i < wordLengths[w]; i++)
        //    {
        //        TMP_CharacterInfo c = levelUPTxt.textInfo.characterInfo[wordIndex + i];

        //        int index = c.vertexIndex;

        //        // Vector3 offset = Wobble(Time.time + i);
        //        vertices[index] += offset;
        //        vertices[index + 1] += offset;
        //        vertices[index + 2] += offset;
        //        vertices[index + 3] += offset;

        //        //Vector3 offset = Wobble(Time.time + i);
        //        //vertices[i] = vertices[i] + offset;
        //    }

        //}

        //mesh.vertices = vertices;
        //levelUPTxt.canvasRenderer.SetMesh(mesh);


        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    // EndCheck();
        //}
    }
    //Vector2 Wobble(float time)
    //{
    //   /// return new Vector2(Mathf.Sin(time * 8f), Mathf.Cos(time * 7f));
    //}
    //void EndCheck()
    //{
    //    if (i <= stringArray.Length - 1)
    //    {
    //        levelUPTxt.text = stringArray[i];
    //        StartCoroutine(TextVisible());
    //    }
    //}

    //IEnumerator TextVisible()
    //{
    //    levelUPTxt.ForceMeshUpdate();
    //    int totalVisbleCharacters = levelUPTxt.textInfo.characterCount;
    //    int counter = 0;

    //    while (true)
    //    {
    //        int visbleCount = counter % (totalVisbleCharacters + 1);
    //        levelUPTxt.maxVisibleCharacters = visbleCount;

    //        if (visbleCount >= totalVisbleCharacters)
    //        {
    //            i += 1;
    //            Invoke("EndCheck", timeBtwnWords);
    //        }
    //        counter += 1;
    //        yield return new WaitForSeconds(timeBtwnChars);
    //    }

    //    //for(int i=0;i<name.Length;i++)
    //    //{
    //    //    levelUPTxt.text = string.Format(name.Substring(0, i));
    //    //    yield return new WaitForSeconds(0.2f); 
    //    //}   
    //}
}
