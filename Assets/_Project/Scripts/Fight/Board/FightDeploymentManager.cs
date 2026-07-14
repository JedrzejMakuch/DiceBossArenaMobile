using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FightDeploymentManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightArenaGenerator arenaGenerator;
    [SerializeField] private FightStateManager fightStateManager;
    [SerializeField] private Button startFightButton;
    [SerializeField] private FightUnitSpawner unitSpawner;
    [SerializeField] private FightUnit playerUnitPrefab;
    [SerializeField] private Transform playerRoot;

    [Header("Deployment Settings")]
    [SerializeField] private int playerSpawnCount = 4;

    private readonly List<FightGridTile> playerSpawnTiles = new();

    private FightGridTile selectedTile;
    private FightUnit playerUnit;
    private bool isDeploymentLocked = false;

    public FightUnit PlayerUnit => playerUnit;

    private void OnDestroy()
    {
        UnsubscribeFromTiles();
    }

    public void PrepareDeployment()
    {
        UnsubscribeFromTiles();

        if (playerUnit != null &&
            unitSpawner != null)
        {
            unitSpawner.Despawn(playerUnit);
            playerUnit = null;
        }

        playerSpawnTiles.Clear();
        selectedTile = null;
        isDeploymentLocked = false;

        if (arenaGenerator == null)
        {
            Debug.LogError(
                "FightDeploymentManager: Arena Generator is not assigned.",
                this);

            return;
        }

        if (unitSpawner == null)
        {
            Debug.LogError(
                "FightDeploymentManager: FightUnitSpawner is not assigned.",
                this);

            return;
        }

        if (playerUnitPrefab == null)
        {
            Debug.LogError(
                "FightDeploymentManager: Player Unit Prefab is not assigned.",
                this);

            return;
        }

        if (playerUnitPrefab.Definition == null)
        {
            Debug.LogError(
                "FightDeploymentManager: Player prefab has no definition.",
                playerUnitPrefab);

            return;
        }

        if (startFightButton == null)
        {
            Debug.LogError(
                "FightDeploymentManager: Start Fight Button is not assigned.",
                this);

            return;
        }

        startFightButton.interactable = false;

        SubscribeToTiles();
        ChoosePlayerSpawnTiles();

        Debug.Log("Player deployment prepared.");
    }

    private void SubscribeToTiles()
    {
        foreach (FightGridTile tile in arenaGenerator.GeneratedTiles)
        {
            tile.Clicked += HandleTileClicked;
        }
    }

    private void UnsubscribeFromTiles()
    {
        if (arenaGenerator == null)
        {
            return;
        }

        foreach (FightGridTile tile in arenaGenerator.GeneratedTiles)
        {
            if (tile != null)
            {
                tile.Clicked -= HandleTileClicked;
            }
        }
    }

    private void ChoosePlayerSpawnTiles()
    {
        playerSpawnTiles.Clear();

        List<FightGridTile> availableTiles = arenaGenerator.GeneratedTiles
            .Where(tile => tile.IsWalkable && !tile.IsBlocked && !tile.IsOccupied)
            .OrderBy(_ => Random.value)
            .Take(playerSpawnCount)
            .ToList();

        foreach (FightGridTile tile in availableTiles)
        {
            tile.SetPlayerSpawn(true);
            playerSpawnTiles.Add(tile);
        }

        Debug.Log($"Selected {playerSpawnTiles.Count} player spawn tiles.");
    }

    private void HandleTileClicked(FightGridTile clickedTile)
    {
        if (isDeploymentLocked)
        {
            return;
        }

        if (!playerSpawnTiles.Contains(clickedTile))
        {
            Debug.Log($"Clicked tile {clickedTile.GridX}, {clickedTile.GridY}, but it is not a player spawn.");
            return;
        }

        SelectPlayerSpawnTile(clickedTile);
    }

    private void SelectPlayerSpawnTile(
    FightGridTile tile)
    {
        if (selectedTile != null)
        {
            selectedTile.SetPlayerSpawn(true);
        }

        if (playerUnit == null)
        {
            if (!TrySpawnPlayer(tile))
            {
                startFightButton.interactable = false;
                return;
            }
        }
        else if (!playerUnit.TryAssignToTile(tile))
        {
            Debug.LogError(
                $"Failed to move player to tile: " +
                $"{tile.GridX}, {tile.GridY}",
                playerUnit);

            startFightButton.interactable = false;
            return;
        }

        selectedTile = tile;
        selectedTile.SetSelectedVisual();

        startFightButton.interactable = true;

        if (fightStateManager != null)
        {
            fightStateManager.SetReadyToStart();
        }

        Debug.Log(
            $"Player deployed on tile: " +
            $"{selectedTile.GridX}, {selectedTile.GridY}");
    }

    public void LockDeployment()
    {
        isDeploymentLocked = true;
        UnsubscribeFromTiles();

        foreach (FightGridTile tile in playerSpawnTiles)
        {
            if (tile != null)
            {
                tile.SetPlayerSpawn(false);
            }
        }

        Debug.Log("Deployment locked and spawn visuals cleared.");
    }

    private bool TrySpawnPlayer(
    FightGridTile tile)
    {
        FightUnitOwnership ownership =
            new FightUnitOwnership(
                FightTeamId.TeamA,
                new FightParticipantId("local-player"),
                FightControllerType.LocalPlayer);

        Transform parent =
            playerRoot != null
                ? playerRoot
                : transform;

        FightUnitSpawnRequest request =
            new FightUnitSpawnRequest(
                playerUnitPrefab,
                playerUnitPrefab.Definition,
                ownership,
                tile,
                parent,
                "Player");

        playerUnit =
            unitSpawner.Spawn(request);

        return playerUnit != null;
    }
}