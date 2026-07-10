using System;
using TMPro;
using UnityEngine;

public class TileEventResolver : MonoBehaviour
{
    [SerializeField] private PlayerBoardMover playerMover;
    [SerializeField] private TMP_Text eventText;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerStatsUI playerStatsUI;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private FloatingTextSpawner floatingTextSpawner;

    private void OnEnable()
    {
        if (playerMover != null)
        {
            playerMover.OnMovementFinished += HandleMovementFinished;
        }
    }

    private void OnDisable()
    {
        if (playerMover != null)
        {
            playerMover.OnMovementFinished -= HandleMovementFinished;
        }
    }

    private void HandleMovementFinished(BoardTile landedTile)
    {
        if (landedTile == null)
        {
            Debug.LogWarning("Landed tile is null.");
            return;
        }

        TileEventData tileEvent = landedTile.AssignedEvent;

        if (tileEvent == null)
        {
            SetEventText($"Tile {landedTile.Index}: no event.");
            return;
        }

        string message = $"Tile {landedTile.Index}: {tileEvent.EventName} - {tileEvent.Description}";
        SetEventText(message);
        ApplyTileEffect(tileEvent);
    }

    private void ApplyTileEffect(TileEventData eventData)
    {
        if (eventData == null)
            return;

        string floatingMessage = string.Empty;

        switch (eventData.TileType)
        {
            case BoardTileType.Normal:
                break;

            case BoardTileType.Heal:
                playerStats.Heal(2);
                floatingMessage = "+2 HP";
                break;

            case BoardTileType.Trap:
                playerStats.TakeDamage(2);
                floatingMessage = "-2 HP";
                break;

            case BoardTileType.Fight:
                playerStats.TakeDamage(1);
                floatingMessage = "-1 HP";
                break;

            case BoardTileType.Upgrade:
                playerStats.AddGold(5);
                floatingMessage = "+5 Gold";
                break;

            case BoardTileType.Event:
                playerStats.AddGold(2);
                floatingMessage = "+2 Gold";
                break;
        }

        playerStatsUI.Refresh();

        if (!string.IsNullOrEmpty(floatingMessage))
        {
            floatingTextSpawner.Show(floatingMessage, playerStats.transform.position);
        }

        if (playerStats.IsDead)
        {
            gameManager.TriggerGameOver();
        }
    }

    private void SetEventText(string message)
    {
        Debug.Log(message);

        if (eventText != null)
        {
            eventText.text = message;
        }
    }
}