using System;

namespace DiceBossArena.UI
{
    public sealed class ResourceBarViewModelSource :
        IUIViewModelSource<ResourceBarViewModel>
    {
        public ResourceBarViewModelSource(
            ResourceBarViewModel initialViewModel)
        {
            Current = initialViewModel;
        }

        public ResourceBarViewModel Current { get; private set; }

        public event Action<ResourceBarViewModel> Changed;

        public void Set(
            ResourceBarViewModel viewModel)
        {
            Current = viewModel;
            Changed?.Invoke(Current);
        }
    }
}