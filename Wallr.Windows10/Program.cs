namespace Wallr.Windows10
{
    class Program
    {
        static void Main(string[] args)
        {
            Windows10Platform windows10Platform = new Windows10Platform(new WallrApplicationContext());
            var closeWallr = WallrLauncher.LaunchWallr(windows10Platform).GetAwaiter().GetResult();
            windows10Platform.Start(closeWallr);
        }
    }
}
