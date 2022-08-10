using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct StoneForm
{
    public Vector2Int[] stonePlaces;

    public Vector2Int[] RelativeStonePlaces(Vector2Int position)
    {
        Vector2Int[] _out = new Vector2Int[stonePlaces.Length];
        for (int i = 0; i < stonePlaces.Length; i++)
            _out[i] = position + stonePlaces[i];
        return _out;
    }
}
[System.Serializable]
public struct WheatForm
{
    public Vector2Int[] wheatPlaces;

    public Vector2Int[] RelativeWheatPlaces(Vector2Int position)
    {
        Vector2Int[] _out = new Vector2Int[wheatPlaces.Length];
        for (int i = 0; i < wheatPlaces.Length; i++)
            _out[i] = position + wheatPlaces[i];
        return _out;
    }
}


public class MapGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] enemyPrefabs;
    public float[] enemyChances;
    public Player player;
    public Tilemap tilemap;
    public Tilemap stonesTilemap;
    public Tilemap wheatTilemap;
    public Tile groundTile;
    public Tile underWheatTile;
    public Tile[] wheatTiles;
    public Tile[] rockTiles;
    public Tile[] coalTiles;
    public Tile[] ironTiles;

    public float wheatLandChance;
    public float wheatChance;
    public float stoneChance;
    public float coalChance;
    public float ironChance;

    public StoneForm[] stoneForms;
    public WheatForm[] wheatForms;

    private List<Vector2Int> generatedChunks = new List<Vector2Int>();
    
    void AddPremadeChunks()
    {
        generatedChunks.Add(new Vector2Int(0, 0));
        generatedChunks.Add(new Vector2Int(-1, 0));
    }

    void Start()
    {
        AddPremadeChunks();
    }


    void GenerateEnemy(Vector3Int posit)
    {
        Vector2 position = new Vector2(posit.x, posit.y);
        for(int i = 0; i < enemyChances.Length; i++)
        {
            if(Random.value < enemyChances[i])
            {
                GameObject enemy = Instantiate(enemyPrefabs[i]);
                enemy.transform.position = position;
                enemy.transform.position += new Vector3(0,0,-0.25f);
                return;
            }    
        }
    }
    void GenerateChunk(Vector2Int chunkIndex)
    {
        int bottom = chunkIndex.y * 16;
        int left = chunkIndex.x * 16;

        for(int i = 0; i < 16; i++)
            for(int j = 0; j < 16; j++)
            {
                Vector3Int tilePosition = new Vector3Int(left + i, bottom + j, 0);
                if (!tilemap.GetTile(tilePosition))
                {
                    float r = Random.value;
                    if (r < wheatLandChance)
                    {
                        Vector2Int[] wheatPlaces = wheatForms[Random.Range(0, wheatForms.Length)].RelativeWheatPlaces(new Vector2Int(tilePosition.x, tilePosition.y));

                        foreach (Vector2Int place in wheatPlaces)
                        {
                            Vector3Int tilePosition2 = new Vector3Int(place.x, place.y, 0);
                            if (!tilemap.GetTile(tilePosition2) || !stonesTilemap.GetTile(tilePosition2))
                            {
                                tilemap.SetTile(tilePosition2, underWheatTile);
                                if (Random.value < wheatChance)
                                    wheatTilemap.SetTile(tilePosition2, wheatTiles[0]);
                            }
                        }

                    }
                    else if (r < wheatLandChance + stoneChance)
                    {
                        Vector2Int[] stonePlaces = stoneForms[Random.Range(0, stoneForms.Length)].RelativeStonePlaces(new Vector2Int(tilePosition.x, tilePosition.y));

                        foreach (Vector2Int place in stonePlaces)
                        {
                            Vector3Int tilePosition2 = new Vector3Int(place.x, place.y, 0);
                            if (!tilemap.GetTile(tilePosition2) || tilemap.GetTile(tilePosition2) == groundTile)
                            {
                                float r2 = Random.value;
                                if (r2 < coalChance)
                                    stonesTilemap.SetTile(tilePosition2, coalTiles[0]);
                                else if (r2 < coalChance + ironChance)
                                    stonesTilemap.SetTile(tilePosition2, ironTiles[0]);
                                else
                                    stonesTilemap.SetTile(tilePosition2, rockTiles[0]);
                                tilemap.SetTile(tilePosition2, groundTile);
                                if (wheatTilemap.GetTile(tilePosition2))
                                    wheatTilemap.SetTile(tilePosition2, null);
                                
                            }
                        }

                    }
                    else tilemap.SetTile(tilePosition, groundTile);
                }
            }
        generatedChunks.Add(chunkIndex);

        for(int i = 0; i < 16;i++)
            for(int j = 0; j < 16; j++)
            {
                
                Vector3Int tilePosition = new Vector3Int(left + i, bottom + j, 0);
                if (!stonesTilemap.GetTile(tilePosition))
                    GenerateEnemy(tilePosition);
            }
    }

    void CheckGeneration()
    {
        Vector3 playerPosition = player.transform.position;
        Vector2Int playerIntPos = new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y) / 16;


        for (int i = -2; i <= 2; i++)
            for(int j = -2; j <= 2; j++)
            {
                Vector2Int checkTile = new Vector2Int(playerIntPos.x + i, playerIntPos.y + j);
                if (!generatedChunks.Contains(checkTile))
                    GenerateChunk(checkTile);
            }
    }

    // Update is called once per frame
    void Update()
    {
        CheckGeneration();
    }
}
