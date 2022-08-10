using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviour
{
    public GameObject finishMenu;
    public int selectedText;
    public string[] texts;
    public TMP_Text dialogueText;
    public CanvasGroup canvasGroup;
    public Camera mainCamera;
    public Vector3 cameraPosA;
    public Vector3 cameraPosB;
    public float cameraSizeA;
    public float cameraSizeB;
    public float transitionProgress;
    public float canvAlpha;
    public float textAlpha;

    private bool starting = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void IncreaseSelectedText()
    {
        selectedText++;
    }

    public void UpdateText()
    {
        dialogueText.text = texts[selectedText];
    }

    public void StartTheGame()
    {
        if(!starting)
            FindObjectOfType<King>().StartMoving();
        starting = true;
    }

    public void UpdateCamera()
    {
        mainCamera.transform.position = transitionProgress * cameraPosB + (1f - transitionProgress) * cameraPosA;
        mainCamera.orthographicSize = transitionProgress * cameraSizeB + (1f - transitionProgress) * cameraSizeA;
    }

    public void DestroyThisObject()
    {
        Destroy(this.gameObject);
    }

    public void LaunchFinishMenu()
    {
        finishMenu.SetActive(true);
    }

    public void SetAnimatorValue(string name)
    {
        GetComponent<Animator>().SetBool(name, true);
    }


    // Update is called once per frame
    void Update()
    {
        UpdateCamera();
        canvasGroup.alpha = canvAlpha;
        dialogueText.color = new Color(0,0,0,textAlpha);
    }
}
