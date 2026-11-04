using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMain : MonoBehaviour
{
    [SerializeField]
    private GameObject SoundObject;
    public static SoundMain instance;

    void Start()
    {
        
    }

    private void Awake()
    {
        instance = this;
    }


    void Update()
    {
        
    }

    public void PlaySound(string soundName)
    {
        var sound = Instantiate(SoundObject).GetComponent<SoundObject>();
        sound.Sound = ResoursesDict.SoundSet[soundName];
        sound.LifeTime = sound.Sound.length;
        sound.Source.volume = ProfileManager.profile.sound_level;
        sound.Play();
    }

    public void PlayMusic(string soundName)
    {
        var sound = Instantiate(SoundObject).GetComponent<SoundObject>();
        sound.Sound = ResoursesDict.SoundSet[soundName];
        sound.loop = true;
        sound.LifeTime = sound.Sound.length;
        sound.Source.volume = ProfileManager.profile.music_level;
        sound.Play();
    }



    public void Accept() { PlaySound("AcceptSound"); }
    public void Restrict() { PlaySound("RestrictSound"); }

}
