using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPoolObject : PoolAbleObject
{
    [SerializeField] private AudioSource source;
    private float stdPitch;
    public override void Init_Pop()
    {
        //DoNothing
    }

    public override void Init_Push()
    {
        source.Stop();
    }
    /// <summary>
    /// 오디오 재생
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    public void Play(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        source.Stop();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        stdPitch = pitch;
        source.Play();
        StartCoroutine(WaitForPush(source.clip.length * 1.05f));
    }
    public void Update()
    {
        source.pitch = stdPitch * (1 + (Time.timeScale - 1) * 0.5f);
    }
    IEnumerator WaitForPush(float time)
    {
        yield return new WaitForSeconds(time);
        PoolManager.instance.Push(PoolType, gameObject);
    }
}
