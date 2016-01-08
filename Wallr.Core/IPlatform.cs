using System.Collections.Generic;
using Wallr.Core.QuickUse;

namespace Wallr.Core
{
    public interface IPlatform
    {
        void SetupQuickUseControl(IReadOnlyList<IQuickUseOption> quickUseOptions);
        string ApplicationDataFolderPath { get; }
    }
}