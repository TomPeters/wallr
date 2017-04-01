using System.Threading.Tasks;
using Wallr.Platform;
using Wallr.UI;

namespace Wallr
{
    public class OpenConfigPageOption : IQuickUseOption
    {
        private readonly INavigation _navigation;
        private readonly IWallrUiServer _wallrUiServer;

        public OpenConfigPageOption(INavigation navigation, IWallrUiServer wallrUiServer)
        {
            _navigation = navigation;
            _wallrUiServer = wallrUiServer;
        }

        public string Label => "Configure";

        public Task SelectOption()
        {
            return _navigation.NavigateToUrl(_wallrUiServer.Url);
        }
    }
}