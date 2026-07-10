using UnityEngine;

public class BoardTile : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private string tileName;
    [SerializeField] private TileEventData assignedEvent;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform standPoint;

    public int Index => index;
    public string TileName => tileName;
    public TileEventData AssignedEvent => assignedEvent;
    public bool HasEvent => assignedEvent != null;
    public Transform StandPoint => standPoint;

    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    public Vector3 GetStandPosition()
    {
        if (standPoint != null)
            return standPoint.position;

        return transform.position;
    }

    public void SetEvent(TileEventData eventData)
    {
        assignedEvent = eventData;
        RefreshVisuals();
    }

    public void ClearEvent()
    {
        assignedEvent = null;
        RefreshVisuals();
    }

    private void RefreshVisuals()
    {
        if (spriteRenderer == null)
            return;

        if (assignedEvent == null)
        {
            spriteRenderer.color = Color.white;
            return;
        }

        spriteRenderer.color = assignedEvent.TileColor;
    }
}