using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChankBuilder : MonoBehaviour
{


    [SerializeField] private GameObject ChunkPrefab;
    [Range(3, 30)] public int ChunkDraw = 3;
    public float ChunkSize = 50;

    private int ChunkDraw_now;

    private List<Vector2> Change_Map_Append = new List<Vector2>();
    private List<Vector2> Change_Map_Delete = new List<Vector2>();

    private Dictionary<Vector2, GameObject> chunks = new Dictionary<Vector2, GameObject>();

    [SerializeField] private Vector2 Player_Position;

    private void Start()
    {
        chunks.Add(Player_Position, FindObjectOfType<ChankModul>().gameObject);
        Redraw_Map();
        ChunkDraw_now = ChunkDraw;
        Player_Position = new Vector2((int)(this.gameObject.transform.position.x / ChunkSize), (int)(this.gameObject.transform.position.y / ChunkSize));
    }

    void Update()
    {
        if(ChunkDraw_now != ChunkDraw)
        {
            Redraw_Map();
            ChunkDraw_now = ChunkDraw;
        }
    }

    public void Update_Player_Position()
    {
        Vector2 Praer_Position_current = new Vector2(Mathf.RoundToInt(this.gameObject.transform.position.x / ChunkSize), Mathf.RoundToInt(this.gameObject.transform.position.z / ChunkSize));
        Create_Change_Map(Praer_Position_current - Player_Position);
        Player_Position = Praer_Position_current;
        Set_Change_Map();
    }

    void Create_Change_Map(Vector2 position_difference)
    {
        Create_Change_Map_Append(position_difference);
        Create_Change_Map_Delete(position_difference);
    }

    async void Create_Change_Map_Append(Vector2 position_difference)
    {
        Change_Map_Append.Clear();
        for (int i = -ChunkDraw; i < ChunkDraw +1; i++)
        {
            if (position_difference.x != 0)
                Change_Map_Append.Add(new Vector2(ChunkDraw * position_difference.x, i));
            if (position_difference.y != 0)
                Change_Map_Append.Add(new Vector2(i, ChunkDraw * position_difference.y));
        }
        if(position_difference.x != 0 && position_difference.y != 0)
            Change_Map_Append.Add(new Vector2((ChunkDraw) * position_difference.x, (ChunkDraw) * position_difference.y));
    }
    async void Create_Change_Map_Delete(Vector2 position_difference)
    {
        Change_Map_Delete.Clear();
        for (int i = -ChunkDraw; i < ChunkDraw + 1; i++)
        {
            if (position_difference.x != 0)
                Change_Map_Delete.Add(new Vector2(ChunkDraw * -position_difference.x + (-position_difference.x) , i));
            if (position_difference.y != 0)
                Change_Map_Delete.Add(new Vector2(i, ChunkDraw * -position_difference.y + (-position_difference.y)));
        }
        if (position_difference.x != 0 && position_difference.y != 0)
            Change_Map_Delete.Add(new Vector2((ChunkDraw) * -position_difference.x + (-position_difference.x), (ChunkDraw) * -position_difference.y + (-position_difference.y)));
    }

    void Set_Change_Map()
    {
        Set_Change_Map_Append();
        Set_Change_Map_Delete();
    }
    async void Set_Change_Map_Append()
    {
        foreach(Vector2 item in Change_Map_Append)
        {
            Vector2 pos = item + Player_Position;
            chunks.Add(pos, Instantiate(ChunkPrefab, new Vector3(pos.x * ChunkSize, 0, pos.y * ChunkSize), Quaternion.identity));
        }
    }
    async void Set_Change_Map_Delete()
    {
        foreach (Vector2 item in Change_Map_Delete)
        {
            Vector2 pos = item + Player_Position;
            if (chunks.TryGetValue(pos, out GameObject chunk))
            {
                Destroy(chunk);
                chunks.Remove(pos);
            }
        }
    }

    void Redraw_Map()
    {

        foreach (GameObject item in chunks.Values)
        {
            Destroy(item);
        }

        chunks.Clear();
        

        for(int i = -ChunkDraw; i < ChunkDraw + 1; i++)
        {
            for (int j = -ChunkDraw; j < ChunkDraw + 1; j++)
            {
                Vector2 pos = new Vector2(i, j) + Player_Position;
                chunks.Add(pos, Instantiate(ChunkPrefab, new Vector3(pos.x * ChunkSize, 0, pos.y * ChunkSize), Quaternion.identity));
            }
        }
    }
}
