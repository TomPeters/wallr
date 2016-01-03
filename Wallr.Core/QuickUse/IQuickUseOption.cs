namespace Wallr.Core.QuickUse
{
    public interface IQuickUseOption
    {
        string Label { get; }
        void SelectOption();
    }
}