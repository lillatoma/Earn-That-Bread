using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
[RequireComponent(typeof(CanvasGroup))]
public class FPSCounter : MonoBehaviour
{
    public float timeDiffMax = 1f;
    GlobalSettings settings;
    TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        settings = FindObjectOfType<GlobalSettings>();
        text = GetComponent<TMP_Text>();
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    // Update is called once per frame
    void Update()
    {
        settings.AddFrame();
        settings.CheckFrames(timeDiffMax);
        int fps = (int)((1f * settings.countedFrames.Count) / timeDiffMax);
        if (settings.showFPS)
            text.text = fps.ToString() + " FPS";
        else
            text.text = "";

    }
}