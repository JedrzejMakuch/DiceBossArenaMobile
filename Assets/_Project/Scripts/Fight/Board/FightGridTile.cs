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
    [SerializeField] private bool isOccupied = false;

    [Header("References")]
    [SerializeField] private Transform standPoint;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Visuals")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color playerSpawnColor = new Color(0.3f, 1f, 0.4f, 1f);
    [SerializeField] private Color selectedColor = new Color(1f, 0.9f, 0.2f, 1f);
    [SerializeField] private Color blockedColor = new Color(0.35f, 0.35f, 0.35f, 1f);

    public event Action<FightGridTile> Clicked;

    public int GridX => gridX;
    public int GridY => gridY;

    public bool IsWalkable => isWalkable;
    public bool IsBlocked => isBlocked;
    public bool CanPlayerSpawn => canPlayerSpawn;
    public bool CanEnemySpawn => canEnemySpawn;
    public bool HasObstacle => hasObstacle;
    public bool IsOccupied => isOccupied;

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
        isOccupied = false;

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

    public void SetOccupied(bool value)
    {
        isOccupied = value;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked fight tile through EventSystem: {gridX}, {gridY}");

        Clicked?.Invoke(this);
    }
}