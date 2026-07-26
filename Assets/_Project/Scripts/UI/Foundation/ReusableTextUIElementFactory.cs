using UnityEngine;

namespace DiceBossArena.UI
{
    public sealed class ReusableTextUIElementFactory :
        MonoBehaviour
    {
        [SerializeField]
        private ReusableTextUIElement prefab;

        [SerializeField]
        private Transform container;

        public ReusableTextUIElement Create()
        {
            if (prefab == null)
            {
                Debug.LogError(
                    $"{nameof(ReusableTextUIElementFactory)} requires " +
                    $"a {nameof(ReusableTextUIElement)} prefab.",
                    this);

                return null;
            }

            Transform parent =
                container != null
                    ? container
                    : transform;

            return Instantiate(
                prefab,
                parent);
        }
    }
}