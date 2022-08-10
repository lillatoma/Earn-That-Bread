using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioWrapper : MonoBehaviour
{
    public bool music = false;
    public bool loopable = false;
    public bool dontDestroy = false;
    public bool allowDuplicates = true;
    public float length;
    public float selfVolume = 1f;
    public string[] destroyOnTheseScenes;
    [Range(0, 1)]
    public float volume = 0.5f;
    private AudioSource audioSource;
    private GlobalSettings globalSettings;

    public float volumeDownSpeed;
    public float volumeUpSpeed;
    public bool shouldVolumeDownOnScenes;
    public string[] allowedScenes;

    void VolumeDown()
    {
        selfVolume -= volumeDownSpeed * Time.deltaTime;
        if (selfVolume < 0f)
            selfVolume = 0f;
    }
    void VolumeUp()
    {
        selfVolume += volumeUpSpeed * Time.deltaTime;
        if (selfVolume > 1f)
            selfVolume = 1f;
    }

    void CheckCurrentScene()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        globalSettings = FindObjectOfType<GlobalSettings>();
        if (!allowDuplicates)
        {
            var wrappers = FindObjectsOfType<AudioWrapper>();
            foreach(var wrapper in wrappers)
            {
                if (wrapper == this || wrapper.audioSource == null)
                    continue;
                else if (wrapper.audioSource.clip == audioSource.clip)
                {
                    Destroy(gameObject);
                    return;
                }
            }
        }
        if(dontDestroy)
            DontDestroyOnLoad(this);
        if(!music)
            audioSource.volume = volume * globalSettings.soundVolume * selfVolume;
        else
            audioSource.volume = volume * globalSettings.musicVolume * selfVolume;
        if (loopable)
            audioSource.loop = true;
        audioSource.Play();
        if(!loopable)
            Destroy(gameObject, length);
    }

    void CheckScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        foreach (string str in destroyOnTheseScenes)
        {
            if (str == currentScene)
                Destroy(gameObject);
        }
        if (shouldVolumeDownOnScenes)
        {
            bool volumeDown = true;
            foreach (string str in allowedScenes)
            {
                if (str == currentScene)
                {
                    VolumeUp();
                    volumeDown = false;
                }
            }
            if (volumeDown)
                VolumeDown();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckScene();
        if (!music)
            audioSource.volume = volume * globalSettings.soundVolume * selfVolume;
        else
            audioSource.volume = volume * globalSettings.musicVolume * selfVolume;
    }
}
