using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    public float LifeTime;
    public AudioClip Sound;
    public AudioSource Source;
    public bool loop = false;
    void Awake()
    {
        Source = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public void Play()
    {
        Source.clip = Sound;
        Source.loop = loop;
        Source.Play();

        LeanTween.delayedCall(LifeTime, () => { Destroy(this.gameObject); });
    }

}
