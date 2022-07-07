using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChankModul : MonoBehaviour
{
    private float Distance = 999;
    private ChankBuilder ChankBuilderPlayer;
    private Transform ChankBuilderPlayerTransform;
    private float RefineRadius = 0.5f;

    private void Start()
    {
        ChankBuilderPlayer = Transform.FindObjectOfType<ChankBuilder>();
        ChankBuilderPlayerTransform = ChankBuilderPlayer.gameObject.transform;
    }
    private void Update()
    {
        Distance = ChankBuilderPlayer.ChunkDraw * ChankBuilderPlayer.ChunkSize + (ChankBuilderPlayer.ChunkSize * RefineRadius);
        if (Vector3.Distance(ChankBuilderPlayerTransform.position, this.gameObject.transform.position) > Distance)
            Destroy(this.gameObject);
    }


   
}
