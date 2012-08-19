﻿using System;
using Ankh;
using Game = Defenetron.Game;

namespace Defenetron
{
	public class DefenetronGame : Game
	{
		public DefenetronGame(GraphicsDeviceBase d)
			: base(d)
		{ }

		ITexture tex;

		public override void Render()
		{
			var d = renderEngine.device as Ankh.DX9.GraphicsDevice;
			if (d != null)
			{
				TestDX9(d);
			}
			renderEngine.Render();
		}

		private void TestDX9(Ankh.DX9.GraphicsDevice dx9)
		{
			dx9.ClearBackBuffer(renderEngine.device.CreateColor(123.0f / 255.0f, 160.0f / 255.0f, 183.0f / 255.0f, 1));
			dx9.Device.BeginScene();
			using (var sb = new SharpDX.Direct3D9.Sprite(dx9.Device))
			{
				sb.Begin(SharpDX.Direct3D9.SpriteFlags.AlphaBlend);
				sb.Draw((tex as Ankh.DX9.GraphicsDevice.DX9Texture).tex, new SharpDX.Rectangle(0, 0, 16, 16), new SharpDX.Vector3(0, 0, 0), new SharpDX.Vector3(0, 0, 0), SharpDX.Colors.White);
				sb.End();
			}
			dx9.Device.EndScene();
		}

		public override void Step()
		{
		}

		public override void Setup()
		{
			tex = renderEngine.device.CreateTexture("content/32bpp.png");
		}
	}
}
