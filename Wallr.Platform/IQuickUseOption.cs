using System.Threading.Tasks;

namespace Wallr.Platform
{
    public interface IQuickUseOption
    {
        string Label { get; }
        Task SelectOption();
    }
}