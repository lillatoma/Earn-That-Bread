using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{

    private GlobalSettings globalSettings;
    public Slider soundSlider;
    public Slider musicSlider;
    public Toggle showFPS;
    public TMP_Dropdown fpsOption;
    // Start is called before the first frame update
    void Start()
    {
        globalSettings = FindObjectOfType<GlobalSettings>();
        soundSlider.value = globalSettings.soundVolume;
        musicSlider.value = globalSettings.musicVolume;
        showFPS.isOn = globalSettings.showFPS;
        fpsOption.value = globalSettings.fpsLock;
    }

    public void UpdateSound()
    {
        globalSettings.soundVolume = soundSlider.value;
    }
    public void UpdateMusic()
    {
        globalSettings.musicVolume = musicSlider.value;
    }

    public void UpdateFPSShow()
    {
        globalSettings.showFPS = showFPS.isOn;
    }

    public void UpdateFPSOption()
    {
        globalSettings.fpsLock = fpsOption.value;
        globalSettings.ChangeFramerate();
    }

    public void SaveSettings()
    {
        globalSettings.SaveSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
