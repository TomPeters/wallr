using System.Collections.Generic;
using Wallr.Core.QuickUse;

namespace Wallr.Core
{
    public class WallrApplication
    {
        public void Setup(IPlatform platform)
        {
            platform.SetupQuickUseControl(new List<IQuickUseOption>());
        }
    }
}
