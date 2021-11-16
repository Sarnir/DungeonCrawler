using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    [SerializeField] private int length;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile floorTile;
    [SerializeField] private Tile wallTile;

    [SerializeField] private EnemyController enemyPrefab;
    [SerializeField] private MinionSpawn minionSpawnPrefab;
    [SerializeField] private Prop propPrefab;

    [SerializeField] private int wallYPos;

    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        tilemap.ClearAllTiles();
        BoxFill(tilemap, floorTile, new Vector3Int(0, -5, 0), new Vector3Int(length-1, 3, 0));
        BoxFill(tilemap, wallTile, new Vector3Int(0, wallYPos, 0), new Vector3Int(length-1, 4, 0));

        EdgeCollider2D wallCollider = GetComponent<EdgeCollider2D>();
        if (wallCollider == null)
        {
            wallCollider = gameObject.AddComponent<EdgeCollider2D>();
        }

        List<Vector2> points = new List<Vector2>();
        points.Add(new Vector2(0, wallYPos));
        points.Add(new Vector2(length, wallYPos));
        wallCollider.SetPoints(points);
        
        SpawnEntityType(enemyPrefab.gameObject, 0.01f);
        SpawnEntityType(propPrefab.gameObject, 0.02f);
        SpawnEntityType(minionSpawnPrefab.gameObject, 0.01f);
    }

    void SpawnEntityType(GameObject prefab, float chanceToSpawn)
    {
        for (int x = 12; x < length; x++)
        {
            for (int y = -4; y < wallYPos; y++)
            {
                if (Random.value <= chanceToSpawn)
                {
                    
                    var spawned = Instantiate(prefab, transform);
                    spawned.transform.position = new Vector3(x, y, 0);
                }
            }
        }
    }

    public void BoxFill(Tilemap map, TileBase tile, Vector3Int start, Vector3Int end) {
        //Determine directions on X and Y axis
        var xDir = start.x < end.x ? 1 : -1;
        var yDir = start.y < end.y ? 1 : -1;
        //How many tiles on each axis?
        int xCols = 1 + Mathf.Abs(start.x - end.x);
        int yCols = 1 + Mathf.Abs(start.y - end.y);
        //Start painting
        for (var x = 0; x < xCols; x++) {
            for (var y = 0; y < yCols; y++) {
                var tilePos = start + new Vector3Int(x * xDir, y * yDir, 0);
                map.SetTile(tilePos, tile);
            }
        }
    }
 
    //Small override, to allow for world position to be passed directly
    public void BoxFill(Tilemap map, TileBase tile, Vector3 start, Vector3 end) {
        BoxFill(map, tile, map.WorldToCell(start), map.WorldToCell(end));
    }
}
