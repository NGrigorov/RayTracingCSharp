using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Template {

class Game
{
	public Surface screen;

		private int CURRENT_FPS;
		private int framesPassed;
		private DateTime previousTime;
		private int timePassed;
		private bool running;
		public float deltaTime;

	public void Init()
	{
		CURRENT_FPS = 0;
		framesPassed = 0;
		timePassed = 1;
		running = true;
		screen.Clear( 0x2222ff );
		previousTime = DateTime.Now;
		deltaTime = 0.05f;
	}
	public void Tick()
	{
		if(this.running)
        {
			this.CalcualteFPS();
			//if(!this.screen.hasRendered)
			screen.Trace();
			screen.Print("FPS: " + CURRENT_FPS, 2, 2, 0xffffff);
		}
		
	}
	public void Render()
	{
		// render stuff over the backbuffer (OpenGL, sprites)
	}

	public void Kill()
	{
		this.running = false;
	}

	public void CalcualteFPS()
    {
		if(DateTime.Now.Second - previousTime.Second > 0)
        {
			timePassed++;
			previousTime = DateTime.Now;
		}
		//deltaTime = this.screen.Utility.Clamp(0.01f,0.5f, DateTime.Now.Second - previousTime.Second / 1000f); //TODO FIX PROPER DELTA
		framesPassed++;
		CURRENT_FPS = framesPassed / timePassed;
	}

}

} // namespace Template
