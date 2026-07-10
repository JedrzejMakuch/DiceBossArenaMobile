using System.Collections.Generic;
using UnityEngine;

public class FightArenaGenerator : MonoBehaviour
{
    [Header("Arena Settings")]
    [SerializeField] private int width = 12;
    [SerializeField] private int height = 12;

    [Header("Tile Settings")]
    [SerializeField] private FightGridTile tilePrefab;
    [SerializeField] private Transform arenaRoot;

    [Header("Iso Spacing")]
    [SerializeField] private float tileSpacingX = 1.28f;
    [SerializeField] private float tileSpacingY = 0.64f;

    private readonly List<FightGridTile> generatedTiles = new();

    public IReadOnlyList<FightGridTile> GeneratedTiles => generatedTiles;

    private void Start()
    {
        GenerateArena();
    }

    public void GenerateArena()
    {
        ClearArena();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                FightGridTile tile = Instantiate(tilePrefab, arenaRoot);

                tile.name = $"FightTile_{x}_{y}";
                tile.transform.position = GridToWorldPosition(x, y);
                tile.Initialize(x, y);

                generatedTiles.Add(tile);
            }
        }
    }

    private Vector3 GridToWorldPosition(int x, int y)
    {
        float worldX = (x - y) * tileSpacingX * 0.5f;
        float worldY = (x + y) * tileSpacingY * 0.5f;

        return new Vector3(worldX, worldY, 0f);
    }

    private void ClearArena()
    {
        generatedTiles.Clear();

        if (arenaRoot == null)
        {
            return;
        }

        for (int i = arenaRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(arenaRoot.GetChild(i).gameObject);
        }
    }
}