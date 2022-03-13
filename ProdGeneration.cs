using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class ProdGeneration : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] int width;
    [SerializeField] int minStoneheight, maxStoneHeight;
    [SerializeField] Tilemap mainTilemap, bgTilemap;
    [SerializeField] Tile dirt, grass, stone, tree, leaf;
    [Range(0, 100)]
    [SerializeField] float heightValue, smoothness;
    [SerializeField] float seed;
    [SerializeField] int treeAmount;
    [SerializeField] int treeInterval;
    [SerializeField] GameObject enemy;
    private Vector3Int prev;
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
        bool spawned = false;
        for (int x = 0; x < width; x++)//This will help spawn a tile on the x axis
        {
            var random = Random.Range(0, 10);
            int height = Mathf.RoundToInt(heightValue * Mathf.PerlinNoise(x / smoothness, seed));
            int minStoneSpawnDistance = height - minStoneheight;
            int maxStoneSpawnDistance = height - maxStoneHeight;
            int totalStoneSpawnDistance = Random.Range(minStoneSpawnDistance, maxStoneSpawnDistance);
            //Perlin noise.
            for (int y = 0; y < height; y++)//This will spawn a tile on the y axis
            {
                if (y < totalStoneSpawnDistance)
                {
                    mainTilemap.SetTile(new Vector3Int(x, y, 0), stone);

                    if (!spawned)
                    {
                        player.position = new Vector3Int(width / 4, height + 1, 0);
                        spawned = true;
                    }

                }
                else
                {
                    mainTilemap.SetTile(new Vector3Int(x, y, 0), dirt);
                }

            }
            
            if (totalStoneSpawnDistance == height)
            {

                mainTilemap.SetTile(new Vector3Int(x, height, 0), stone);
            }
            else
            {
                mainTilemap.SetTile(new Vector3Int(x, height, 0), grass);
                if (random < treeAmount && Vector3Int.Distance(new Vector3Int(x, height + 1), prev) > treeInterval)
                {
                    bgTilemap.SetTile(new Vector3Int(x, height + 1, 0), tree);
                    bgTilemap.SetTile(new Vector3Int(x, height + 2, 0), tree);
                    bgTilemap.SetTile(new Vector3Int(x, height + 3, 0), leaf);
                    bgTilemap.SetTile(new Vector3Int(x, height + 4, 0), leaf);
                    bgTilemap.SetTile(new Vector3Int(x, height + 5, 0), leaf);
                    bgTilemap.SetTile(new Vector3Int(x + 1, height + 3, 0), leaf);
                    bgTilemap.SetTile(new Vector3Int(x - 1, height + 3, 0), leaf);
                    bgTilemap.SetTile(new Vector3Int(x + 1, height + 4, 0), leaf);
                    bgTilemap.SetTile(new Vector3Int(x - 1, height + 4, 0), leaf);
                    bgTilemap.SetTile(new Vector3Int(x + 1, height + 5, 0), leaf);
                    bgTilemap.SetTile(new Vector3Int(x - 1, height + 5, 0), leaf);
                    prev = new Vector3Int(x, height + 1, 0);
                }
            }
        }
    }
}
