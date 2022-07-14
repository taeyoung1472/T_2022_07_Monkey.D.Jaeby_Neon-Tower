using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class SlotMachineManager : MonoBehaviour
{

    public SlotImage slotImagePrefab;
     
    public List<UpgradeTable> playerSO = new List<UpgradeTable>();
    public List<UpgradeTable> ETCSO = new List<UpgradeTable>();
    public List<UpgradeTable> bulletSO = new List<UpgradeTable>();

    public AbilitySO maxSO;

    private Transform _slotMachineTrm;

    public Transform _playerSlotTrm;
    public Transform _bulletSlotTrm;
    public Transform _etcSlotTrm;

    private void Awake()
    {
        _slotMachineTrm = GameObject.Find("SlotMachine").transform;

        //_playerSlotTrm = _slotMachineTrm.Find("Panel/PlayerButton/MaskImage/SlotObj");

        //_bulletSlotTrm = _slotMachineTrm.Find("Panel/BulletButton/SlotObj");

        //_etcSlotTrm = _slotMachineTrm.Find("Panel/ETCButton/SlotObj");
    }

    public void Make()
    {
        for (int i = 0; i < _playerSlotTrm.childCount; i++)
        {
            Destroy(_playerSlotTrm.GetChild(i).gameObject);
        }

        for (int i = 0; i < _bulletSlotTrm.childCount; i++)
        {
            Destroy(_bulletSlotTrm.GetChild(i).gameObject);
        }

        for (int i = 0; i < _etcSlotTrm.childCount; i++)
        {
            Destroy(_etcSlotTrm.GetChild(i).gameObject);
        }

        AbilitySO[] so = AbilitySOs(playerSO.ToArray(), UpgradeType.Player);
        for (int i = 0; i < so.Length; i++)
        {
            SlotImage slot = Instantiate(slotImagePrefab, _playerSlotTrm) as SlotImage;

            slot.SetData(0, so[i]);
            RectTransform rectTrm = slot.GetComponent<RectTransform>();
            rectTrm.anchoredPosition = new Vector2(0, 100) * i;

            rectTrm.sizeDelta = new Vector2(100, 100);
        }

        so = AbilitySOs(bulletSO.ToArray(), UpgradeType.Bullet);
        for (int i = 0; i < so.Length; i++)
        {
            SlotImage slot = Instantiate(slotImagePrefab, _bulletSlotTrm) as SlotImage;

            slot.SetData(1, so[i]);
            RectTransform rectTrm = slot.GetComponent<RectTransform>();
            rectTrm.anchoredPosition = new Vector2(0, 100) * i;

            rectTrm.sizeDelta = new Vector2(100, 100);
        }

        so = AbilitySOs(ETCSO.ToArray(), UpgradeType.ETC);
        for (int i = 0; i < so.Length; i++)
        {
            SlotImage slot = Instantiate(slotImagePrefab, _etcSlotTrm) as SlotImage;

            slot.SetData(2, so[i]);
            RectTransform rectTrm = slot.GetComponent<RectTransform>();
            rectTrm.anchoredPosition = new Vector2(0, 100) * i;

            rectTrm.sizeDelta = new Vector2(100, 100);
        }
    }

    private AbilitySO[] AbilitySOs(UpgradeTable[] table, UpgradeType type)
    {
        List<AbilitySO> soList = new List<AbilitySO>();
        for (int i = 0; i < 3; i++)
        {
            int rand = 0;
            switch (type)
            {
                case UpgradeType.Player:
                    rand = Random.Range(0, playerSO.Count);
                    break;
                case UpgradeType.ETC:
                    rand = Random.Range(0, ETCSO.Count);
                    break;
                case UpgradeType.Bullet:
                    rand = Random.Range(0, bulletSO.Count);
                    break;
                default:
                    break;
            }

            if (table[rand].count == 0)
            {
                int cnt = 0;

                switch (type)
                {
                    case UpgradeType.Player:
                        playerSO.RemoveAt(rand);
                        cnt = playerSO.Count;
                        break;
                    case UpgradeType.ETC:
                        ETCSO.RemoveAt(rand);
                        cnt = ETCSO.Count;
                        break;
                    case UpgradeType.Bullet:
                        bulletSO.RemoveAt(rand);
                        cnt = bulletSO.Count;
                        break;
                    default:
                        break;
                }

                if(cnt < 3)
                {
                    for (int j = i;  j < 3;  j ++)
                    {
                        soList.Add(maxSO);
                    }
                    return soList.ToArray();
                }
                i--;
                continue;
            }
            if (soList.Contains(table[rand].ability))
            {
                int cnt = 0;

                switch (type)
                {
                    case UpgradeType.Player:
                        cnt = playerSO.Count;
                        break;
                    case UpgradeType.ETC:
                        cnt = ETCSO.Count;
                        break;
                    case UpgradeType.Bullet:
                        cnt = bulletSO.Count;
                        break;
                    default:
                        break;
                }

                if(cnt <= 2)
                {
                    for (int j = 1; j < 3; j++)
                    {
                        soList.Add(maxSO);
                    }
                    return soList.ToArray();
                }

                i--;
                continue;
            }
            else
            {
                soList.Add(table[rand].ability);
            }
        }
        return soList.ToArray();
    }
    public void DestroyUpgradeTable(AbilitySO so)
    {
        ResultButton.Instance._btn.interactable = false;
        if (so == null)
            return;
        foreach (var item in playerSO)
        {
            if(item.ability == so)
            {
                item.count--;
                return;
            }
        }
        foreach (var item in bulletSO)
        {
            if (item.ability == so)
            {
                item.count--;
                return;
            }
        }
        foreach (var item in ETCSO)
        {
            if (item.ability == so)
            {
                item.count--;
                return;
            }
        }
    }
    enum UpgradeType
    {
        Player,
        ETC,
        Bullet
    }
    [Serializable]
    public class UpgradeTable
    {
        public AbilitySO ability;
        public int count;
    }
}