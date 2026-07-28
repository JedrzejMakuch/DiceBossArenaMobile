using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DiceBossArena.UI
{
    public sealed class ResourceBarUIView :
        EventDrivenUIView<ResourceBarViewModel>
    {
        [Header("References")]
        [SerializeField]
        private Image fillImage;

        [SerializeField]
        private TMP_Text labelText;

        [SerializeField]
        private TMP_Text valueText;

        [Header("Visual States")]
        [SerializeField]
        private Color defaultColor = Color.white;

        [SerializeField]
        private Color warningColor = Color.yellow;

        [SerializeField]
        private Color criticalColor = Color.red;

        [SerializeField]
        private Color depletedColor = Color.gray;

        [SerializeField]
        private Color enhancedColor = Color.cyan;

        protected override void Render(
            ResourceBarViewModel viewModel)
        {
            fillImage.fillAmount =
                viewModel.FillAmount;

            fillImage.color =
                GetColor(viewModel.VisualState);

            labelText.text =
                viewModel.Label;

            valueText.text =
                viewModel.ValueText;
        }

        private Color GetColor(
            ResourceBarVisualState visualState)
        {
            return visualState switch
            {
                ResourceBarVisualState.Warning =>
                    warningColor,

                ResourceBarVisualState.Critical =>
                    criticalColor,

                ResourceBarVisualState.Depleted =>
                    depletedColor,

                ResourceBarVisualState.Enhanced =>
                    enhancedColor,

                _ =>
                    defaultColor
            };
        }
    }
}