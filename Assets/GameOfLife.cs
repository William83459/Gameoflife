using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    
    public GameObject cellPrefab;
    Cell[,] cells;
    float cellSize = 0.1f; //Size of our cells
    int numberOfColums, numberOfRows;
    int spawnChancePercentage = 20;

    private int[,] neighbourOffsets = new int[,]
    {
        {-1,-1}, {0,-1}, {1, -1},
        {-1,0}, {1,0},
        {-1, 1}, {0,1}, {1, 1},
    };

    void Start()
    {
        //Lower framerate makes it easier to test and see whats happening.
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 4;

        

        //Calculate our grid depending on size and cellSize
        numberOfColums = (int)Mathf.Floor(Camera.main.orthographicSize * Camera.main.aspect * 2  / cellSize);
        numberOfRows = (int)Mathf.Floor(Camera.main.orthographicSize * 2 / cellSize);

        

        

        //Initiate our matrix array
        cells = new Cell[numberOfColums, numberOfRows];

        //Create all objects

        //For each row
        for (int y = 0; y < numberOfRows; y++)
        {
            //for each column in each row
            for (int x = 0; x < numberOfColums; x++)
            {
                //Create our game cell objects, multiply by cellSize for correct world placement
                Vector2 newPos = new Vector2(x * cellSize - Camera.main.orthographicSize *
                    Camera.main.aspect,
                    y * cellSize - Camera.main.orthographicSize);

                var newCell = Instantiate(cellPrefab, newPos, Quaternion.identity);
                newCell.transform.localScale = Vector2.one * cellSize;
                cells[x, y] = newCell.GetComponent<Cell>();

                //Random check to see if it should be alive
                if (Random.Range(0, 100) < spawnChancePercentage)
                {
                    cells[x, y].alive = true;
                }

                cells[x, y].UpdateStatus();
            }
        }
    }


    void Update()
    {
        bool[,] nextGeneration = new bool[numberOfColums, numberOfRows]; 
        

        for (int y = 0; y < numberOfRows; ++y)
        {
            for (int x = 0;x < numberOfColums; x++)
            {
                int neighbours = Neighbour(x, y);

                if (cells[x,y].alive)
                {
                    if (neighbours == 2 ||  neighbours == 3)
                    {
                        nextGeneration[x, y] = true;
                    }
                    else 
                    {
                        nextGeneration[x, y] = false;
                    }
                }
                else
                {
                    if (neighbours == 3)
                    {
                        nextGeneration[x, y] = true;
                    }
                    else
                    {
                        nextGeneration[x, y] = false;
                    }
                }
            }
        }

        


        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColums; x++)
            {
                cells[x, y].alive = nextGeneration[x, y];
                cells[x, y].UpdateStatus();
            }
        }
    }
    private int Neighbour(int x,int y)
    {

        int neighbourCount = 0;

        for (int i = 0; i < neighbourOffsets.GetLength(0); i++)
        {
            int neighbourX = x + neighbourOffsets[i, 0];
            int neighbourY = y + neighbourOffsets[i, 1];

            if (neighbourX >= 0 && neighbourX < numberOfColums && neighbourY >= 0 && neighbourY < numberOfRows)
            {
                if (cells[neighbourX, neighbourY].alive)
                {
                    neighbourCount++;
                }
            }
        }


        return neighbourCount;
    }
}

