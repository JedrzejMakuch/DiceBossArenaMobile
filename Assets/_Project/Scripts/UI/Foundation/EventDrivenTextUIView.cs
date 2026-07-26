using TMPro;
using UnityEngine;

namespace DiceBossArena.UI
{
    public sealed class EventDrivenTextUIView :
        EventDrivenUIView<string>
    {
        [SerializeField]
        private TMP_Text text;

        protected override void Render(
            string viewModel)
        {
            if (text == null)
            {
                Debug.LogError(
                    $"{nameof(EventDrivenTextUIView)} requires " +
                    $"a {nameof(TMP_Text)} reference.",
                    this);

                return;
            }

            text.text =
                viewModel ?? string.Empty;
        }
    }
}