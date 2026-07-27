using UnityEngine;

namespace DiceBossArena.UI
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class SafeAreaFitter :
        MonoBehaviour
    {
        private RectTransform rectTransform;
        private Rect lastSafeArea;
        private int lastScreenWidth;
        private int lastScreenHeight;

        private void Awake()
        {
            rectTransform =
                GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            ApplySafeArea();
        }

        private void Update()
        {
            if (Screen.safeArea == lastSafeArea &&
                Screen.width == lastScreenWidth &&
                Screen.height == lastScreenHeight)
            {
                return;
            }

            ApplySafeArea();
        }

        private void ApplySafeArea()
        {
            if (Screen.width <= 0 ||
                Screen.height <= 0)
            {
                return;
            }

            Rect safeArea =
                Screen.safeArea;

            Vector2 anchorMin =
                safeArea.position;

            Vector2 anchorMax =
                safeArea.position +
                safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            rectTransform.anchorMin =
                anchorMin;

            rectTransform.anchorMax =
                anchorMax;

            rectTransform.offsetMin =
                Vector2.zero;

            rectTransform.offsetMax =
                Vector2.zero;

            lastSafeArea =
                safeArea;

            lastScreenWidth =
                Screen.width;

            lastScreenHeight =
                Screen.height;
        }
    }
}