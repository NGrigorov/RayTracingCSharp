using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
 
namespace Template
{
	public class OpenTKApp : GameWindow
	{
		static int screenID;
		static Game game;
		protected override void OnLoad( EventArgs e )
		{
			// called upon app init
			GL.ClearColor( Color.Black );
			GL.Enable( EnableCap.Texture2D );
			GL.Hint( HintTarget.PerspectiveCorrectionHint, HintMode.Nicest );
			ClientSize = new Size( 350, 350 );
			game = new Game();
			game.screen = new Surface( Width, Height );
			Sprite.target = game.screen;
			screenID = game.screen.GenTexture();
			game.Init();
		}
		protected override void OnUnload(EventArgs e)
		{
			// called upon app close
			game.Kill();
			GL.DeleteTextures( 1, ref screenID );
		}
		protected override void OnResize(EventArgs e)
		{
			// called upon window resize
			GL.Viewport(0, 0, Width, Height);
			GL.MatrixMode( MatrixMode.Projection );
			GL.LoadIdentity();
			GL.Ortho( -1.0, 1.0, -1.0, 1.0, 0.0, 4.0 );
		}
		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			// called once per frame; app logic
			var keyboard = OpenTK.Input.Keyboard.GetState();
			if (keyboard[OpenTK.Input.Key.Escape]) this.Exit();
			if (keyboard[OpenTK.Input.Key.A]) game.screen.sceneCam.TranslateCamera(new Vector3(1f,0,0) * 0.05f); //change to deltatime when its done
			if (keyboard[OpenTK.Input.Key.D]) game.screen.sceneCam.TranslateCamera(new Vector3(-1f, 0, 0) * 0.05f);
			if (keyboard[OpenTK.Input.Key.W]) game.screen.sceneCam.TranslateCamera(new Vector3(0, 0, -1f) * 0.05f);
			if (keyboard[OpenTK.Input.Key.S]) game.screen.sceneCam.TranslateCamera(new Vector3(0, 0, 1f) * 0.05f);
			//if (keyboard[OpenTK.Input.Key.R]) game.screen.sceneCam.TranslateCamera(new Vector3(0, 1f, 0) * 0.05f);
			//if (keyboard[OpenTK.Input.Key.F]) game.screen.sceneCam.TranslateCamera(new Vector3(0, -1f, 0) * 0.05f);
			//if (keyboard[OpenTK.Input.Key.Left]) game.screen.sceneCam.RotateCameraX(new Vector3(0, -0.05f, 0));
			//if (keyboard[OpenTK.Input.Key.Right]) game.screen.sceneCam.RotateCameraX(new Vector3(0, 0.05f, 0));
			//if (keyboard[OpenTK.Input.Key.Up]) game.screen.sceneCam.RotateCameraY(new Vector3(0.05f, 0, 0));
			//if (keyboard[OpenTK.Input.Key.Down]) game.screen.sceneCam.RotateCameraY(new Vector3(-0.05f, 0, 0));
			//if (keyboard[OpenTK.Input.Key.Plus]) game.screen.sceneCam.ChangeFoV(1);
			//if (keyboard[OpenTK.Input.Key.Minus]) game.screen.sceneCam.ChangeFoV(-1);

		}

		protected override void OnMouseMove(MouseMoveEventArgs e)
        {
			var mouse = e.Position;
			//game.screen.sceneCam.RotateCamera(new Vector3(mouse.X*0.0001f, 0, 0));
            //Console.WriteLine("MouseMoved");
			base.OnMouseMove(e);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			// called once per frame; render
			game.Tick();
			GL.BindTexture( TextureTarget.Texture2D, screenID );
			GL.TexImage2D( TextureTarget.Texture2D, 
						   0, 
						   PixelInternalFormat.Rgba, 
						   game.screen.width, 
						   game.screen.height, 
						   0, 
						   OpenTK.Graphics.OpenGL.PixelFormat.Bgra, 
						   PixelType.UnsignedByte, 
						   game.screen.pixels 
						 );
			GL.Clear( ClearBufferMask.ColorBufferBit );
			GL.MatrixMode( MatrixMode.Modelview );
			GL.LoadIdentity();
			GL.BindTexture( TextureTarget.Texture2D, screenID );
			GL.Begin( PrimitiveType.Quads );
			GL.TexCoord2( 0.0f, 1.0f ); GL.Vertex2( -1.0f, -1.0f );
			GL.TexCoord2( 1.0f, 1.0f ); GL.Vertex2(  1.0f, -1.0f );
			GL.TexCoord2( 1.0f, 0.0f ); GL.Vertex2(  1.0f,  1.0f );
			GL.TexCoord2( 0.0f, 0.0f ); GL.Vertex2( -1.0f,  1.0f );
			GL.End();
			game.Render();
			SwapBuffers();
		}
		[STAThread]
		public static void Main() 
		{ 
			// entry point
			using (OpenTKApp app = new OpenTKApp()) 
			{ 
				app.Run( 30.0, 0.0 ); 
			} 
		}
	}
}