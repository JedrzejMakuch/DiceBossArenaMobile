namespace DiceBossArena.UI
{
    public abstract class UIScreenView<TModel> :
        BindableUIView<TModel>
    {
        protected sealed override void OnShow()
        {
            gameObject.SetActive(true);
            OnScreenShown();
        }

        protected sealed override void OnHide()
        {
            OnScreenHidden();
            gameObject.SetActive(false);
        }

        protected virtual void OnScreenShown()
        {
        }

        protected virtual void OnScreenHidden()
        {
        }
    }
}