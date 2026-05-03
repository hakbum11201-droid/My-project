using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageTileGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Tilemap groundTilemap;

    [Header("Ground Tiles")]
    [SerializeField] private TileBase groundDirt01;
    [SerializeField] private TileBase groundDirt02;
    [SerializeField] private TileBase groundGrass01;
    [SerializeField] private TileBase groundGrass02;
    [SerializeField] private TileBase groundStone01;
    [SerializeField] private TileBase groundCrack01;

    [Header("Generation Settings")]
    [SerializeField] private int chunkSize = 16;
    [SerializeField] private int activeChunkRadius = 2;
    [SerializeField] private int seed = 12345;
    [SerializeField] private bool clearExistingTilesOnStart = true;

    private readonly HashSet<Vector2Int> generatedChunks = new HashSet<Vector2Int>();
    private Vector2Int currentPlayerChunk;

    private void Start()
    {
        if (player == null || groundTilemap == null)
        {
            Debug.LogError("StageTileGenerator: Player 또는 Ground Tilemap이 연결되지 않았습니다.");
            enabled = false;
            return;
        }

        if (clearExistingTilesOnStart)
        {
            groundTilemap.ClearAllTiles();
        }

        currentPlayerChunk = GetPlayerChunk();
        GenerateChunksAroundPlayer();
    }

    private void Update()
    {
        Vector2Int newPlayerChunk = GetPlayerChunk();

        if (newPlayerChunk == currentPlayerChunk)
            return;

        currentPlayerChunk = newPlayerChunk;

        GenerateChunksAroundPlayer();
        RemoveFarChunks();
    }

    private Vector2Int GetPlayerChunk()
    {
        Vector3Int playerCell = groundTilemap.WorldToCell(player.position);

        int chunkX = Mathf.FloorToInt((float)playerCell.x / chunkSize);
        int chunkY = Mathf.FloorToInt((float)playerCell.y / chunkSize);

        return new Vector2Int(chunkX, chunkY);
    }

    private void GenerateChunksAroundPlayer()
    {
        for (int y = -activeChunkRadius; y <= activeChunkRadius; y++)
        {
            for (int x = -activeChunkRadius; x <= activeChunkRadius; x++)
            {
                Vector2Int chunkPosition = currentPlayerChunk + new Vector2Int(x, y);

                if (generatedChunks.Contains(chunkPosition))
                    continue;

                GenerateChunk(chunkPosition);
                generatedChunks.Add(chunkPosition);
            }
        }
    }

    private void GenerateChunk(Vector2Int chunkPosition)
    {
        int startX = chunkPosition.x * chunkSize;
        int startY = chunkPosition.y * chunkSize;

        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                int tileX = startX + x;
                int tileY = startY + y;

                TileBase selectedTile = PickGroundTile(tileX, tileY);

                Vector3Int cellPosition = new Vector3Int(tileX, tileY, 0);
                groundTilemap.SetTile(cellPosition, selectedTile);
            }
        }
    }

    private TileBase PickGroundTile(int x, int y)
    {
        float value = GetStableRandom01(x, y);

        if (value < 0.55f)
            return groundDirt01;

        if (value < 0.75f)
            return groundDirt02;

        if (value < 0.83f)
            return groundGrass01;

        if (value < 0.88f)
            return groundGrass02;

        if (value < 0.96f)
            return groundStone01;

        return groundCrack01;
    }

    private float GetStableRandom01(int x, int y)
    {
        int hash = seed;
        hash ^= x * 73856093;
        hash ^= y * 19349663;
        hash = (hash << 13) ^ hash;

        int result = (hash * (hash * hash * 15731 + 789221) + 1376312589);
        result &= 0x7fffffff;

        return result / 2147483647f;
    }

    private void RemoveFarChunks()
    {
        List<Vector2Int> chunksToRemove = new List<Vector2Int>();

        foreach (Vector2Int chunk in generatedChunks)
        {
            int distanceX = Mathf.Abs(chunk.x - currentPlayerChunk.x);
            int distanceY = Mathf.Abs(chunk.y - currentPlayerChunk.y);

            if (distanceX > activeChunkRadius || distanceY > activeChunkRadius)
            {
                chunksToRemove.Add(chunk);
            }
        }

        foreach (Vector2Int chunk in chunksToRemove)
        {
            ClearChunk(chunk);
            generatedChunks.Remove(chunk);
        }
    }

    private void ClearChunk(Vector2Int chunkPosition)
    {
        int startX = chunkPosition.x * chunkSize;
        int startY = chunkPosition.y * chunkSize;

        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                Vector3Int cellPosition = new Vector3Int(startX + x, startY + y, 0);
                groundTilemap.SetTile(cellPosition, null);
            }
        }
    }
}