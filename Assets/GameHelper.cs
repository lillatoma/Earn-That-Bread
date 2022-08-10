using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameHelper : MonoBehaviour
{
    TMP_Text text;

    private float currentTextActiveTime;
    private int currentTextIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        text.text = "You have to build a furnace first.";
        UpdateTextAlpha();
        UpdateTextScaling();
    }

    void UpdateText()
    {
        string[] texts = new string[]{ "You have to build a furnace first.",
        "Mine to get the needed resources.",
        "Once you start cooking bread, it might attract hungry goblins.",
        "Be careful.",
        "If you are hurt, you can always eat a bread.",
        "Make sure it's freshly baked."};
        if (currentTextIndex >= texts.Length)
        {
            Destroy(gameObject);
            return; 
        }    

        string Text = texts[currentTextIndex];
        text.text = Text;
        currentTextActiveTime = 0f;
    }

    void IncreaseActiveTime()
    {
        currentTextActiveTime += Time.deltaTime;
    }

    void UpdateTextAlpha()
    {
        if (currentTextActiveTime < 0.5f)
            text.alpha = 0;
        else if (currentTextActiveTime < 1.5f)
            text.alpha = currentTextActiveTime - 0.5f;
        else if (currentTextActiveTime < 6.5f)
            text.alpha = 1f;
        else
            text.alpha = 7.5f - currentTextActiveTime;  
    }

    void UpdateTextScaling()
    {
        float scale = 0.5f;
        if (currentTextActiveTime < 0.5f)
            scale = 0.5f;
        else if (currentTextActiveTime < 1.5f)
            scale = 0.5f + (currentTextActiveTime - 0.5f) * 0.5f;
        else if (currentTextActiveTime < 6.5f)
            scale = 1f;
        else
            scale = (0.5f ) + (7.5f - currentTextActiveTime) * 0.5f;

        text.rectTransform.localScale = new Vector3(scale,scale,scale);
    }

    // Update is called once per frame
    void Update()
    {


        IncreaseActiveTime();
        UpdateTextAlpha();
        UpdateTextScaling();
        if(currentTextActiveTime > 7.5f)
        {
            currentTextIndex++;
            UpdateText();
        };    
    }
}
