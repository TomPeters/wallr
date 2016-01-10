using Serilog;
using Wallr.Core;

namespace Wallr.Windows10
{
    class Program
    {
        static void Main(string[] args)
        {
            Windows10Platform windows10Platform = new Windows10Platform(new WallrApplicationContext());
            new WallrApplication().Setup(windows10Platform);
            Log.Logger.Information("Application started");
            windows10Platform.Start();
        }
    }
}
