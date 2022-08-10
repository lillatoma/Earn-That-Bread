using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        eulerAngles += new Vector3(0, rotationSpeed * Time.deltaTime, 0);
        transform.rotation = Quaternion.Euler(eulerAngles);
    }
}
