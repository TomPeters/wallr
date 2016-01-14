namespace Wallr.Platform
{
    public interface IQuickUseOption
    {
        string Label { get; }
        void SelectOption();
    }
}