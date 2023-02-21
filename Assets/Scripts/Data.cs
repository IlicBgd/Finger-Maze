using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data.asset", menuName = "Maze/Create data asset")]
public class Data : ScriptableObject
{
    public int sizeHorizontal;
    public int sizeVertical;

    public bool mediumRingCheck;
    public bool outerRingCheck;
}
