using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishMenu : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnClickNo()
    {
        FindObjectOfType<GameStartManager>().SetAnimatorValue("No");
        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }

    public void OnClickYes()
    {
        FindObjectOfType<GameStartManager>().SetAnimatorValue("Yes");
        
        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (canvasGroup.alpha < 1)
            canvasGroup.alpha += Time.deltaTime;
    }
}
