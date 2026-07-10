using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Renderer textRenderer;

    [SerializeField] private string sortingLayerName = "Characters";
    [SerializeField] private int orderInLayer = 100;

    [SerializeField] private float lifetime = 1f;
    [SerializeField] private float moveSpeed = 0.6f;

    private float timer;

    private void Awake()
    {
        if (textRenderer == null)
            textRenderer = GetComponent<Renderer>();

        if (textRenderer != null)
        {
            textRenderer.sortingLayerName = sortingLayerName;
            textRenderer.sortingOrder = orderInLayer;
        }
    }

    public void Initialize(string message)
    {
        text.text = message;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}