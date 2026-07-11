using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class FightGridTile : MonoBehaviour, IPointerClickHandler
{
    [Header("Grid Position")]
    [SerializeField] private int gridX;
    [SerializeField] private int gridY;

    [Header("Tile State")]
    [SerializeField] private bool isWalkable = true;
    [SerializeField] private bool isBlocked = false;
    [SerializeField] private bool canPlayerSpawn = false;
    [SerializeField] private bool canEnemySpawn = false;
    [SerializeField] private bool hasObstacle = false;
    [SerializeField] private FightUnit occupyingUnit;

    [Header("References")]
    [SerializeField] private Transform standPoint;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Visuals")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color playerSpawnColor = new Color(0.3f, 1f, 0.4f, 1f);
    [SerializeField] private Color selectedColor = new Color(1f, 0.9f, 0.2f, 1f);
    [SerializeField] private Color blockedColor = new Color(0.35f, 0.35f, 0.35f, 1f);
    [SerializeField] private Color movementRangeColor = new Color(0.25f, 0.65f, 1f, 1f);

    public event Action<FightGridTile> Clicked;

    public int GridX => gridX;
    public int GridY => gridY;

    public bool IsWalkable => isWalkable;
    public bool IsBlocked => isBlocked;
    public bool CanPlayerSpawn => canPlayerSpawn;
    public bool CanEnemySpawn => canEnemySpawn;
    public bool HasObstacle => hasObstacle;
    public bool IsOccupied => occupyingUnit != null;
    public FightUnit OccupyingUnit => occupyingUnit;

    public Vector3 GetStandPosition()
    {
        if (standPoint != null)
        {
            return standPoint.position;
        }

        return transform.position;
    }

    public void Initialize(int x, int y)
    {
        gridX = x;
        gridY = y;

        isWalkable = true;
        isBlocked = false;
        canPlayerSpawn = false;
        canEnemySpawn = false;
        hasObstacle = false;
        occupyingUnit = null;

        SetNormalVisual();
    }

    public void SetPlayerSpawn(bool value)
    {
        canPlayerSpawn = value;

        if (value)
        {
            SetPlayerSpawnVisual();
        }
        else
        {
            SetNormalVisual();
        }
    }

    public void SetEnemySpawn(bool value)
    {
        canEnemySpawn = value;
    }

    public bool CanBeOccupiedBy(FightUnit unit)
    {
        if (unit == null)
        {
            return false;
        }

        return isWalkable &&
               !isBlocked &&
               !hasObstacle &&
               (occupyingUnit == null || occupyingUnit == unit);
    }

    public bool TryOccupy(FightUnit unit)
    {
        if (!CanBeOccupiedBy(unit))
        {
            return false;
        }

        occupyingUnit = unit;

        Debug.Log(
            $"Tile ({gridX}, {gridY}) occupied by {unit.UnitName}.");

        return true;
    }

    public bool TryRelease(FightUnit unit)
    {
        if (unit == null || occupyingUnit != unit)
        {
            return false;
        }

        Debug.Log(
            $"Tile ({gridX}, {gridY}) released by {unit.UnitName}.");

        occupyingUnit = null;
        return true;
    }

    public void SetBlocked(bool value)
    {
        isBlocked = value;
        isWalkable = !value;

        if (isBlocked)
        {
            SetBlockedVisual();
        }
        else
        {
            SetNormalVisual();
        }
    }

    public void SetSelectedVisual()
    {
        SetColor(selectedColor);
    }

    public void SetNormalVisual()
    {
        SetColor(normalColor);
    }

    public void SetPlayerSpawnVisual()
    {
        SetColor(playerSpawnColor);
    }

    public void SetBlockedVisual()
    {
        SetColor(blockedColor);
    }

    private void SetColor(Color color)
    {
        if (spriteRenderer == null)
        {
            return;
        }

        spriteRenderer.color = color;
    }

    public void SetMovementRangeVisual()
    {
        SetColor(movementRangeColor);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked fight tile through EventSystem: {gridX}, {gridY}");

        Clicked?.Invoke(this);
    }
}