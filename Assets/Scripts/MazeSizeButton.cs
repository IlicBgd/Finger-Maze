using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeSizeButton : MonoBehaviour
{
    [SerializeField]
    CellsGenerator cellsGenerator;
    [SerializeField]
    Button button;
    [SerializeField]
    int sizeX = 0;
    [SerializeField]
    int sizeY = 0;

    private void Start()
    {
        button.onClick.AddListener(MazeSize);
    }
    void MazeSize()
    {
        cellsGenerator.MazeSize(sizeX, sizeY);
    }
}
