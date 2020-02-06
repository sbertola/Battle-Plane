using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;
    public Count terrainCount = new Count(5, 9);
    public GameObject exit;
    public GameObject[] terrainTiles;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitalizeList()
    {
        gridPositions.Clear();

        for(int x = 0; x < columns; x++)
        {
            for(int y = 0; y < rows; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject ("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 && y == -1)
                {
                    toInstantiate = wallTiles[0];
                }
                else if (x == -1 && y == rows)
                {
                    toInstantiate = wallTiles[6];
                }
                else if (x == columns && y == -1)
                {
                    toInstantiate = wallTiles[2];
                }
                else if (x == columns && y == rows)
                {
                    toInstantiate = wallTiles[4];
                }
                else if (x == columns && y == rows - 1)
                {
                    toInstantiate = exit;
                }
                else if (x == -1 && (y != -1 || y != rows))
                {
                    toInstantiate = wallTiles[7];
                }
                else if (x == columns && (y != -1 || y != rows || y != rows-1))
                {
                    toInstantiate = wallTiles[3];
                }
                else if (y == -1 && (x != -1 || x != columns))
                {
                    toInstantiate = wallTiles[1];
                }
                else if (y == rows && (x != -1 || x != columns))
                {
                    toInstantiate = wallTiles[5];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int RandomIndex = Random.Range(0,gridPositions.Count);
        Vector3 randomPosition = gridPositions[RandomIndex];
        gridPositions.RemoveAt(RandomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray,int miniumum, int maximum)
    {
        int objectCount = Random.Range(miniumum, maximum + 1);

        for(int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitalizeList();
        LayoutObjectAtRandom(terrainTiles, terrainCount.minimum, terrainCount.maximum);
        //Instantiate(exit, new Vector3(columns-1, rows - 1, 0f), Quaternion.identity);
    }
   //could potentially include multiple enemies if desired refer video 4
}
