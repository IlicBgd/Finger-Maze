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

    List<Cell> cellList = new List<Cell>();
    private void Awake()
    {
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

        int counter = 0;
        for (int i = 0; i < sizeHorizontal; i++)
        {
            Edge horEdge;
            Vector2 horPosition = startPosition + new Vector2(cellScale.x * i, -cellScale.y * i);

            for (int j = 0; j < sizeVertical; j++)
            {
                Cell cell;
                Vector2 newPosition = startPosition + new Vector2(cellScale.x * i, -cellScale.y * j);
                cell = Instantiate(cellPrefab, newPosition, Quaternion.identity, transform);
                cell.transform.localScale = cellScale;
                cell.name = string.Format("Cell number: {0}", counter++);
                cellList.Add(cell);
            }
        }
    }
}