using System;

namespace DiceBossArena.UI
{
    public sealed class ReusableUIPanelModel
    {
        private readonly Action closeRequested;

        public ReusableUIPanelModel(
            string title,
            string value,
            Action closeRequested)
        {
            Title = title ??
                throw new ArgumentNullException(
                    nameof(title));

            Value = value ??
                throw new ArgumentNullException(
                    nameof(value));

            this.closeRequested =
                closeRequested ??
                throw new ArgumentNullException(
                    nameof(closeRequested));
        }

        public string Title { get; }
        public string Value { get; }

        public void RequestClose()
        {
            closeRequested.Invoke();
        }
    }
}