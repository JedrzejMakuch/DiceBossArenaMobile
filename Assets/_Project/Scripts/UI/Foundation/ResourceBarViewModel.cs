using UnityEngine;

namespace DiceBossArena.UI
{
    public readonly struct ResourceBarViewModel
    {
        public ResourceBarViewModel(
            int current,
            int maximum,
            string label = null,
            ResourceBarVisualState visualState =
                ResourceBarVisualState.Default)
        {
            Maximum = Mathf.Max(0, maximum);
            Current = Mathf.Clamp(
                current,
                0,
                Maximum);

            Label = label ?? string.Empty;
            VisualState = visualState;
        }

        public int Current { get; }

        public int Maximum { get; }

        public string Label { get; }

        public ResourceBarVisualState VisualState { get; }

        public float FillAmount =>
            Maximum > 0
                ? (float)Current / Maximum
                : 0f;

        public string ValueText =>
            $"{Current} / {Maximum}";
    }
}