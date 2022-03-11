using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProdGeneration : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int minStoneheight, maxStoneHeight;
    [SerializeField] Tilemap dirtTilemap, grassTilemap, stoneTilemap;
    [SerializeField] Tile dirt, grass, stone;
    [Range(0, 100)]
    [SerializeField] float heightValue, smoothness;
    [SerializeField] float seed;
    [SerializeField] int treeAmount;
    [SerializeField] GameObject enemy;
    void Start()
    {
        seed = Random.Range(-1000000, 1000000);
        Generation();
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            Instantiate(enemy, new Vector2(Random.Range(0, width), 20), Quaternion.identity);
            yield return new WaitForSeconds(5);
        }
    }

    private void Generation()
    {
        for (int x = 0; x < width; x++)//This will help spawn a tile on the x axis
        {
            int height = Mathf.RoundToInt(heightValue * Mathf.PerlinNoise(x / smoothness, seed));
            int minStoneSpawnDistance = height - minStoneheight;
            int maxStoneSpawnDistance = height - maxStoneHeight;
            int totalStoneSpawnDistance = Random.Range(minStoneSpawnDistance, maxStoneSpawnDistance);
            //Perlin noise.
            for (int y = 0; y < height; y++)//This will help spawn a tile on the y axis
            {
                if (y < totalStoneSpawnDistance)
                {
                    //spawnObj(stone, x, y);
                    stoneTilemap.SetTile(new Vector3Int(x, y, 0), stone);
                }
                else
                {
                    // spawnObj(dirt, x, y);
                    dirtTilemap.SetTile(new Vector3Int(x, y, 0), dirt);
                }

            }
            if (totalStoneSpawnDistance == height)
            {
                // spawnObj(stone, x, height);
                stoneTilemap.SetTile(new Vector3Int(x, height, 0), stone);
            }
            else
            {
                //spawnObj(grass, x, height);
                grassTilemap.SetTile(new Vector3Int(x, height, 0), grass);
            }

        }
    }

}
