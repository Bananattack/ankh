﻿using System;
using Ankh;

namespace Defenetron.Metro
{
    public class DefenetronGame : Ankh.Platform.Metro.MetroGame
    {
        public DefenetronGame(GraphicsDeviceBase d)
            : base(d)
        { }

        ITexture tex;

        public override void Render()
        {

        }

  
        public override void Step()
        {
        }

        public override void Setup()
        {
            //tex = Device.CreateTexture("content/32bpp.png");
            tex = Device.CreateTexture("Assets/32bpp.png");
        }
    }
}
