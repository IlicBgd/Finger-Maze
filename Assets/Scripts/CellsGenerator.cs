using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class CellsGenerator : MonoBehaviour
{
    public int sizeHorizontal;
    public int sizeVertical;

    [SerializeField]
    Cell cellPrefab;
    [SerializeField]
    Edge horizontalEdge;
    [SerializeField]
    Edge verticalEdge;
    [SerializeField]
    Vector2 cellAreaSize;

    Vector2 cellScale;
    Vector2 startPosition;

    float scaleX;
    float scaleY;

    Cell[,] cells;

    List<Edge> horEdges = new List<Edge>();
    List<Edge> verEdges = new List<Edge>();

    int cellCounter = 0;
    private void Awake()
    {
        cells = new Cell[sizeHorizontal, sizeVertical];
        for (int x = 0; x < sizeHorizontal; x++)
        {
            for (int y = 0; y < sizeVertical; y++)
            {
                cells[x, y] = new Cell();
                cells[x, y].cellID = cellCounter;
                cellCounter++;
            }
        }

        scaleX = cellAreaSize.x / sizeHorizontal;
        scaleY = cellAreaSize.y / sizeVertical;

        cellScale = scaleX < scaleY ? new Vector2(scaleX, scaleX) : new Vector2(scaleY, scaleY);

        GenerateCells();
        AddEdgesToCells();
    }
    private void Start()
    {
        MazeAlgorithm();
    }
    //Cell => only info
    //Use parent => scale, offset, position
    void GenerateCells()
    {
        startPosition.x = transform.position.x - (cellScale.x / 2) * (sizeHorizontal - 1);
        startPosition.y = transform.position.y + (cellScale.y / 2) * (sizeVertical - 1);

        //int counter = 0;
        float offsetHoriz = horizontalEdge.spriteRenderer.bounds.size.x * cellScale.x / 2;
        float offsetVert = verticalEdge.spriteRenderer.bounds.size.y * cellScale.y / 2;

        transform.localScale = cellScale;

        for (int i = 0; i < sizeHorizontal; i++)
        {
            for (int j = 0; j < sizeVertical; j++)
            {
                Edge horEdge;
                Edge verEdge;
                Vector2 position = startPosition + new Vector2(cellScale.x * i, -cellScale.y * j + offsetVert);
                horEdge = Instantiate(horizontalEdge, position, Quaternion.identity, transform);
                horEdges.Add(horEdge);
                position = startPosition + new Vector2(cellScale.x * i - offsetHoriz, -cellScale.y * j);
                verEdge = Instantiate(verticalEdge, position, Quaternion.identity, transform);
                verEdges.Add(verEdge);
                if (i == sizeHorizontal - 1)
                {
                    position = startPosition + new Vector2(cellScale.x * i + offsetHoriz, -cellScale.y * j);
                    verEdge = Instantiate(verticalEdge, position, Quaternion.identity, transform);
                    verEdges.Add(verEdge);
                }
                if (j == sizeVertical - 1)
                {
                    position = startPosition + new Vector2(cellScale.x * i, -cellScale.y * j - offsetVert);
                    horEdge = Instantiate(horizontalEdge, position, Quaternion.identity, transform);
                    horEdges.Add(horEdge);
                }
            }
        }
    }
    void AddEdgesToCells()
    {
        int counter = 0;

        for (int x = 0; x < sizeHorizontal; x++)
        {
            for (int y = 0; y < sizeVertical; y++)
            {
                NorthEdge(cells[x, y], x + y + counter);
                SouthEdge(cells[x, y], x + y + counter + 1);
                WestEdge(cells[x, y], x + y);
                EastEdge(cells[x, y], x + y + sizeVertical);
                counter++;
            }
        }
    }
    void NorthEdge(Cell cell, int number)
    {
        cell.north = horEdges[number];
    }
    void SouthEdge(Cell cell, int number)
    {
        cell.south = horEdges[number];
    }
    void WestEdge(Cell cell, int number)
    {
        cell.west = verEdges[number];
    }
    void EastEdge(Cell cell, int number)
    {
        cell.east = verEdges[number];
    }
    void MazeAlgorithm()
    {
        if (cellCounter > 1)
        {
            MazeHelper(cells[Random.Range(0, sizeHorizontal), Random.Range(0, sizeVertical)]);
            //MazeAlgorithm();
        }
    }
    void MazeHelper(Cell cell)
    {
        int randomSide = Random.Range(0, 4);

        if (randomSide == 0)
        {
            if (cell.cellID % sizeVertical != 0)
            {
                Destroy(cell.north);
            }
            else
            {
                return;
            }
        }
        else if (randomSide == 1)
        {
            if ((cell.cellID + 1) % sizeVertical != 0)
            {
                Destroy(cell.south);
            }
            else
            {
                return;
            }
        }
        else if (randomSide == 2)
        {
            if (cell.cellID > sizeVertical)
            {
                Destroy(cell.west);
            }
            else
            {
                return;
            }
        }
        else if (randomSide == 3)
        {
            if (cell.cellID < sizeHorizontal * sizeVertical - sizeVertical)
            {
                Destroy(cell.east);
            }
            else
            {
                return;
            }
        }
    }
}