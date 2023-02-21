using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class CellsGenerator : MonoBehaviour
{
    public Data data;
    public int sizeHorizontal => data.sizeHorizontal;
    public int sizeVertical => data.sizeVertical;

    [SerializeField]
    Cell cellPrefab;
    [SerializeField]
    Edge horizontalEdge;
    [SerializeField]
    Edge verticalEdge;
    [SerializeField]
    Vector2 cellAreaSize;
    [SerializeField]
    Player player;
    [SerializeField]
    GameObject startArrow;
    [SerializeField]
    GameObject exitArrow;

    Vector2 cellScale;
    Vector2 startPosition;

    float scaleX;
    float scaleY;

    //Color color;

    Cell[,] cells;

    int cellCounter = 0;

    Edge[] leftSideEdges;
    Edge[] rightSideEdges;
    private void Awake()
    {
        //color = new Color(0.241f, 0.250f, 0.238f, 1f);

        cells = new Cell[sizeHorizontal, sizeVertical];
        for (int x = 0; x < sizeHorizontal; x++)
        {
            for (int y = 0; y < sizeVertical; y++)
            {
                cells[x, y] = new Cell();
                //cells[x, y] = Instantiate(cellPrefab, new Vector2(0, 0), Quaternion.identity, transform);
                //cells[x, y].name = string.Format("Cell number: {0}", cellCounter);
                cells[x, y].cellID = cellCounter;
                cells[x, y].cellNumber = cellCounter;
                cellCounter++;
            }
        }
        if (sizeHorizontal > 12)
        {
            player.transform.localScale *= 0.8f;
        }
        else if (sizeHorizontal == 12)
        {
            player.transform.localScale *= 0.9f;
        }
        else if (sizeHorizontal < 12)
        {
            player.transform.localScale *= 1f;
        }

        scaleX = cellAreaSize.x / sizeHorizontal;
        scaleY = cellAreaSize.y / sizeVertical;

        cellScale = scaleX < scaleY ? new Vector2(scaleX, scaleX) : new Vector2(scaleY, scaleY);

        GenerateCells();
    }
    private void Start()
    {
        MazeAlgorithm();
        //StartCoroutine(RemoveEdgeCoroutine());
        EntranceExitRandomizer();
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

        leftSideEdges = new Edge[sizeVertical];
        rightSideEdges = new Edge[sizeVertical];

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
                    rightSideEdges[j] = cells[i, j].east;
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
                if (i == 0)
                {
                    leftSideEdges[j] = cells[i, j].west;
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
            MazeAlgorithm();
        }
    }
    public void MazeSize(int x, int y)
    {
        data.sizeHorizontal = x;
        data.sizeVertical = y;

        SceneManager.LoadScene(1);
    }
    void MazeHelper(Cell cell, int x, int y)
    {
        //int randomSide;
        //try
        //{
        //    randomSide = cell.GetRandomEdge();
        //}
        //catch (System.Exception)
        //{
        //    return;
        //}
        int randomSide = cell.GetRandomEdge();
        if (randomSide == -1)
        {
            return;
        }
        if (randomSide == 0)
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
        else if (randomSide == 1)
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
        else if (randomSide == 2)
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
        else if (randomSide == 3)
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
    void EntranceExitRandomizer()
    {
        int randomLeft = Random.Range(0, leftSideEdges.Length);
        int randomRight = Random.Range(0, rightSideEdges.Length);

        Destroy(rightSideEdges[randomRight].gameObject);
        leftSideEdges[randomLeft].spriteRenderer.enabled = false;

        float xPos = leftSideEdges[randomLeft].transform.position.x;
        float yPos = leftSideEdges[randomLeft].transform.position.y;
        float offset = horizontalEdge.spriteRenderer.bounds.size.x / 2.1f;
        if (sizeHorizontal > 12)
        {
            offset = horizontalEdge.spriteRenderer.bounds.size.x / 2.6f;
        }

        player.transform.position = new Vector2(xPos + offset, yPos);

        startArrow.transform.position = new Vector2(xPos - offset, yPos);
        exitArrow.transform.position = new Vector2(rightSideEdges[randomRight].transform.position.x + offset, rightSideEdges[randomRight].transform.position.y);
    }
    //public IEnumerator RemoveEdgeCoroutine()
    //{
    //    int loopNum = GetUniqueIDCount();
    //    while (GetUniqueIDCount() > 1)
    //    {
    //        int randomX = Random.Range(0, sizeHorizontal);
    //        int randomY = Random.Range(0, sizeVertical);
    //        MazeHelper(cells[randomX, randomY], randomX, randomY);
    //        Debug.Log(GetUniqueIDCount());
    //        yield return new WaitForSeconds(.2f);
    //    }
    //}
}