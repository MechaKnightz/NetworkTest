using System;
using EmptyKeys.UserInterface.Mvvm;

namespace TerraStructorClient
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ServiceManager.Instance.AddService<IClipboardService>(new ClipboardService());

            using (var game = new Game1())
                game.Run();
        }
    }
#endif
}
