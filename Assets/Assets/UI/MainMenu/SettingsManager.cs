using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsManager : MonoBehaviour
{
    public TextMeshProUGUI temp_name;
    public TextMeshProUGUI placeholder_name;
    public UnityEngine.UI.Slider music_slider;
    public UnityEngine.UI.Slider sound_slider;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnEnable()
    {
        Updateinfo();
    }

    public void Updateinfo()
    {
        Profile profile = ProfileManager.profile;
        placeholder_name.text = profile.name;
        music_slider.value = profile.music_level;
        sound_slider.value = profile.sound_level;
    }

    public void ApplyMusicLevel(UnityEngine.UI.Slider slider)
    {
        Profile profile = ProfileManager.profile;
        profile.music_level = slider.value;
        ResoursesDict.GetClass<SoundMain>().Accept();
    }

    public void ApplySoundLevel(UnityEngine.UI.Slider slider)
    {
        Profile profile = ProfileManager.profile;
        profile.sound_level = slider.value;
        ResoursesDict.GetClass<SoundMain>().Accept();
    }

    public void ApplyProfileName()
    {
        Profile profile = ProfileManager.profile;
        if (temp_name.text.Length > 12) { temp_name.text.Substring(0, 12); ResoursesDict.GetClass<SoundMain>().Restrict(); }
        else { ResoursesDict.GetClass<SoundMain>().Accept(); }
            profile.name = temp_name.text;
    }

    public void ApplyPicturePath(TextMeshProUGUI temp_path)
    {
        Profile profile = ProfileManager.profile;
        profile.profile_picture_path = temp_path.text;
        ProfileManager.LoadImage(profile.profile_picture_path);
        if (ProfileManager.profile_picture == ProfileManager.DeffaultImage) { ResoursesDict.GetClass<SoundMain>().Restrict(); }
        else { ResoursesDict.GetClass<SoundMain>().Accept(); }
    }
}
