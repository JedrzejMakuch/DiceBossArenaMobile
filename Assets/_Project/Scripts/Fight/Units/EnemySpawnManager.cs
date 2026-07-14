using DiceBossArena.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightArenaGenerator arenaGenerator;
    [SerializeField] private FightUnit enemyUnitPrefab;
    [SerializeField] private Transform enemiesRoot;
    [SerializeField] private FightUnitSpawner unitSpawner;

    [Header("Spawn Settings")]
    [SerializeField, Min(1)] private int enemyCount = 3;

    private readonly List<FightUnit> spawnedEnemies = new();
    private readonly List<FightGridTile> enemySpawnTiles = new();

    public IReadOnlyList<FightUnit> SpawnedEnemies => spawnedEnemies;
    public IReadOnlyList<FightGridTile> EnemySpawnTiles => enemySpawnTiles;

    private CharacterBuildSnapshot enemyBuildSnapshot =
    CharacterBuildSnapshot.Empty;

    private FightUnitRuntimeSnapshot enemyRuntimeSnapshot =
        FightUnitRuntimeSnapshot.Fresh;

    public CharacterBuildSnapshot EnemyBuildSnapshot =>
    enemyBuildSnapshot;

    public FightUnitRuntimeSnapshot EnemyRuntimeSnapshot =>
        enemyRuntimeSnapshot;

    public event Action EnemiesSpawned;

    public void ConfigureSpawnData(
    CharacterBuildSnapshot buildSnapshot,
    FightUnitRuntimeSnapshot runtimeSnapshot)
    {
        enemyBuildSnapshot =
            (buildSnapshot ??
             CharacterBuildSnapshot.Empty)
            .Copy();

        enemyRuntimeSnapshot =
            runtimeSnapshot ??
            FightUnitRuntimeSnapshot.Fresh;
    }

    public void SpawnEnemies()
    {
        if (arenaGenerator == null)
        {
            Debug.LogError(
                "EnemySpawnManager: FightArenaGenerator is not assigned.",
                this);

            return;
        }

        if (enemyUnitPrefab == null)
        {
            Debug.LogError(
                "EnemySpawnManager: Enemy Unit Prefab is not assigned.",
                this);

            return;
        }

        if (unitSpawner == null)
        {
            Debug.LogError(
                "EnemySpawnManager: FightUnitSpawner is not assigned.",
                this);

            return;
        }

        ClearSpawnedEnemies();

        List<FightGridTile> availableTiles = arenaGenerator.GeneratedTiles
            .Where(CanSpawnEnemyOnTile)
            .OrderBy(_ => UnityEngine.Random.value)
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

        EnemiesSpawned?.Invoke();

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
        string enemyName =
            $"Enemy_{tile.GridX}_{tile.GridY}";

        FightUnitDefinition definition =
            enemyUnitPrefab.Definition;

        if (definition == null)
        {
            Debug.LogError(
                "EnemySpawnManager: Enemy prefab has no definition.",
                enemyUnitPrefab);

            return;
        }

        FightUnitOwnership ownership =
            new FightUnitOwnership(
                FightTeamId.TeamB,
                new FightParticipantId("enemy-ai"),
                FightControllerType.AI);

        Transform parent =
            enemiesRoot != null
                ? enemiesRoot
                : transform;

        FightUnitSpawnRequest request =
            new FightUnitSpawnRequest(
                prefab:
                    enemyUnitPrefab,
                definition:
                    definition,
                ownership:
                    ownership,
                tile:
                    tile,
                parent:
                    parent,
                objectName:
                    enemyName,
                runtimeSnapshot:
                    enemyRuntimeSnapshot,
                buildSnapshot:
                    enemyBuildSnapshot);

        FightUnit enemy =
            unitSpawner.Spawn(request);

        if (enemy == null)
        {
            return;
        }

        tile.SetEnemySpawn(true);

        spawnedEnemies.Add(enemy);
        enemySpawnTiles.Add(tile);
    }

    private void ClearSpawnedEnemies()
    {
        foreach (FightUnit enemy in spawnedEnemies)
        {
            if (enemy == null)
            {
                continue;
            }

            unitSpawner.Despawn(enemy);
        }

        foreach (FightGridTile tile in enemySpawnTiles)
        {
            if (tile != null)
            {
                tile.SetEnemySpawn(false);
            }
        }

        spawnedEnemies.Clear();
        enemySpawnTiles.Clear();
    }
}