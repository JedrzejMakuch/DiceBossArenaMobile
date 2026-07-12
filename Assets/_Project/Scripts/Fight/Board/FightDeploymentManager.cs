using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FightDeploymentManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightArenaGenerator arenaGenerator;
    [SerializeField] private FightStateManager fightStateManager;
    [SerializeField] private FightUnit playerUnit;
    [SerializeField] private Button startFightButton;

    [Header("Deployment Settings")]
    [SerializeField] private int playerSpawnCount = 4;

    private readonly List<FightGridTile> playerSpawnTiles = new();
    private FightGridTile selectedTile;
    private bool isDeploymentLocked = false;

    public FightUnit PlayerUnit => playerUnit;

    private void OnDestroy()
    {
        UnsubscribeFromTiles();
    }

    public void PrepareDeployment()
    {
        UnsubscribeFromTiles();

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

        if (playerUnit == null)
        {
            Debug.LogError(
                "FightDeploymentManager: Player Unit is not assigned.",
                this);

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

    private void SelectPlayerSpawnTile(FightGridTile tile)
    {
        if (selectedTile != null)
        {
            selectedTile.SetPlayerSpawn(true);
        }

        selectedTile = tile;
        selectedTile.SetSelectedVisual();

        playerUnit.gameObject.SetActive(true);

        if (!playerUnit.TryAssignToTile(selectedTile))
        {
            Debug.LogError(
                $"Failed to deploy player on tile: " +
                $"{selectedTile.GridX}, {selectedTile.GridY}",
                playerUnit);

            startFightButton.interactable = false;
            return;
        }

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
}