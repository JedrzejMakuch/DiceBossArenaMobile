using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoardMover : MonoBehaviour
{
    [Header("Board")]
    [SerializeField] private BoardGenerator boardGenerator;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float delayBetweenSteps = 0.12f;

    private int currentTileIndex = 0;
    private bool isMoving = false;

    public bool IsMoving => isMoving;
    public int CurrentTileIndex => currentTileIndex;
    private List<BoardTile> tiles;
    public BoardTile CurrentTile => tiles.Count > 0 ? tiles[currentTileIndex] : null;
    public event Action<BoardTile> OnMovementFinished;

    private void Start()
    {
        tiles = boardGenerator.GeneratedTiles;
        ValidateTiles();
        MoveInstantlyToTile(currentTileIndex);
    }

    public void MoveBySteps(int steps)
    {
        if (isMoving)
            return;

        if (tiles.Count == 0)
        {
            Debug.LogWarning("Cannot move. Tiles list is empty.");
            return;
        }

        if (steps <= 0)
        {
            Debug.LogWarning("Cannot move. Steps must be greater than 0.");
            return;
        }

        StartCoroutine(MoveStepByStep(steps));
    }

    private IEnumerator MoveStepByStep(int steps)
    {
        isMoving = true;

        for (int i = 0; i < steps; i++)
        {
            currentTileIndex = GetNextTileIndex();

            Vector3 targetPosition = tiles[currentTileIndex].GetStandPosition();
            targetPosition.z = transform.position.z;

            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    moveSpeed * Time.deltaTime
                );

                yield return null;
            }

            transform.position = targetPosition;

            yield return new WaitForSeconds(delayBetweenSteps);
        }

        isMoving = false;

        BoardTile landedTile = tiles[currentTileIndex];

        Debug.Log($"Stopped on tile: {landedTile.Index} - {landedTile.TileName}");

        OnMovementFinished?.Invoke(landedTile);
    }

    private int GetNextTileIndex()
    {
        int nextIndex = currentTileIndex + 1;

        if (nextIndex >= tiles.Count)
            nextIndex = 0;

        return nextIndex;
    }

    private void MoveInstantlyToTile(int tileIndex)
    {
        if (tiles.Count == 0)
            return;

        Vector3 targetPosition = tiles[tileIndex].GetStandPosition();
        targetPosition.z = transform.position.z;
        transform.position = targetPosition;
    }

    private void ValidateTiles()
    {
        if (tiles.Count == 0)
        {
            Debug.LogWarning("PlayerBoardMover: Tiles list is empty.");
            return;
        }

        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i] == null)
            {
                Debug.LogWarning($"PlayerBoardMover: Tile at index {i} is missing.");
            }
        }
    }
}