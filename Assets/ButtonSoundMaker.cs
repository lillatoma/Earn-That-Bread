using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundMaker : MonoBehaviour
{
    public GameObject buttonSound;

    public void MakeButtonSound()
    {
        Instantiate(buttonSound); 
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
