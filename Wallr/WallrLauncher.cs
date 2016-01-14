using Wallr.Core;
using Wallr.Platform;

namespace Wallr
{
    public static class WallrLauncher
    {
        public static void LaunchWallr(IPlatform platform)
        {
            new WallrApplication().Setup(platform);
        }
    }
}
