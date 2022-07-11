using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbiltyManager : MonoBehaviour
{
    public static AbiltyManager Instance;
    public List<AbilitySO> playerSO = new List<AbilitySO>();
    public List<AbilitySO> ETCSO = new List<AbilitySO>();
    public List<AbilitySO> bulletSO = new List<AbilitySO>();


    [SerializeField] private AM playAm;
    [SerializeField] private AM bulletAm;
    [SerializeField] private AM ETCAm;

    private void Start()
    {
        Instance = this;
    }
    public void RandomSelect()
    {

        int randomPlayerIdx = Random.Range(0, playerSO.Count);
        int randomBulletIdx = Random.Range(0, bulletSO.Count);
        int randomETCIdx = Random.Range(0, ETCSO.Count);
        playAm.Set(playerSO[randomPlayerIdx]);
        bulletAm.Set(bulletSO[randomBulletIdx]);
        ETCAm.Set(ETCSO[randomETCIdx]);
        Debug.Log("¤¤¤¨¤¼");

    }

    public GameObject[] objects;


}
