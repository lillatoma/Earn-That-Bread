using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Saver
{
    static public void CreateIntKey(string a, int b)
    {
        if (!PlayerPrefs.HasKey(a))
            PlayerPrefs.SetInt(a, b);
    }

    static public void CreateFloatKey(string a, float b)
    {
        if (!PlayerPrefs.HasKey(a))
            PlayerPrefs.SetFloat(a, b);
    }
}

public class GlobalSettings : MonoBehaviour
{
    public float soundVolume = 0.25f;
    public float musicVolume = 0.25f;
    public bool showFPS = false;
    public int fpsLock = 1; // 0- 30, 1 -60, 2 - 120, 3 - unlimited
    public List<float> countedFrames;
    public float currentTime = 0f;

    public void CheckFrames(float maxDiff)
    {
        for (int i = countedFrames.Count - 1; i >= 0; i--)
            if (countedFrames[i] - currentTime < -maxDiff)
                countedFrames.RemoveAt(i);
    }

    public void AddFrame()
    {
        countedFrames.Add(currentTime);
    }

    void BeginKeys()
    {
        Saver.CreateIntKey("ShowFPS", 0);
        Saver.CreateIntKey("FPSLock", 1);
        Saver.CreateFloatKey("SoundVolume", 0.25f);
        Saver.CreateFloatKey("MusicVolume", 0.25f);
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        if (FindObjectsOfType<GlobalSettings>().Length > 1)
            Destroy(gameObject);
        else
        {
            BeginKeys();
            soundVolume = PlayerPrefs.GetFloat("SoundVolume", 0.25f);
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.25f);
            showFPS = false;
            if (PlayerPrefs.GetInt("ShowFPS", 0) != 0)
                showFPS = true;
            fpsLock = PlayerPrefs.GetInt("FPSLock", 1);
            if (fpsLock < 0)
                fpsLock = 0;
            if (fpsLock > 3)
                fpsLock = 3;
            int[] FPSValues = new int[] { 30, 60, 120, 5000 };
            Application.targetFrameRate = FPSValues[fpsLock];
        }
    }

    public void ResetSettings()
    {
        soundVolume = 0.25f;
        musicVolume = 0.25f;
        showFPS = false;
        fpsLock = 1;
    }

    public void ChangeFramerate()
    {
        int[] FPSValues = new int[] { 30, 60, 120, 5000 };
        Application.targetFrameRate = FPSValues[fpsLock];
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("SoundVolume", soundVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetInt("FPSLock", fpsLock);
        if(showFPS)
            PlayerPrefs.SetInt("ShowFPS", 1);
        else
            PlayerPrefs.SetInt("ShowFPS", 0);
        PlayerPrefs.Save();

    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.unscaledDeltaTime;
        //ChangeFramerate();
    }
}
