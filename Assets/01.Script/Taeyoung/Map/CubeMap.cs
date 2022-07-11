using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Random = UnityEngine.Random;

public class CubeMap : MonoBehaviour
{
    List<Transform> prevPattern = new List<Transform>();

    [SerializeField] private Pattern[] pattern;
    int prevIdx = -1;
    public void Start()
    {
        StartCoroutine(MapCycle());
    }
    IEnumerator MapCycle()
    {
        while (true)
        {
            int randIdx = Random.Range(0, pattern.Length);
            while (randIdx == prevIdx)
            {
                randIdx = Random.Range(0, pattern.Length);
            }
            prevIdx = randIdx;

            foreach (Transform item in prevPattern)
            {
                Sequence seq = DOTween.Sequence();
                MeshRenderer renderer = item.GetComponent<MeshRenderer>();
                seq.Append(item.DOScaleY(0, 1f));
                seq.Join(DOTween.To(() => renderer.material.color, x => renderer.material.color = x, Color.white, 1));
                seq.AppendCallback(() => item.gameObject.SetActive(false));
            }
            prevPattern.Clear();
            foreach (Transform item in pattern[randIdx].pattern)
            {
                Sequence seq = DOTween.Sequence();
                MeshRenderer renderer = item.GetComponent<MeshRenderer>();
                seq.AppendCallback(() => item.gameObject.SetActive(true));
                seq.Append(item.DOScaleY(2 + Random.Range(0, 2.5f), 1f));
                seq.Join(DOTween.To(() => renderer.material.color, x => renderer.material.color = x, Color.yellow, 1));
                prevPattern.Add(item);
            }
            float cycleTime = Random.Range(2f, 3f);
            yield return new WaitForSeconds(cycleTime);
        }
    }
#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(15, 5, 15));
    }
#endif
    [Serializable]
    class Pattern
    {
        public Transform[] pattern;
    }
}
