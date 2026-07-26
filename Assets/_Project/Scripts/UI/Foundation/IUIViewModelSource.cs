using System;

namespace DiceBossArena.UI
{
    public interface IUIViewModelSource<TViewModel>
    {
        TViewModel Current { get; }

        event Action<TViewModel> Changed;
    }
}