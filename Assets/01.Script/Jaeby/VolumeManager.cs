using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _audioMixer = null;
    [SerializeField]
    private Slider _masterSlider = null;
    [SerializeField]
    private Slider _bgSlider = null;
    [SerializeField]
    private Slider _sfxSlider = null;

    public void MasterVolumeSet()
    {
        if (_masterSlider.value > 0f)
        {
            _masterSlider.value = 0f;
        }
    }
    public void BGVolumeSet()
    {
        if (_bgSlider.value > 0f)
        {
            _bgSlider.value = 0f;
        }
    }
    public void SFXVolumeSet()
    {
        if (_sfxSlider.value > 0f)
        {
            _sfxSlider.value = 0f;
        }
    }

    private void Update()
    {
        MasterVolumeUpdate();
    }

    private void MasterVolumeUpdate()
    {
        float sound = _masterSlider.value;

        _audioMixer.SetFloat("Master",sound);
    }
}
