using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class TestScript : MonoBehaviour
{
    //public GameObject drawing;
    public GameObject center;

    public Transform[] holes;
    //public float distance;
    //public float angle;
    public Path path;

    public Vector2 directionX;
    public Vector2 directionY;

    public float firstPos;
    public float secondPos;

    public List<Segment> segments = new List<Segment>();

    public List<float> angles = new List<float>();

    public List<SegmentPosition> usedAngles = new List<SegmentPosition>();
    private void Awake()
    {
        for (int i = 0; i < holes.Length; i++)
        {
            angles.Add(AngleFinder(holes[i]));
        }
    }
    private void Start()
    {
        RandomPositions(holes);
        PositionSelect(firstPos, secondPos);
        usedAngles.Sort();
        //SegmentsCreator();
    }
    private void Update()
    {
        //float posX;
        //float posY;
        //posX = Mathf.Cos(angle * Mathf.Deg2Rad);
        //posY = Mathf.Sin(angle * Mathf.Deg2Rad);

        //drawing.transform.position = new Vector3(posX, posY) * distance + transform.position;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(center.transform.position, directionX * 10);
        Gizmos.DrawRay(center.transform.position, directionY * 10);
    }
    float AngleFinder(Transform x)
    {
        Vector2 distanceX = x.transform.position - center.transform.position;
        directionX = distanceX.normalized;

        float xAngle = Vector2.SignedAngle(Vector3.right, directionX);
        if (xAngle < 0)
        {
            xAngle += 360;
        }

        return xAngle;
    }
    void RandomPositions(Transform[] positions)
    {
        int posX = Random.Range(0, positions.Length);
        int posY = Random.Range(0, positions.Length);
        if (posY == posX)
        {
            posY++;
        }
        firstPos = angles[posX];

        Vector2 distanceX = positions[posX].transform.position - center.transform.position;
        directionX = distanceX.normalized;

        secondPos = angles[posY];

        Vector2 distanceY = positions[posY].transform.position - center.transform.position;
        directionY = distanceY.normalized;
    }
    void PositionSelect(float x, float y)
    {
        if (x > y)
        {
            for (int i = 0; i < holes.Length; i++)
            {
                if (angles[i] <= x && angles[i] >= y)
                {
                    usedAngles.Add(new SegmentPosition(holes[i], angles[i]));
                }
            }
        }
        else if (x < y)
        {
            for (int i = 0; i < holes.Length; i++)
            {
                if (angles[i] >= x && angles[i] <= y)
                {
                    usedAngles.Add(new SegmentPosition(holes[i], angles[i]));
                }
            }
        }
    }
    //void SegmentsCreator()
    //{
    //    List<Segment> segments = new List<Segment>();

    //    float offset = 5f; //example

    //    for (int i = 0; i < usedAngles.Count - 1; i++)
    //    {
    //        Segment segment = new Segment(usedAngles[i].angle + offset, usedAngles[i + 1].angle - offset);
    //        segments.Add(segment);
    //    }

    //    path = new Path(segments);
    //}
}
