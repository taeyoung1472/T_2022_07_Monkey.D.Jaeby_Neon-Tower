using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip _slotClip = null;
    [SerializeField]
    private AudioClip _selectClip = null;
    [SerializeField]
    private AudioClip _buttonClickClip = null;


    public void SlotClipPlay()
    {
        PoolManager.instance.Pop(PoolType.Sound).GetComponent<AudioPoolObject>().Play(_slotClip);
    }
    public void SelectClipPlay()
    {
        PoolManager.instance.Pop(PoolType.Sound).GetComponent<AudioPoolObject>().Play(_selectClip);
    }
    public void ButtonClickClipPlay()
    {
        PoolManager.instance.Pop(PoolType.Sound).GetComponent<AudioPoolObject>().Play(_buttonClickClip);
    }
}
