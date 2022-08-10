using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class WheatTileMapInteractor : MonoBehaviour
{
    public GameObject[] breakSounds;
    public GameObject breakageGameObject;
    public Tile[] wheatTiles;
    private Player player;
    private Tilemap tilemap;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        tilemap = GetComponent<Tilemap>();
    }
    void CreateBreakage(Vector3Int posInt)
    {
        Vector3 position = new Vector3(0.5f + posInt.x, 0.5f + posInt.y, -0.5f);
        GameObject go = Instantiate(breakageGameObject);
        go.transform.position = position;
        if (breakSounds.Length > 0)
        {
            GameObject go2 = Instantiate(breakSounds[Random.Range(0, breakSounds.Length)]);
            go.transform.position = position;
        }
    }
    public void Mine(Vector3Int posInt)
    {
        if (tilemap.GetTile(posInt) == wheatTiles[0] || tilemap.GetTile(posInt) == wheatTiles[1])
        {
            player.wheat++;
            tilemap.SetTile(posInt, null);
            CreateBreakage(posInt);
        }
       
    }
    public Vector3Int GetActiveTileCoord()
    {
        Vector3 pos = player.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (pos.x < 0)
            pos.x -= 1;
        if (pos.y < 0)
            pos.y -= 1;
        Vector3Int posInt = new Vector3Int((int)pos.x, (int)pos.y, 0);
        if (tilemap.GetTile(posInt))
            return posInt;
        else return new Vector3Int(0, 0, 10000);
    }

    public bool GetTileAtCoord(Vector3Int v)
    {
        return tilemap.GetTile(v);
    }
    void OnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = player.mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (pos.x < 0)
                pos.x -= 1;
            if (pos.y < 0)
                pos.y -= 1;
            Vector3Int posInt = new Vector3Int((int)pos.x, (int)pos.y, 0);
            Mine(posInt);

        }
    }

    // Update is called once per frame
    void Update()
    {
        //OnClick();
    }
}

