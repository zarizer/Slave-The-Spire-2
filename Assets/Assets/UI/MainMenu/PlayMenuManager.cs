using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayMenuManager : MonoBehaviour
{
    Profile profile;
    public RawImage ProfileImage;
    public TextMeshProUGUI ProfileName;
    public TextMeshProUGUI BudgetText;
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    private void OnEnable()
    {
        profile = ProfileManager.profile;
        UpdateData();
    }

    public void UpdateData()
    {
        ProfileManager.LoadImage(ProfileManager.profile.profile_picture_path);
        ProfileImage.texture = ProfileManager.profile_picture;
        ProfileName.text = profile.name;
        BudgetText.text = profile.budget.ToString();
    }
}
