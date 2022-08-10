using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingSpawner : MonoBehaviour
{
    public GameObject[] enemies;
    public Vector2 spawnTimeWindow;
    public float spawnDistance;


    private Furnace furnace;
    private float timeTillNext;

    // Start is called before the first frame update
    void Start()
    {
        furnace = FindObjectOfType<Furnace>();
        timeTillNext = Random.Range(spawnTimeWindow.x, spawnTimeWindow.y);
    }

    void SpawnEnemy()
    {

        if(furnace.burning)
        {
            timeTillNext -= Time.deltaTime;
            if(timeTillNext <= 0f)
            {
                GameObject go = Instantiate(enemies[Random.Range(0, enemies.Length)]);
                go.transform.position = Random.insideUnitCircle.normalized * spawnDistance;
                go.transform.position += furnace.transform.position;
                timeTillNext = Random.Range(spawnTimeWindow.x, spawnTimeWindow.y);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpawnEnemy();
    }
}
