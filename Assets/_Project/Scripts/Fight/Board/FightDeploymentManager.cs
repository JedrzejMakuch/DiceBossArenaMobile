using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FightDeploymentManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightArenaGenerator arenaGenerator;
    [SerializeField] private FightStateManager fightStateManager;
    [SerializeField] private GameObject playerPlaceholder;
    [SerializeField] private Button startFightButton;

    [Header("Deployment Settings")]
    [SerializeField] private int playerSpawnCount = 4;

    private readonly List<FightGridTile> playerSpawnTiles = new();
    private FightGridTile selectedTile;
    private bool isDeploymentLocked = false;

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

        if (playerPlaceholder == null)
        {
            Debug.LogError(
                "FightDeploymentManager: Player Placeholder is not assigned.",
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

        playerPlaceholder.SetActive(false);
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
            selectedTile.SetOccupied(false);
        }

        selectedTile = tile;
        selectedTile.SetSelectedVisual();
        selectedTile.SetOccupied(true);

        playerPlaceholder.transform.position = selectedTile.GetStandPosition();
        playerPlaceholder.SetActive(true);

        startFightButton.interactable = true;

        if (fightStateManager != null)
        {
            fightStateManager.SetReadyToStart();
        }

        Debug.Log($"Player deployed on tile: {selectedTile.GridX}, {selectedTile.GridY}");
    }

    public void LockDeployment()
    {
        isDeploymentLocked = true;

        foreach (FightGridTile tile in playerSpawnTiles)
        {
            if (tile == null)
            {
                continue;
            }

            if (tile == selectedTile)
            {
                tile.SetSelectedVisual();
            }
            else
            {
                tile.SetPlayerSpawn(false);
            }
        }

        Debug.Log("Deployment locked.");
    }
}