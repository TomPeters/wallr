namespace Wallr.QuickUse
{
    public interface IQuickUseOption
    {
        string Label { get; }
        void SelectOption();
    }
}