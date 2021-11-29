using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;


/**
 * 
 */

public class TilemapGenerator : MonoBehaviour
{
    [SerializeField] Tilemap tilemap = null;

    // CHANGE: Add an array of tiles instead of 2 tile types alone.
    // The user should add the tile manually on the editor.
    [Tooltip("Add your wall/border tile first, and then add the other tiles that will be used on the map.")]
    [SerializeField] TileBase[] Tiles;

    [Tooltip("Length and height of the grid")]
    [SerializeField] int gridSize = 100;

    [Tooltip("How many steps do we want to simulate?")]
    [SerializeField] int simulationSteps = 20;

    [Tooltip("For how long will we pause between each simulation step so we can look at the result?")]
    [SerializeField] float pauseTime = 1f;

    private RandomTileGenerator tileGenerator;

    void Start()
    {
        // CHANGE: Initialize
        tileGenerator = new RandomTileGenerator(Tiles.Length, gridSize);
        tileGenerator.RandomizeMap();

        //For testing that init is working
        GenerateAndDisplayTexture(tileGenerator.GetMap());

        //Start the simulation
        StartCoroutine(SimulateMapPattern());
    }


    //Do the simulation in a coroutine so we can pause and see what's going on
    private IEnumerator SimulateMapPattern()
    {
        for (int i = 0; i < simulationSteps; i++)
        {
            yield return new WaitForSeconds(pauseTime);

            //Calculate the new values
            tileGenerator.SmoothMap();

            //Generate texture and display it on the plane
            GenerateAndDisplayTexture(tileGenerator.GetMap());
        }
        Debug.Log("Simulation completed!");
    }

    //Generate a texture depending on the tiles index.
    //Display the texture on a plane:
    private void GenerateAndDisplayTexture(int[,] data)
    {
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                var position = new Vector3Int(x, y, 0);

                // CHANGE: Set the tile to display according to it's index on the data (generated) array.
                int tileToUse = data[x, y];
                var tile = Tiles[tileToUse];
                tilemap.SetTile(position, tile);
            }
        }
    }
}
