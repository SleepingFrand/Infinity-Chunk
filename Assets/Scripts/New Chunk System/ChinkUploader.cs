using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChinkUploader : MonoBehaviour
{
    [SerializeField] private GameObject ChunkPrefab;

    [SerializeField]private int ChunksRadius = 4;
    private float ChunksSize = 50;

    private Dictionary<Vector2, ChankModul> Chunks = new Dictionary<Vector2, ChankModul>();

    private Vector2 PlayerPosNow = new Vector2();
    private Vector2 PlayerPosLast = new Vector2(-1,-1);

    // Start is called before the first frame update
    void Start()
    {
        PlayerPosLast = PlayerPosNow - new Vector2(1,1);
       foreach (ChankModul item in FindObjectsOfType<ChankModul>())
        {
            Chunks.Add(new Vector2(item.gameObject.transform.position.x / 50, item.gameObject.transform.position.z / 50), item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPosNow = new Vector2(Mathf.RoundToInt(this.gameObject.transform.position.x / ChunksSize), Mathf.RoundToInt(this.gameObject.transform.position.z / ChunksSize));
        if(PlayerPosNow != PlayerPosLast)
        {
            PlayerPosLast = PlayerPosNow;
            ChangeChunks();

        }

        foreach(Vector2 item in Chunks.Keys)
        {
            Debug.DrawLine(new Vector3(PlayerPosNow.x * ChunksSize, 5, PlayerPosNow.y * ChunksSize), new Vector3(item.x * ChunksSize, 0, item.y * ChunksSize));
        }
    }

    async void ChangeChunks()
    {
        StartCoroutine(DestroyOldChunk());

        if (Chunks.Count < ChunkCurrentCount())
        for(int i = -ChunksRadius; i  < ChunksRadius + 1; i++)
        {
                StartCoroutine(CreateLineChunk(i));
        }
    }

    IEnumerator CreateLineChunk(int line)
    {
        for (int j = -ChunksRadius; j < ChunksRadius + 1; j++)
        {
            if (!Chunks.ContainsKey(new Vector2(line + PlayerPosNow.x, j + PlayerPosNow.y)))
            {
                Chunks.Add(new Vector2(line + PlayerPosNow.x, j + PlayerPosNow.y), Instantiate(ChunkPrefab, GetChunkPos(line, j), Quaternion.identity).GetComponent<ChankModul>());
            }
        }

        yield return null;
    }


    IEnumerator DestroyOldChunk()
    {
        List<Vector2> remove = new List<Vector2>();
        foreach (Vector2 item in Chunks.Keys)
        {
            if (Mathf.Abs((item - PlayerPosNow).x) > ChunksRadius || Mathf.Abs((item - PlayerPosNow).y) > ChunksRadius)
            {
                Destroy(Chunks[item].gameObject);
                remove.Add(item);
            }
        }

        foreach (Vector2 item in remove)
        {
            Chunks.Remove(item);
        }

        yield return null;
    }

    Vector3 GetChunkPos(Vector2 chunkVec2Pos)
    {
        return new Vector3((chunkVec2Pos.x + PlayerPosNow.x) * ChunksSize,
            0, (chunkVec2Pos.y + PlayerPosNow.y) * ChunksSize);
    }
    Vector3 GetChunkPos(int chunkVec2PosX, int chunkVec2PosY)
    {
        return new Vector3((chunkVec2PosX + PlayerPosNow.x) * ChunksSize,
            0, (chunkVec2PosY + PlayerPosNow.y) * ChunksSize);
    }

    int ChunkCurrentCount()
    {
        return (ChunksRadius + ChunksRadius + 1) * (ChunksRadius + ChunksRadius + 1);
    }
}
