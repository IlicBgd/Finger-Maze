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
    private void Awake()
    {
        cells = new Cell[sizeHorizontal,sizeVertical];
        for (int x = 0; x < sizeHorizontal; x++)
        {
            for (int y = 0; y < sizeVertical; y++)
            {
                cells[x, y] = new Cell();
            }
        }

        scaleX = cellAreaSize.x / sizeHorizontal;
        scaleY = cellAreaSize.y / sizeVertical;

        cellScale = scaleX < scaleY ? new Vector2(scaleX, scaleX) : new Vector2(scaleY, scaleY);

        GenerateCells();
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

        /////////////////////////////////
        //GENERATE EDGES BASED ON CELLS//
        /////////////////////////////////

        for (int i = 0; i < sizeHorizontal; i++)
        {
            for (int j = 0; j < sizeVertical; j++)
            {
                Edge horEdge;
                Edge verEdge;
                Vector2 position = startPosition + new Vector2(cellScale.x * i, -cellScale.y * j + offsetVert);
                horEdge = Instantiate(horizontalEdge, position, Quaternion.identity, transform);
                position = startPosition + new Vector2(cellScale.x * i - offsetHoriz, -cellScale.y * j);
                verEdge = Instantiate(verticalEdge, position, Quaternion.identity, transform);
                if (i == sizeHorizontal - 1)
                {
                    position = startPosition + new Vector2(cellScale.x * i + offsetHoriz, -cellScale.y * j);
                    verEdge = Instantiate(verticalEdge, position, Quaternion.identity, transform);
                }
                if (j == sizeVertical - 1)
                {
                    position = startPosition + new Vector2(cellScale.x * i, -cellScale.y * j - offsetVert);
                    horEdge = Instantiate(horizontalEdge, position, Quaternion.identity, transform);
                }
            }
        }
    }
}