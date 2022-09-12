using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cell
{
    public Edge north;
    public Edge south;
    public Edge west;
    public Edge east;

    public int cellID;

    public int GetRandomEdge()
    {
        List<Edge> edges = new List<Edge>();
        edges.Add(north);
        edges.Add(south);
        edges.Add(west);
        edges.Add(east);

        var tempList = edges.Where(e => e != null).ToList();
        Edge edge = tempList[Random.Range(0, tempList.Count)];
        return edges.IndexOf(edge);
    }
}
