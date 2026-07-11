using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightArenaGenerator arenaGenerator;
    [SerializeField] private GameObject enemyPlaceholderPrefab;
    [SerializeField] private Transform enemiesRoot;

    [Header("Spawn Settings")]
    [SerializeField, Min(1)] private int enemyCount = 3;

    private readonly List<GameObject> spawnedEnemies = new();
    private readonly List<FightGridTile> enemySpawnTiles = new();

    public IReadOnlyList<GameObject> SpawnedEnemies => spawnedEnemies;
    public IReadOnlyList<FightGridTile> EnemySpawnTiles => enemySpawnTiles;

    public void SpawnEnemies()
    {
        if (arenaGenerator == null)
        {
            Debug.LogError(
                "EnemySpawnManager: FightArenaGenerator is not assigned.",
                this);

            return;
        }

        if (enemyPlaceholderPrefab == null)
        {
            Debug.LogError(
                "EnemySpawnManager: Enemy Placeholder Prefab is not assigned.",
                this);

            return;
        }

        ClearSpawnedEnemies();

        List<FightGridTile> availableTiles = arenaGenerator.GeneratedTiles
            .Where(CanSpawnEnemyOnTile)
            .OrderBy(_ => Random.value)
            .Take(enemyCount)
            .ToList();

        if (availableTiles.Count < enemyCount)
        {
            Debug.LogWarning(
                $"EnemySpawnManager: Requested {enemyCount} enemies, " +
                $"but only {availableTiles.Count} valid tiles were found.",
                this);
        }

        foreach (FightGridTile tile in availableTiles)
        {
            SpawnEnemy(tile);
        }

        Debug.Log($"Spawned {spawnedEnemies.Count} enemies.");
    }

    private bool CanSpawnEnemyOnTile(FightGridTile tile)
    {
        return tile != null
            && tile.IsWalkable
            && !tile.IsBlocked
            && !tile.IsOccupied
            && !tile.CanPlayerSpawn;
    }

    private void SpawnEnemy(FightGridTile tile)
    {
        Transform parent = enemiesRoot != null
            ? enemiesRoot
            : transform;

        GameObject enemy = Instantiate(
            enemyPlaceholderPrefab,
            tile.GetStandPosition(),
            Quaternion.identity,
            parent);

        enemy.name = $"EnemyPlaceholder_{tile.GridX}_{tile.GridY}";

        tile.SetEnemySpawn(true);
        tile.SetOccupied(true);

        spawnedEnemies.Add(enemy);
        enemySpawnTiles.Add(tile);
    }

    private void ClearSpawnedEnemies()
    {
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        foreach (FightGridTile tile in enemySpawnTiles)
        {
            if (tile == null)
            {
                continue;
            }

            tile.SetEnemySpawn(false);
            tile.SetOccupied(false);
        }

        spawnedEnemies.Clear();
        enemySpawnTiles.Clear();
    }
}