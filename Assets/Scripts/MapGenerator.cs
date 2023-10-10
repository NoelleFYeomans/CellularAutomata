using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour
{
    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)]
    public int randomFillPercent;

    int[,] map;

    private void Start()
    {
        GenerateMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateMap();
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "GenNewMap"))
        {
            GenerateMap();
        }
    }

    void GenerateMap() //this is what generates an actual map to display
    {
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < 5; i++)
        {
            SmoothMap();
        }

        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(map, 1);
    }

    void RandomFillMap() //this is the code that randomly choses whether each coordinate is 1(filled) or 0(empty)
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random psuedoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || y == 0 || x == (height - 1) || y == (width - 1))
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (psuedoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0; //assigns a values of 1 or 0 depending on if the value rolled is above or below filler percent
                }
            }
        }
    }

    void SmoothMap() //this removes cubes if it has < 4 neighbours and adds if > 4
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4)
                {
                    map[x, y] = 1;
                }
                else if (neighbourWallTiles < 4)
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY) //this counts the number of squares surrounding any given square
    {
        int wallCount = 0;
        for (int neighbourX = (gridX - 1); neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = (gridY - 1); neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    private void OnDrawGizmos() //this is essentially the drawMap equivalent of the textRPG, this is what *draws* the squares
    {
        //if (map != null)
        //{
        //    for (int x = 0; x < width; x++)
        //    {
        //        for (int y = 0; y < height; y++)
        //        {
        //            Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white; //assigns colour based on if 1 or 0
        //            Vector3 pos = new Vector3(-width / 2 + x + .5f, 0, 0 - height / 2 + y + .5f); //idk
        //            Gizmos.DrawCube(pos, Vector3.one);
        //        }
        //    }
        //}
    }
}
