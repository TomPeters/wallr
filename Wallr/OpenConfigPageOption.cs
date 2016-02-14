using Wallr.Platform;
using Wallr.UI;

namespace Wallr
{
    public class OpenConfigPageOption : IQuickUseOption
    {
        private readonly IPlatform _platform;
        private readonly IWallrUiServer _wallrUiServer;

        public OpenConfigPageOption(IPlatform platform, IWallrUiServer wallrUiServer)
        {
            _platform = platform;
            _wallrUiServer = wallrUiServer;
        }

        public string Label => "Configure";

        public void SelectOption()
        {
            _platform.NavigateToUrl(_wallrUiServer.Url);
        }
    }
}