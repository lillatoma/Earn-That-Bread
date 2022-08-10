using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSplash : MonoBehaviour
{
    public Camera mainCamera;

    private Animator animator;
    private float time = 0f;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    private void OnMouse()
    {
        if (Input.GetMouseButtonDown(1))
        {
            transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0.375f,9.5f);
            time = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {


        OnMouse();
        animator.SetFloat("Time", time);
        time += Time.deltaTime;
    }
}
