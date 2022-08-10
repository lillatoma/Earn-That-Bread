using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadPlaneSpawner : MonoBehaviour
{
    public GameObject breadPlane;
    public float circleRadius;
    public int timesOnCircle;
    public int tallTimes;
    public int heightDistance;

    void GenerateBreadPlanesInCircle()
    {
        for(int i = 0; i < timesOnCircle; i++)
            for(int j = -tallTimes; j <= tallTimes; j++)
            {
                float x = Mathf.Sin(Mathf.Deg2Rad * (360f * i) / (float)timesOnCircle) * circleRadius;
                float y = Mathf.Cos(Mathf.Deg2Rad * (360f * i) / (float)timesOnCircle) * circleRadius;
                float z = j * heightDistance;
                GameObject go = Instantiate(breadPlane);
                go.transform.position = new Vector3(x, z, y);
                go.transform.rotation = Quaternion.Euler(180f, 180f-UseTools.RealVector2Angle(new Vector3(x,y)), -90f);
            }
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateBreadPlanesInCircle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
