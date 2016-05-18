namespace bandview
{
	using System;

	using OpenTK;
	using OpenTK.Graphics;
	using OpenTK.Graphics.ES11;
	using OpenTK.Platform.Android;
	using Android.Util;
	using Android.Content;

	class GlView : AndroidGameView
	{
		float[] cube = {
			-0.5f, 0.5f, 0.5f, // vertex[0]
			0.5f, 0.5f, 0.5f, // vertex[1]
			0.5f, -0.5f, 0.5f, // vertex[2]
			-0.5f, -0.5f, 0.5f, // vertex[3]
			-0.5f, 0.5f, -0.5f, // vertex[4]
			0.5f, 0.5f, -0.5f, // vertex[5]
			0.5f, -0.5f, -0.5f, // vertex[6]
			-0.5f, -0.5f, -0.5f, // vertex[7]
		};

		byte[] triangles = {
			1, 0, 2, // front
			3, 2, 0,
			6, 4, 5, // back
			4, 6, 7,
			4, 7, 0, // left
			7, 3, 0,
			1, 2, 5, // right
			2, 6, 5,
			0, 1, 5, // top
			0, 5, 4,
			2, 3, 6, // bottom
			3, 7, 6,
		};

		float[] cubeColors = {
			1.0f, 0.0f, 0.0f, 1.0f,
			0.0f, 1.0f, 0.0f, 1.0f,
			0.0f, 0.0f, 1.0f, 1.0f,
			0.0f, 1.0f, 1.0f, 1.0f,
			1.0f, 0.0f, 0.0f, 1.0f,
			0.0f, 1.0f, 0.0f, 1.0f,
			0.0f, 0.0f, 1.0f, 1.0f,
			0.0f, 1.0f, 1.0f, 1.0f,
		};

		float[] _angles = { 0, 0, 0 };
		int _viewportWidth, _viewportHeight;

		public double SpeedX { get; set; } = 0;
		public double SpeedY { get; set; } = 0;
		public double SpeedZ { get; set; } = 0;

		public GlView(Context context, IAttributeSet attrs) :
			base(context, attrs)
		{
		}

		public GlView(IntPtr handle, Android.Runtime.JniHandleOwnership transfer)
			: base(handle, transfer)
		{
		}

		protected override void CreateFrameBuffer()
		{
			ContextRenderingApi = GLVersion.ES1;

			try
			{
				base.CreateFrameBuffer();
				return;
			}
			catch (Exception) { }

			try
			{
				GraphicsMode = new AndroidGraphicsMode(0, 0, 0, 0, 0, false);
				base.CreateFrameBuffer();
				return;
			}
			catch (Exception) { }

			throw new Exception("Can't load egl, aborting");
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			UpdateFrame += delegate (object sender, FrameEventArgs args)
			{
				_angles[0] = (float) SpeedX;//+= (float)(SpeedX * args.Time);
				_angles[1] = (float) SpeedY; //+= (float)(SpeedY * args.Time);
				_angles[2] = (float) SpeedZ;//+= (float)(SpeedZ * args.Time);
			};

			RenderFrame += (s, args) => RenderCube();

			GL.Enable(All.CullFace);
			GL.ShadeModel(All.Smooth);
			GL.Hint(All.PerspectiveCorrectionHint, All.Nicest);

			Run(30);
		}

		protected override void OnResize(EventArgs e)
		{
			_viewportWidth = Width;
			_viewportHeight = Height;
		}

		void RenderCube()
		{
			GL.Viewport(0, 0, _viewportWidth, _viewportHeight);
			GL.MatrixMode(All.Projection);
			GL.LoadIdentity();

			if (_viewportWidth > _viewportHeight)
			{
				GL.Ortho(-1.5f, 1.5f, 1.0f, -1.0f, -1.0f, 1.0f);
			}
			else
			{
				GL.Ortho(-1.0f, 1.0f, -1.5f, 1.5f, -1.0f, 1.0f);
			}

			GL.MatrixMode(All.Modelview);
			GL.LoadIdentity();
			GL.Rotate(_angles[0], 1.0f, 0.0f, 0.0f);
			GL.Rotate(_angles[1], 0.0f, 1.0f, 0.0f);
			GL.Rotate(_angles[2], 0.0f, 1.0f, 0.0f);
			GL.ClearColor(0, 0, 0, 1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit);

			unsafe
			{
				fixed (float* pcube = cube, pcubeColors = cubeColors)
				{
					fixed (byte* ptriangles = triangles)
					{
						GL.VertexPointer(3, All.Float, 0, new IntPtr(pcube));
						GL.EnableClientState(All.VertexArray);
						GL.ColorPointer(4, All.Float, 0, new IntPtr(pcubeColors));
						GL.EnableClientState(All.ColorArray);
						GL.DrawElements(All.Triangles, 36, All.UnsignedByte, new IntPtr(ptriangles));
						GL.Finish();
					}
				}
			}

			SwapBuffers();
		}
	}
}

