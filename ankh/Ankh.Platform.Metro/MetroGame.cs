namespace Ankh.Platform.Metro
{
    public class MetroGame : Ankh.Game
    {
        static MetroGame()
        {
            MetroPlatformApi.Initialize();
        }
        public MetroGame(GraphicsDeviceBase device)
            : base(device)
        {
        }
    }
}