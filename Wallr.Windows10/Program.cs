namespace Wallr.Windows10
{
    class Program
    {
        static void Main(string[] args)
        {
            Windows10Platform windows10Platform = new Windows10Platform(new WallrApplicationContext());
            windows10Platform.Start(WallrLauncher.LaunchWallr(windows10Platform));
        }
    }
}
