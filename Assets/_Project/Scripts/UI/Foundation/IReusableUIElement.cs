namespace DiceBossArena.UI
{
    public interface IReusableUIElement
    {
        void PrepareForUse();

        void ResetForPool();
    }
}