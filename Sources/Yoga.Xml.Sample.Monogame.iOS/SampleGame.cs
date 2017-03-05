namespace Yoga.Parser.Sample.Monogame.iOS
{
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	public class SampleGame : Game
	{
		#region Native view

		public class YogaView : IView
		{
			private Color backgroundColor;

			public string Id { get; set; }

			public Sample.Rectangle Frame { get; set; }

			Sample.Color IView.Background 
			{
				get => new Sample.Color(this.backgroundColor.R, this.backgroundColor.G, this.backgroundColor.B);
				set => this.backgroundColor = new Color(value.R, value.G, value.B); 
			}

			public void Draw(SpriteBatch batch)
			{
				var frame = new Rectangle((int)this.Frame.X, (int)this.Frame.Y, (int)this.Frame.Width, (int)this.Frame.Height);
				batch.FillRectangle(frame, backgroundColor);
			}
		}

		#endregion

		#region Fields

		GraphicsDeviceManager graphics;

		SpriteBatch spriteBatch;

		LayoutView layout;

		#endregion

		public SampleGame()
		{
			this.IsMouseVisible = true;
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			graphics.IsFullScreen = true;
		}

		private Vector2 screenSize;

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			var newScreenSize = new Vector2(this.GraphicsDevice.PresentationParameters.BackBufferWidth, this.GraphicsDevice.PresentationParameters.BackBufferHeight);
			if(screenSize != newScreenSize)
			{
				this.screenSize = newScreenSize;
				this.Layout();
			}
		}

		protected override void Initialize()
		{
			base.Initialize();

			this.layout = new LayoutView(2f);
			this.layout.RegisterRenderer<IView, YogaView>();
			this.layout.LoadFromAssembly("Sample.xml");
			this.Layout();
		}

		private void Layout() => this.layout.Frame = new Sample.Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
		}

		protected override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
	
			GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin();

			foreach (var view in this.layout.Views)
			{
				var mgview = view as YogaView;
				mgview.Draw(spriteBatch);
			}

			spriteBatch.End();
		}
	}
}
