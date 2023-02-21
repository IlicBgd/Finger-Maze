using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RingMazeSizeButton : MonoBehaviour
{
    [SerializeField]
    RingGenerator ringGenerator;
    [SerializeField]
    Button button;
    [SerializeField]
    GameObject buttonSize;

    bool medRing;
    bool outerRing;

    private void Start()
    {
        button.onClick.AddListener(ButtonCheck);
        button.onClick.AddListener(MazeSize);
    }
    void MazeSize()
    {
        ringGenerator.MazeSize(medRing, outerRing);
    }
    void ButtonCheck()
    {
        if (buttonSize.name == "SmallMazeButton")
        {
            medRing = false;
            outerRing = false;
        }
        else if (buttonSize.name == "MediumMazeButton")
        {
            medRing = true;
            outerRing = false;
        }
        else if (buttonSize.name == "LargeMazeButton")
        {
            medRing = true;
            outerRing = true;
        }
    }
}
