﻿using System;
using Ankh;

namespace Defenetron
{
	public class Game
	{
		protected RenderHandler renderEngine;

		public Game(GraphicsDeviceBase device)
		{
			renderEngine = new RenderHandler(device);
			this.Setup();
		}

		public virtual void Render()
		{
			renderEngine.Render();
		}

		public virtual void Step()
		{
		}

		// to implement
		public virtual void Setup()
		{

		}
	}
}
