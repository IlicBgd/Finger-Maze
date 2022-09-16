using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    int cellCounter = 0;
    private void Awake()
    {
        cells = new Cell[sizeHorizontal, sizeVertical];
        for (int x = 0; x < sizeHorizontal; x++)
        {
            for (int y = 0; y < sizeVertical; y++)
            {
                //cells[x, y] = new Cell();
                cells[x, y] = Instantiate(cellPrefab, new Vector2(0, 0), Quaternion.identity, transform);
                cells[x, y].name = string.Format("Cell number: {0}", cellCounter);
                cells[x, y].cellID = cellCounter;
                cells[x, y].cellNumber = cellCounter;
                cellCounter++;
            }
        }

        scaleX = cellAreaSize.x / sizeHorizontal;
        scaleY = cellAreaSize.y / sizeVertical;

        cellScale = scaleX < scaleY ? new Vector2(scaleX, scaleX) : new Vector2(scaleY, scaleY);

        GenerateCells();
    }
    private void Start()
    {
        //MazeAlgorithm();
        StartCoroutine(RemoveEdgeCoroutine());
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
                Vector2 position = startPosition + new Vector2(cellScale.x * i, -cellScale.y * j + offsetVert);
                cells[i, j].north = Instantiate(horizontalEdge, position, Quaternion.identity, transform);
                position = startPosition + new Vector2(cellScale.x * i - offsetHoriz, -cellScale.y * j);
                cells[i, j].west = Instantiate(verticalEdge, position, Quaternion.identity, transform);
                if (i == sizeHorizontal - 1)
                {
                    position = startPosition + new Vector2(cellScale.x * i + offsetHoriz, -cellScale.y * j);
                    cells[i, j].east = Instantiate(verticalEdge, position, Quaternion.identity, transform);
                }
                if (i != 0)
                {
                    cells[i - 1, j].east = cells[i, j].west;
                }
                if (j == sizeVertical - 1)
                {
                    position = startPosition + new Vector2(cellScale.x * i, -cellScale.y * j - offsetVert);
                    cells[i, j].south = Instantiate(horizontalEdge, position, Quaternion.identity, transform);
                }
                if (j != 0)
                {
                    cells[i, j - 1].south = cells[i, j].north;
                }
            }
        }
    }
    int GetUniqueIDCount()
    {
        List<Cell> tempCells = new List<Cell>();
        for (int i = 0; i < sizeHorizontal; i++)
        {
            for (int j = 0; j < sizeVertical; j++)
            {
                if (tempCells.FirstOrDefault(c => cells[i, j].cellID == c.cellID) == null)
                {
                    tempCells.Add(cells[i, j]);
                }
            }
        }
        return tempCells.Count;
    }
    void MazeAlgorithm()
    {
        if (GetUniqueIDCount() > 1)
        {
            int randomX = Random.Range(0, sizeHorizontal);
            int randomY = Random.Range(0, sizeVertical);
            MazeHelper(cells[randomX, randomY], randomX, randomY);
            Debug.Log(GetUniqueIDCount());
            MazeAlgorithm();
        }
    }
    void MazeHelper(Cell cell, int x, int y)
    {
        int randomSide = cell.GetRandomEdge();
        if (randomSide == 0 && cell.edgesExist == true)
        {
            if (cell.cellNumber % sizeVertical != 0 && cell.cellID != cells[x, y - 1].cellID)
            {
                Destroy(cell.north.gameObject);
                TreeUnifier(cell.cellID, cells[x, y - 1].cellID);
            }
            else
            {
                return;
            }
        }
        else if (randomSide == 1 && cell.edgesExist == true)
        {
            if ((cell.cellNumber + 1) % sizeVertical != 0 && cell.cellID != cells[x, y + 1].cellID)
            {
                Destroy(cell.south.gameObject);
                TreeUnifier(cell.cellID, cells[x, y + 1].cellID);
            }
            else
            {
                return;
            }
        }
        else if (randomSide == 2 && cell.edgesExist == true)
        {
            if (cell.cellNumber >= sizeVertical && cell.cellID != cells[x - 1, y].cellID)
            {
                Destroy(cell.west.gameObject);
                TreeUnifier(cell.cellID, cells[x - 1, y].cellID);
            }
            else
            {
                return;
            }
        }
        else if (randomSide == 3 && cell.edgesExist == true)
        {
            if (cell.cellNumber < sizeHorizontal * sizeVertical - sizeVertical && cell.cellID != cells[x + 1, y].cellID)
            {
                Destroy(cell.east.gameObject);
                TreeUnifier(cell.cellID, cells[x + 1, y].cellID);
            }
            else
            {
                return;
            }
        }
    }
    void TreeUnifier(int oldID, int newID)
    {
        for (int i = 0; i < sizeHorizontal; i++)
        {
            for (int j = 0; j < sizeVertical; j++)
            {
                if (cells[i, j].cellID == oldID)
                {
                    cells[i, j].cellID = newID;
                }
            }
        }
    }
    public IEnumerator RemoveEdgeCoroutine()
    {
        int loopNum = GetUniqueIDCount();
        while (GetUniqueIDCount() > 1)
        {
            int randomX = Random.Range(0, sizeHorizontal);
            int randomY = Random.Range(0, sizeVertical);
            MazeHelper(cells[randomX, randomY], randomX, randomY);
            Debug.Log(GetUniqueIDCount());
            yield return new WaitForSeconds(.2f);
        }
    }
}