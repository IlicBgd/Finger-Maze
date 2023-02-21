using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
public class RingGenerator : MonoBehaviour
{
    public Data data;

    [SerializeField]
    Vector2 cellAreaSize;
    [SerializeField]
    GameObject centerPrefab;
    [SerializeField]
    GameObject middlePrefab;
    [SerializeField]
    GameObject outerPrefab;
    [SerializeField]
    GameObject finish;
    [SerializeField]
    Player player;
    [SerializeField]
    GameObject wallPrefab;

    float scaleX;
    float scaleY;
    float wallDistance = 0;
    float offset = 25;

    Vector2 mazeScale;
    Vector2 startPosition;

    Transform playerStart;

    public Color color;

    public Path path;

    public List<Segment> segments = new List<Segment>();

    public List<SegmentPosition> usedAngles = new List<SegmentPosition>();

    public List<Transform> entrances = new List<Transform>();

    public List<float> angles = new List<float>();

    public int firstRandom;
    public int secondRandom;
    public int thirdRandom;
    public int fourthRandom;
    private void Awake()
    {
        scaleX = cellAreaSize.x;
        scaleY = cellAreaSize.y;

        mazeScale = scaleX < scaleY ? new Vector2(scaleX, scaleX) : new Vector2(scaleY, scaleY);
    }
    private void Start()
    {
        GenerateMaze(data.mediumRingCheck, data.outerRingCheck);
        GeneratePath();
        //Debug.Log(wallDistance);
    }
    void GenerateMaze(bool medRing, bool outerRing)
    {
        startPosition = transform.position;

        transform.localScale = mazeScale;

        if (outerRing)
        {
            wallDistance = 0.4f;
            offset = 30f;
            GameObject thirdRing = Instantiate(outerPrefab, startPosition, Quaternion.Euler(RandomRotation()), transform);
            thirdRing.GetComponent<SpriteRenderer>().color = color;
            transform.localScale = new Vector2(mazeScale.x - 2f, mazeScale.y - 2f);
            Transform largeRingEntrance = thirdRing.transform.GetChild(0);
            playerStart = largeRingEntrance;
            entrances.Add(playerStart);
            player.transform.position = new Vector3(playerStart.transform.position.x, playerStart.transform.position.y, -1f);
        }
        if (medRing)
        {
            GameObject secondRing = Instantiate(middlePrefab, startPosition, Quaternion.Euler(RandomRotation()), transform);
            secondRing.GetComponent<SpriteRenderer>().color = color;
            Transform[] medRingEntrances = new Transform[3] { secondRing.transform.GetChild(0), secondRing.transform.GetChild(1), secondRing.transform.GetChild(2) };

            if (!outerRing)
            {
                secondRing.transform.GetChild(3).gameObject.SetActive(true);
                secondRing.transform.GetChild(4).gameObject.SetActive(true);
                secondRing.transform.GetChild(5).gameObject.SetActive(true);

                wallDistance = 0.45f;
                transform.localScale = new Vector2(mazeScale.x - 1f, mazeScale.y - 1f);
                int randomStart = Random.Range(0, 3);
                offset = 25f;
                //playerStart = medRingEntrances[randomStart];
                //player.transform.position = playerStart.transform.position;
                entrances.Add(medRingEntrances[randomStart]);
                if (randomStart == 0)
                {
                    entrances.Add(medRingEntrances[1]);
                    entrances.Add(medRingEntrances[2]);
                }
                else if (randomStart == 1)
                {
                    entrances.Add(medRingEntrances[0]);
                    entrances.Add(medRingEntrances[2]);
                }
                else if (randomStart == 2)
                {
                    entrances.Add(medRingEntrances[0]);
                    entrances.Add(medRingEntrances[1]);
                }
            }
            else
            {
                entrances.Add(medRingEntrances[0]);
                entrances.Add(medRingEntrances[1]);
                entrances.Add(medRingEntrances[2]);
            }
        }

        GameObject firstRing = Instantiate(centerPrefab, startPosition, Quaternion.Euler(RandomRotation()), transform);
        firstRing.GetComponent<SpriteRenderer>().color = color;

        if (!outerRing && !medRing)
        {
            firstRing.transform.GetChild(7).gameObject.SetActive(true);
            firstRing.transform.GetChild(8).gameObject.SetActive(true);

            wallDistance = 0.55f;
            offset = 15f;
            Transform[] smallRingEntrances = new Transform[2] { firstRing.transform.GetChild(0), firstRing.transform.GetChild(1) };
            int randomFirst = Random.Range(0, 2);
            //playerStart = smallRingEntrances[randomFirst];
            //player.transform.position = playerStart.transform.position;
            entrances.Add(smallRingEntrances[randomFirst]);
            if (randomFirst == 0)
            {
                entrances.Add(smallRingEntrances[1]);
            }
            else if (randomFirst == 1)
            {
                entrances.Add(smallRingEntrances[0]);
            }
            entrances.Add(firstRing.transform.GetChild(2));
            entrances.Add(firstRing.transform.GetChild(3));
            entrances.Add(firstRing.transform.GetChild(4));
            entrances.Add(firstRing.transform.GetChild(5));
            entrances.Add(firstRing.transform.GetChild(6));
        }
        else
        {
            entrances.Add(firstRing.transform.GetChild(0));
            entrances.Add(firstRing.transform.GetChild(1));
            entrances.Add(firstRing.transform.GetChild(2));
            entrances.Add(firstRing.transform.GetChild(3));
            entrances.Add(firstRing.transform.GetChild(4));
            entrances.Add(firstRing.transform.GetChild(5));
            entrances.Add(firstRing.transform.GetChild(6));
        }
    }
    void GeneratePath()
    {
        for (int i = 0; i < entrances.Count; i++)
        {
            angles.Add(AngleFinder(entrances[i]));
        }

        ////////////////////////////////////////
        ///Levo Desno
        ////////////////////////////////////////

        if (data.outerRingCheck)
        {
            firstRandom = Random.Range(1, 4);
            EntrancesSelect(angles[0], angles[firstRandom], 0, 4);
            usedAngles.Sort();
            SegmentsCreator();
            WallsCreator(path);
            usedAngles.Clear();
            segments.Clear();

            secondRandom = Random.Range(4, 6);
            EntrancesSelect(angles[firstRandom], angles[secondRandom], 1, 6);
            usedAngles.Sort();
            SegmentsCreator();
            WallsCreator(path);
            usedAngles.Clear();
            segments.Clear();

            thirdRandom = Random.Range(6, 8);
            EntrancesSelect(angles[secondRandom], angles[thirdRandom], 4, 8);
            usedAngles.Sort();
            SegmentsCreator();
            WallsCreator(path);
            usedAngles.Clear();
            segments.Clear();

            fourthRandom = Random.Range(8, 10);
            EntrancesSelect(angles[thirdRandom], angles[fourthRandom], 6, 9);
            usedAngles.Sort();
            SegmentsCreator();
            WallsCreator(path);
            usedAngles.Clear();
            segments.Clear();

            EntrancesSelect(angles[fourthRandom], angles[10], 8, 11);
            usedAngles.Sort();
            SegmentsCreator();
            WallsCreator(path);
        }
        else
        {
            if (data.mediumRingCheck)
            {
                int randStart = Random.Range(0, 3);
                player.transform.position = entrances[randStart].position;
                firstRandom = Random.Range(3, 5);
                EntrancesSelect(angles[randStart], angles[firstRandom], 0, 5);
                usedAngles.Sort();
                SegmentsCreator();
                WallsCreator(path);
                usedAngles.Clear();
                segments.Clear();

                secondRandom = Random.Range(5, 7);
                EntrancesSelect(angles[firstRandom], angles[secondRandom], 3, 7);
                usedAngles.Sort();
                SegmentsCreator();
                WallsCreator(path);
                usedAngles.Clear();
                segments.Clear();

                thirdRandom = Random.Range(7, 9);
                EntrancesSelect(angles[secondRandom], angles[thirdRandom], 5, 8);
                usedAngles.Sort();
                SegmentsCreator();
                WallsCreator(path);
                usedAngles.Clear();
                segments.Clear();

                EntrancesSelect(angles[thirdRandom], angles[9], 6, 10);
                usedAngles.Sort();
                SegmentsCreator();
                WallsCreator(path);
            }
            else
            {
                int randStart = Random.Range(0, 2);
                player.transform.position = entrances[randStart].position;
                if (randStart == 0)
                {
                    firstRandom = 2;
                    secondRandom = 4;
                }
                else if (randStart == 1)
                {
                    firstRandom = 3;
                    secondRandom = 5;
                }
                //firstRandom = Random.Range(2, 4);
                EntrancesSelect(angles[randStart], angles[firstRandom], 0, 4);
                usedAngles.Sort();
                SegmentsCreator();
                WallsCreator(path);
                usedAngles.Clear();
                segments.Clear();

                //secondRandom = Random.Range(4, 6);
                EntrancesSelect(angles[firstRandom], angles[secondRandom], 2, 5);
                usedAngles.Sort();
                SegmentsCreator();
                WallsCreator(path);
                usedAngles.Clear();
                segments.Clear();

                EntrancesSelect(angles[secondRandom], angles[6], 4, 7);
                usedAngles.Sort();
                SegmentsCreator();
                WallsCreator(path);
            }
        }
    }
    float AngleFinder(Transform x)
    {
        Vector2 distanceX = x.transform.position - finish.transform.position;
        Vector2 directionX = distanceX.normalized;

        float xAngle = Vector2.SignedAngle(Vector3.right, directionX);
        if (xAngle < 0)
        {
            xAngle += 360;
        }

        return xAngle;
    }
    void EntrancesSelect(float x, float y, int entry, int exit)
    {
        if (x > y)
        {
            for (int i = entry; i < exit; i++)
            {
                if (angles[i] <= x && angles[i] >= y)
                {
                    usedAngles.Add(new SegmentPosition(entrances[i], angles[i]));
                }
            }
        }
        else if (x < y)
        {
            for (int i = entry; i < exit; i++)
            {
                if (angles[i] >= x && angles[i] <= y)
                {
                    usedAngles.Add(new SegmentPosition(entrances[i], angles[i]));
                }
            }
        }
    }
    void SegmentsCreator()
    {
        //offset = 20f; //example
        float distanceBetweenPoints;

        float distanceToUse = 0;
        float distanceToMeasure = 0;

        for (int i = 0; i < usedAngles.Count - 1; i++)
        {
            distanceBetweenPoints = Math.Abs(usedAngles[i].angle - usedAngles[i + 1].angle);
            if (distanceBetweenPoints > offset * 2)
            {
                Segment segment = new Segment(usedAngles[i].angle + offset, usedAngles[i + 1].angle - offset);

                segments.Add(segment);
            }
        }
        for (int i = 0; i < usedAngles.Count; i++)
        {
            distanceToMeasure = Vector3.Distance(usedAngles[i].position.position, finish.transform.position);
            if (distanceToMeasure > distanceToUse)
            {
                distanceToUse = distanceToMeasure;
            }
        }

        path = new Path(segments, distanceToUse);
    }
    void WallsCreator(Path path)
    {
        Vector3 position;
        float distance;
        float entranceDistance = 1;
        for (int i = 0; i < path.segments.Count; i++)
        {
            float randAngle = path.segments[i].RandomWallLocation();
            position = Quaternion.Euler(0, 0, randAngle) * Vector3.right * (path.distance - wallDistance) + finish.transform.position;
            GameObject wall;
            wall = Instantiate(wallPrefab, position, Quaternion.identity);
            if (data.outerRingCheck)
            {
                wall.transform.localScale *= 0.75f;
                entranceDistance = 0.6f;
            }
            if (data.outerRingCheck == false && data.mediumRingCheck == false)
            {
                wall.transform.localScale *= 1.1f;
            }
            distance = Vector3.Distance(wall.transform.position, entrances[entrances.Count - 1].position);
            //Debug.Log("Distance is " + distance);
            
            wall.GetComponent<SpriteRenderer>().color = color;
            wall.transform.SetParent(transform);
            Vector2 direction = wall.transform.position - finish.transform.position;
            wall.transform.up = direction.normalized;
            if (distance < entranceDistance)
            {
                wall.transform.localPosition *= -1f;
            }
        }
    }
    private Vector3 RandomRotation()
    {
        float z = Random.Range(0f, 360f);
        Vector3 rotation = new Vector3(0, 0, z);

        return rotation;
    }
    public void MazeSize(bool x, bool y)
    {
        data.mediumRingCheck = x;
        data.outerRingCheck = y;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //Vector2 RandomAngle(Transform x, Transform y, Transform wall)
    //{
    //Vector2 directionX;
    //Vector2 directionY;
    //    Vector2 distanceY = y.transform.position - finish.transform.position;
    //    directionY = distanceY.normalized;
    //    distanceY += directionY * 0.5f;
    //    Vector2 distanceX = x.transform.position - finish.transform.position;
    //    directionX = distanceX.normalized;

    //    float angle = Vector3.Angle(directionY, directionX);
    //    float randAngle = Random.Range(0, angle);

    //    xAngle = Vector2.SignedAngle(Vector3.right, directionX);
    //    //yAngle = Vector2.SignedAngle(Vector3.right, directionY);
    //    randAngle += xAngle;
    //    //anglesList.Add(randAngle);

    //    float posX = Mathf.Cos(randAngle * Mathf.Deg2Rad);
    //    float posY = Mathf.Sin(randAngle * Mathf.Deg2Rad);

    //    Vector3 randPosition = new Vector3(posX, posY) * distanceY.magnitude + finish.transform.position;

    //    wall.position = randPosition;
    //    Vector2 randPosDirection = (randPosition - finish.transform.position).normalized;
    //    wall.up = randPosDirection;
    //}
}
[System.Serializable]
public class Segment
{
    public float from;
    public float to;


    /// ////////////////////////
    public GameObject wall;
    public Transform fromPosition;
    public Transform toPosition;
    //////////////////////////////

    public Segment(float from, float to)
    {
        this.from = from;
        this.to = to;
    }
    public float RandomWallLocation()
    {
        return Random.Range(from, to);
    }
}
[System.Serializable]
public class SegmentPosition : IComparable<SegmentPosition>
{
    public Transform position;
    public float angle;
    public SegmentPosition(Transform pos, float x)
    {
        position = pos;
        angle = x;
    }
    public int CompareTo(SegmentPosition other)
    {
        return angle.CompareTo(other.angle);
    }
}
[System.Serializable]
public class Path
{
    public List<Segment> segments = new List<Segment>();

    public float distance;
    public Path(List<Segment> segments, float distance)
    {
        this.segments = segments;
        this.distance = distance;
    }
}