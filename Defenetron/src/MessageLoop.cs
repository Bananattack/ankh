﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;

namespace Defenetron
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Message
    {
        public IntPtr hWnd;
        public uint msg;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public Point p;
    }

    class MessageLoop
    {
        [SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PeekMessage(
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

        void ApplicationIdle(object sender, EventArgs e)
        {
            while(AppStillIdle)
            {
                loop();
            }
        }
    }
}
