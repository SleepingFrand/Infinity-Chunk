using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChankBuilder : MonoBehaviour
{
    [SerializeField] private GameObject ChunkPrefab;
    [Range(3, 30)] public int ChunkDraw = 3;
    public float ChunkSize = 50;

    private List<ChankModul> chunks = new List<ChankModul> { };
    private List<Vector2> PosihonChunks = new List<Vector2> { };

    void Update()
    {
        chunks.AddRange(GameObject.FindObjectsOfType<ChankModul>());
        CreateVector2Positions();
        Putchunks();

        foreach (ChankModul item in chunks)
        {
            if (Vector3.Distance(this.transform.position, item.gameObject.transform.position) < ChunkDraw * ChunkSize)
            {
                Debug.DrawLine(this.transform.position, item.transform.position, Color.green);
            }
            else
            {
                Debug.DrawLine(this.transform.position, item.transform.position, Color.red);
            }
        }

        PosihonChunks.Clear();
        chunks.Clear();
    }

    void CreateVector2Positions()
    {
        Vector2 center = new Vector2(Mathf.RoundToInt(this.gameObject.transform.position.x / ChunkSize),
            Mathf.RoundToInt(this.gameObject.transform.position.z / ChunkSize));
        PosihonChunks.Add(center);
        bool drawedSomething = true;

        List<Vector2> corners = new List<Vector2> {
            new Vector2(center.x + 1f, center.y - 1),
            new Vector2(center.x - 1f, center.y - 1),
            new Vector2(center.x - 1f, center.y + 1),
            new Vector2(center.x + 1, center.y + 1),
            };

        for (int i = 1; drawedSomething; i++)
        {
            drawedSomething = false;
            if (i > 1)
                corners = new List<Vector2> {
            new Vector2(corners[0].x+1, corners[0].y-1),
            new Vector2(corners[1].x-1, corners[1].y-1),
            new Vector2(corners[2].x-1, corners[2].y+1),
            new Vector2(corners[3].x+1, corners[3].y+1),
            };
            int direction = 0;
            Vector2 current = corners[0];
            bool first = true;
            bool end = false;
            while (!end)
            { 
                switch (direction)
                {
                    case 0:
                        if (current.x > corners[1].x)
                        {
                            if (first) { first = false; }
                            else current = new Vector2(current.x - 1, current.y);
                        }
                        else
                        {
                            direction++; continue;
                        }
                        break;
                    case 1:
                        if (current.y < corners[2].y)
                        {
                            current = new Vector2(current.x, current.y + 1);
                        }
                        else
                        {
                            direction++; continue;
                        }
                        break;
                    case 2:
                        if (current.x < corners[3].x)
                        {
                            current = new Vector2(current.x + 1, current.y);
                        }
                        else
                        {
                            direction++; continue;
                        }
                        break;
                    case 3:
                        if (current.y > corners[0].y + 1)
                        {
                            current = new Vector2(current.x, current.y - 1);
                        }
                        else
                        {
                            end = true; continue;
                        }
                        break;
                }
                if (Vector2.Distance(center, current) < ChunkDraw)
                {
                    if (!CheakPosition(current))
                    {
                        drawedSomething = true;
                        PosihonChunks.Add(current);
                    }
                }
            }
        }
    }

    bool CheakPosition(Vector2 chunk_position)
    {
        foreach (Vector2 item in PosihonChunks)
        {
            if (item == chunk_position)
                return true;
        }

        return false;
    }

    void Putchunks()
    {
        bool newchunk = true;
        foreach (Vector2 point in PosihonChunks)
        {
            newchunk = true;
            foreach (ChankModul chunk in chunks)
            {
                if (chunk.gameObject.transform.position == new Vector3(point.x * ChunkSize, 0, point.y * ChunkSize))
                {
                    newchunk = false;
                    break;
                }
            }
            if (newchunk)
            {
                GameObject chaukGO = Instantiate(ChunkPrefab, new Vector3(point.x * ChunkSize, 0, point.y * ChunkSize), Quaternion.identity);
                chunks.Add(chaukGO.GetComponent<ChankModul>());
            }
        }
    }
}
