namespace DiceBossArena.UI
{
    public interface IBindableUIView<in TModel> :
        IUIView
    {
        bool IsBound { get; }

        void Bind(TModel model);
        void Unbind();
    }
}