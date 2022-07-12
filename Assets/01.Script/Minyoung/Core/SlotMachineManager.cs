using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineManager : MonoBehaviour
{

    public SlotImage slotImagePrefab;
     
    public List<AbilitySO> playerSO = new List<AbilitySO>();
    public List<AbilitySO> ETCSO = new List<AbilitySO>();
    public List<AbilitySO> bulletSO = new List<AbilitySO>();

    private Transform _slotMachineTrm;

    private Transform _playerSlotTrm;
    private Transform _bulletSlotTrm;
    private Transform _etcSlotTrm;

    private void Awake()
    {
        _slotMachineTrm = GameObject.Find("SlotMachine").transform;

        _playerSlotTrm = _slotMachineTrm.Find("Panel/PlayerButton/SlotObj");

        _bulletSlotTrm = _slotMachineTrm.Find("Panel/BulletButton/SlotObj");

        _etcSlotTrm = _slotMachineTrm.Find("Panel/ETCButton/SlotObj");
    }

    
    private void Start()
    {
        for(int i = 0; i < playerSO.Count; i++)
        {
            SlotImage slot = Instantiate(slotImagePrefab, _playerSlotTrm) as SlotImage;

            slot.SetData(playerSO[i]);
            RectTransform rectTrm = slot.GetComponent<RectTransform>();
            rectTrm.anchoredPosition = new Vector2(0, 100) * i;

            rectTrm.sizeDelta = new Vector2(100, 100);
        }

        for (int i = 0; i < bulletSO.Count; i++)
        {
            SlotImage slot = Instantiate(slotImagePrefab, _bulletSlotTrm) as SlotImage;

            slot.SetData(bulletSO[i]);
            RectTransform rectTrm = slot.GetComponent<RectTransform>();
            rectTrm.anchoredPosition = new Vector2(0, 100) * i;

            rectTrm.sizeDelta = new Vector2(100, 100);
        }

        for (int i = 0; i < ETCSO.Count; i++)
        {
            SlotImage slot = Instantiate(slotImagePrefab, _etcSlotTrm) as SlotImage;

            slot.SetData(ETCSO[i]);
            RectTransform rectTrm = slot.GetComponent<RectTransform>();
            rectTrm.anchoredPosition = new Vector2(0, 100) * i;

            rectTrm.sizeDelta = new Vector2(100, 100);
        }


    }
}
