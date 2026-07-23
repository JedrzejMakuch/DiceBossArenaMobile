namespace DiceBossArena.UI
{
    public interface IUIView
    {
        bool IsVisible { get; }

        void Show();
        void Hide();
    }
}