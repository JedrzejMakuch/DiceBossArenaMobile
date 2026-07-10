using System.Collections.Generic;
using UnityEngine;

public class BoardEventManager : MonoBehaviour
{
    [SerializeField] private BoardGenerator boardGenerator;
    [SerializeField] private TileEventData normalTileEvent;
    [SerializeField] private List<TileEventSpawnConfig> eventSpawnConfigs = new List<TileEventSpawnConfig>();

    private List<BoardTile> tiles => boardGenerator.GeneratedTiles;

    private void Start()
    {
        RandomizeEvents();
    }

    private List<TileEventData> BuildEventPool()
    {
        List<TileEventData> eventPool = new List<TileEventData>();

        foreach (TileEventSpawnConfig config in eventSpawnConfigs)
        {
            for (int i = 0; i < config.Count; i++)
            {
                eventPool.Add(config.EventData);
            }
        }

        return eventPool;
    }

    private List<BoardTile> GetFreeTilesExceptStart()
    {
        List<BoardTile> freeTiles = new List<BoardTile>();

        for (int i = 1; i < tiles.Count; i++)
        {
            if (tiles[i] != null)
            {
                freeTiles.Add(tiles[i]);
            }
        }

        return freeTiles;
    }

    public void RandomizeEvents()
    {
        ClearAllTiles();

        if (tiles.Count == 0)
        {
            Debug.LogWarning("BoardEventManager: Tiles list is empty.");
            return;
        }

        if (normalTileEvent == null)
        {
            Debug.LogWarning("BoardEventManager: Normal tile event is missing.");
            return;
        }

        tiles[0].SetEvent(normalTileEvent);

        List<TileEventData> eventPool = BuildEventPool();
        List<BoardTile> freeTiles = GetFreeTilesExceptStart();

        foreach (TileEventData tileEvent in eventPool)
        {
            if (freeTiles.Count == 0)
            {
                Debug.LogWarning("BoardEventManager: Not enough free tiles for all events.");
                break;
            }

            int randomIndex = Random.Range(0, freeTiles.Count);
            BoardTile selectedTile = freeTiles[randomIndex];

            selectedTile.SetEvent(tileEvent);
            freeTiles.RemoveAt(randomIndex);
        }

        FillEmptyTilesWithNormal();
    }

    private void ClearAllTiles()
    {
        foreach (BoardTile tile in tiles)
        {
            if (tile != null)
            {
                tile.ClearEvent();
            }
        }
    }

    private void FillEmptyTilesWithNormal()
    {
        foreach (BoardTile tile in tiles)
        {
            if (tile == null)
                continue;

            if (!tile.HasEvent)
            {
                tile.SetEvent(normalTileEvent);
            }
        }
    }
}