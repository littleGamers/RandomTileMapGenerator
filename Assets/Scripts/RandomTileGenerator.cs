using System;
using UnityEngine;

/**
 * This script generates a random map of tiles in int[][].
 * Read the notes in the code.
 */
public class RandomTileGenerator
{
    //The height and length of the grid
    protected int gridSize;

    //The double buffer
    private int[,] bufferOld;
    private int[,] bufferNew;


    private System.Random random;

    // CHANGE: Number of tiles to select from
    private int tileSelectionCount;

    public RandomTileGenerator(int tileSelectionCount = 1, int gridSize = 100)
    {
        // CHANGE: Initialize tileSelectionCount
        this.tileSelectionCount = tileSelectionCount;
        this.gridSize = gridSize;

        this.bufferOld = new int[gridSize, gridSize];
        this.bufferNew = new int[gridSize, gridSize];

        random = new System.Random();
    }

    public int[,] GetMap()
    {
        return bufferOld;
    }

    /**
     * Generate a random map.
     * The map is not smoothed; call Smooth several times in order to smooth it.
     */
    public void RandomizeMap()
    {
        //Init the old values so we can calculate the new values
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (x == 0 || x == gridSize - 1 || y == 0 || y == gridSize - 1)
                {
                    //We dont want holes in our walls, so the border is always a wall
                    // CHANGE: Walls are now on index 0 for convenience
                    bufferOld[x, y] = 0;
                }
                else
                {
                    //Random walls and caves
                    // CHANGE: Create a random tile from selection indexes
                    bufferOld[x, y] = random.Next (1, tileSelectionCount);
                }
            }
        }
    }


    /**
     * Generate caves by smoothing the data
     * Remember to always put the new results in bufferNew and use bufferOld to do the calculations
     */
    public void SmoothMap()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                //Border is always wall
                if (x == 0 || x == gridSize - 1 || y == 0 || y == gridSize - 1)
                {
                    // CHANGE: Walls are now on index 0 for convenience
                    bufferNew[x, y] = 0;
                    continue;
                }

                //Uses bufferOld to get the wall count
                // CHANGE: Search for the tile that is most common in my neighbours and his number of appearances:
                Tuple<int, int> mostCommonNeighbourTile = GetMostCommonNeighbourTile(x, y);

                //Use some smoothing rules to generate caves
                // CHANGE: If I am surrounded by a common tile which takes more than 4 of my neighbours,
                //          Make my tile look like the common tile.
                // commonLimit is used to classify tiles with a lot of neighbours tile types or little. 
                int commonLimit = 2;
                if (mostCommonNeighbourTile.Item2 > commonLimit)
                {
                    bufferNew[x, y] = mostCommonNeighbourTile.Item1;
                }
                else if (mostCommonNeighbourTile.Item2 < commonLimit)
                {
                    spreadTile(x, y);
                }
                else
                {
                    bufferNew[x, y] = bufferOld[x, y];
                }
                
                
            }
        }

        //Swap the pointers to the buffers
        (bufferOld, bufferNew) = (bufferNew, bufferOld);
    }



    // Who is the most common tile around the tile on [cellX,cellY] ?
    private Tuple<int,int> GetMostCommonNeighbourTile(int cellX, int cellY)
    {
        // This function initializes an array of all the tiles from the entire tile collection.
        // Each index in the array represents a tile index.
        // The value in tilesAppearances[i] represents the number of appearances of tile type 'i' arround [cellX,cellY].
        // Also, we keep the max appearances index and value to return.

        int[] tilesAppearances = new int[tileSelectionCount];
        int maxIndex = 0, maxValue = 0;

        for (int neighborX = cellX - 1; neighborX <= cellX + 1; neighborX++)
        {
            for (int neighborY = cellY - 1; neighborY <= cellY + 1; neighborY++)
            {
                // Don't count border or current tile as neighbours
                if (bufferOld[neighborX, neighborY] == 0 || (neighborX == cellX && neighborY == cellY))
                { 
                    continue;
                }

                int currentNeighbour = bufferOld[neighborX, neighborY];
                
                // Increase the number of appearences of the current neighbour and update the max variables if needed:
                if (++tilesAppearances[currentNeighbour] > maxValue)
                {
                    maxIndex = currentNeighbour;
                    maxValue = tilesAppearances[currentNeighbour];
                }
                
            }
        }
        return Tuple.Create(maxIndex,  maxValue);
    }

    private void spreadTile(int cellX, int cellY)
    {
        /*
         * This function spreads the middle tile to it's neighbours.
         * Use carefully only on tile with many tile types surrounding it.
         * Not recommended to use on a tile with a lot of neighbours from a specific type of tile.
         */
        for (int neighborX = cellX - 1; neighborX <= cellX + 1; neighborX++)
        {
            for (int neighborY = cellY - 1; neighborY <= cellY + 1; neighborY++)
            {
                if (neighborX == 0 || neighborX == gridSize - 1 || neighborY == 0 || neighborY == gridSize - 1
                    || (neighborX == cellX && neighborY == cellY))
                    continue;
               
                bufferNew[neighborX, neighborY] = bufferNew[cellX, cellY];
                
            }
        }
    }
}
