﻿using System;
using Ankh;

namespace Ankh
{
	/*
	 * TODO: Implement whatever it is that calls DoRender() and Step()
	 */
	public class Game
	{
		public readonly GraphicsDeviceBase Device;

		public Game(GraphicsDeviceBase device)
		{
			Device = device;
			this.Setup();
		}

		internal void DoRender()
		{
			Device.BeginScene();
			Render();
			Device.EndScene();
			Device.Present();
		}

		public void Run()
		{
		}

		#region Subclass and implement these
		public virtual void Render()
		{
		}

		public virtual void Step()
		{
		}

		public virtual void Setup()
		{
		}
		#endregion
	}
}
