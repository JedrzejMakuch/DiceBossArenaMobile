using UnityEngine;

namespace DiceBossArena.UI
{
    public sealed class UIRootView : MonoBehaviour
    {
        [Header("UI Layers")]
        [SerializeField]
        private Canvas staticNavigationLayer;

        [SerializeField]
        private Canvas dynamicHudLayer;

        [SerializeField]
        private Canvas screenLayer;

        [SerializeField]
        private Canvas modalLayer;

        [SerializeField]
        private Canvas tooltipLayer;

        [SerializeField]
        private Canvas transitionLayer;

        public Canvas StaticNavigationLayer =>
            staticNavigationLayer;

        public Canvas DynamicHudLayer =>
            dynamicHudLayer;

        public Canvas ScreenLayer =>
            screenLayer;

        public Canvas ModalLayer =>
            modalLayer;

        public Canvas TooltipLayer =>
            tooltipLayer;

        public Canvas TransitionLayer =>
            transitionLayer;
    }
}