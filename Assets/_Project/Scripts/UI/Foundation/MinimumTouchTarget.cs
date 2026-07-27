using UnityEngine;
using UnityEngine.UI;

namespace DiceBossArena.UI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(LayoutElement))]
    public sealed class MinimumTouchTarget :
        MonoBehaviour
    {
        private const float DefaultMinimumSize = 96f;

        [SerializeField]
        [Min(1f)]
        private float minimumWidth =
            DefaultMinimumSize;

        [SerializeField]
        [Min(1f)]
        private float minimumHeight =
            DefaultMinimumSize;

        private LayoutElement layoutElement;

        private void Awake()
        {
            ApplyMinimumSize();
        }

        private void OnValidate()
        {
            ApplyMinimumSize();
        }

        private void ApplyMinimumSize()
        {
            if (layoutElement == null)
            {
                layoutElement =
                    GetComponent<LayoutElement>();
            }

            layoutElement.minWidth =
                minimumWidth;

            layoutElement.minHeight =
                minimumHeight;
        }
    }
}