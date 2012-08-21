﻿using System;
using Ankh;

namespace Defenetron
{
	public class DefenetronGame : Ankh.Game
	{
		public DefenetronGame(GraphicsDeviceBase d)
			: base(d)
		{}

		ITexture tex;

		public override void Render()
		{
			var d = Device as Ankh.DX9.GraphicsDevice;
			if (d != null)
			{
				TestDX9(d);
			}
		}

		private void TestDX9(Ankh.DX9.GraphicsDevice device)
		{
			device.ClearBackBuffer(new Color4(123.0f / 255.0f, 160.0f / 255.0f, 183.0f / 255.0f, 1));
			using (var sb = device.CreateSpriteBatch())
			{
				sb.Draw(tex, Vector3.Zero);
				sb.Draw(tex, new Vector3(32, 32, 0));
			}
		}

		public override void Step()
		{
		}

		public override void Setup()
		{
			tex = Device.CreateTexture("content/32bpp.png");
		}
	}
}
