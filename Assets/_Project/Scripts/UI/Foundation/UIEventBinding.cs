using System;

namespace DiceBossArena.UI
{
    public sealed class UIEventBinding<TViewModel>
    {
        private IUIViewModelSource<TViewModel> source;
        private Action<TViewModel> render;

        public bool IsBound =>
            source != null;

        public bool IsActive { get; private set; }

        public void Bind(
            IUIViewModelSource<TViewModel> newSource,
            Action<TViewModel> renderAction)
        {
            if (newSource == null)
            {
                throw new ArgumentNullException(
                    nameof(newSource));
            }

            if (renderAction == null)
            {
                throw new ArgumentNullException(
                    nameof(renderAction));
            }

            if (IsBound)
            {
                throw new InvalidOperationException(
                    "UI event binding is already bound.");
            }

            source = newSource;
            render = renderAction;

            render.Invoke(source.Current);
        }

        public void Activate()
        {
            EnsureBound();

            if (IsActive)
            {
                return;
            }

            source.Changed += HandleChanged;
            IsActive = true;
        }

        public void Deactivate()
        {
            if (!IsActive)
            {
                return;
            }

            source.Changed -= HandleChanged;
            IsActive = false;
        }

        public void Unbind()
        {
            EnsureBound();

            Deactivate();

            source = null;
            render = null;
        }

        private void HandleChanged(
            TViewModel viewModel)
        {
            render.Invoke(viewModel);
        }

        private void EnsureBound()
        {
            if (!IsBound)
            {
                throw new InvalidOperationException(
                    "UI event binding is not bound.");
            }
        }
    }
}