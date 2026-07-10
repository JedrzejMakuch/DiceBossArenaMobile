using UnityEngine;

[CreateAssetMenu(fileName = "NewTileEvent", menuName = "Dice Boss Arena/Tile Event")]
public class TileEventData : ScriptableObject
{
    [SerializeField] private string eventName;
    [SerializeField] private BoardTileType tileType;
    [SerializeField] private Color tileColor = Color.gray;
    [SerializeField, TextArea] private string description;

    public string EventName => eventName;
    public BoardTileType TileType => tileType;
    public Color TileColor => tileColor;
    public string Description => description;
}
