using UnityEngine;

public class FloatingTextSpawner : MonoBehaviour
{
    [SerializeField] private FloatingText floatingTextPrefab;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0.6f, 0f);

    public void Show(string message, Vector3 worldPosition)
    {
        FloatingText floatingText = Instantiate(
            floatingTextPrefab,
            worldPosition + offset,
            Quaternion.identity
        );

        floatingText.Initialize(message);
    }
}