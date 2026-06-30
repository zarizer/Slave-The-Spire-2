using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMain : MonoBehaviour
{
    [SerializeField]
    private GameObject SoundObject;
    public float MusicVolume = 1f;
    public float SoundVolume = 1f;
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void PlaySound(string soundName)
    {
        var sound = Instantiate(SoundObject).GetComponent<SoundObject>();
        sound.Sound = ResoursesDict.SoundSet[soundName];
        sound.LifeTime = sound.Sound.length;
        sound.Source.volume = SoundVolume;
        sound.Play();
    }


}
