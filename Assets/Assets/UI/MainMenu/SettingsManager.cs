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
    public UnityEngine.UI.Slider roll_speed_slider;
    public TextMeshProUGUI graphics_text;
    int current_graphics_tier;

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
        roll_speed_slider.value = 6.75f - profile.roll_speed;
        current_graphics_tier = ProfileManager.profile.graphics_settings;
        ChangeGraphicsName();
    }

    public void ApplyMusicLevel(UnityEngine.UI.Slider slider)
    {
        Profile profile = ProfileManager.profile;
        profile.music_level = slider.value;
        ResoursesDict.GetClass<SoundMain>().Accept();
    }

    public void ApplyRollSpeed(UnityEngine.UI.Slider slider)
    {
        float v = 0.75f + (6 - slider.value);
        ProfileManager.profile.roll_speed = v;
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

    public void OpenRedactor()
    {
        gameObject.SetActive(false);
        ResoursesDict.GetClass<LevelRedactor>().OpenRedactor();
    }

    public void OnGraphicsPress()
    {
        int tier = current_graphics_tier + 1;
        if (tier == 4) tier = 1;
        if (tier == 1) Graphics.activeTier = UnityEngine.Rendering.GraphicsTier.Tier1;
        if (tier == 2) Graphics.activeTier = UnityEngine.Rendering.GraphicsTier.Tier2;
        if (tier == 3) Graphics.activeTier = UnityEngine.Rendering.GraphicsTier.Tier3;
        ProfileManager.profile.graphics_settings = tier;
        current_graphics_tier = tier;
        ChangeGraphicsName();
        Shader.WarmupAllShaders();
        Debug.Log("Shaders");
    }

    void ChangeGraphicsName()
    {
        if (current_graphics_tier == 1) { graphics_text.text = "позорные"; }
        if (current_graphics_tier == 2) { graphics_text.text = "нормич"; }
        if (current_graphics_tier == 3) { graphics_text.text = "гуд"; }
    }
}
