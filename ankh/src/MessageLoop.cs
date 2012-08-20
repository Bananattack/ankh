using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;

namespace Ankh
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Message
    {
        public IntPtr hWnd;
        public uint msg;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public Point p;
    }

    public class MessageLoop
    {
        [SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PeekMessage(
            out Message msg, 
            IntPtr hWnd, 
            uint msgFilterMin, 
            uint msgFilterMax,
            uint removeMsg);

        static bool AppStillIdle
        {
            get
            {
                Message msg;
                return !PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
            }
        }

        private Action loop;

        public void Run(Form form, Action loop)
        {
            this.loop = loop;
            Application.Idle += ApplicationIdle;
            Application.Run(form);
        }

	public void Run(Ankh.Game game)
	{
		Run(game.Device.Form, game.DoRender);
	}

        void ApplicationIdle(object sender, EventArgs e)
        {
            while(AppStillIdle)
            {
                loop();
            }
        }
    }
}
