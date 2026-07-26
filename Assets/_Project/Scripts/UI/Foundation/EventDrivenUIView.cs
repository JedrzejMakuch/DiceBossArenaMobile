namespace DiceBossArena.UI
{
    public abstract class EventDrivenUIView<TViewModel> :
        BindableUIView<IUIViewModelSource<TViewModel>>
    {
        private readonly UIEventBinding<TViewModel>
            eventBinding = new();

        protected override void OnBind(
            IUIViewModelSource<TViewModel> source)
        {
            eventBinding.Bind(
                source,
                Render);
        }

        protected override void OnShow()
        {
            eventBinding.Activate();
        }

        protected override void OnHide()
        {
            eventBinding.Deactivate();
        }

        protected override void OnUnbind()
        {
            eventBinding.Unbind();
        }

        protected abstract void Render(
            TViewModel viewModel);
    }
}