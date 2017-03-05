namespace Yoga.Parser.Sample.Monogame.iOS
{
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

		/// <summary>
		/// </summary>
		public static class Helpers
		{
			#region Private Members

			private static Texture2D pixel;

			#endregion


			#region Private Methods

			private static void CreateThePixel(SpriteBatch spriteBatch)
			{
				pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
				pixel.SetData(new[] { Color.White });
			}

			#endregion


			#region FillRectangle

			/// <summary>
			/// Draws a filled rectangle
			/// </summary>
			/// <param name="spriteBatch">The destination drawing surface</param>
			/// <param name="rect">The rectangle to draw</param>
			/// <param name="color">The color to draw the rectangle in</param>
			public static void FillRectangle(this SpriteBatch spriteBatch, Rectangle rect, Color color)
			{
				if (pixel == null)
				{
					CreateThePixel(spriteBatch);
				}

				// Simply use the function already there
				spriteBatch.Draw(pixel, rect, color);
			}


			/// <summary>
			/// Draws a filled rectangle
			/// </summary>
			/// <param name="spriteBatch">The destination drawing surface</param>
			/// <param name="rect">The rectangle to draw</param>
			/// <param name="color">The color to draw the rectangle in</param>
			/// <param name="angle">The angle in radians to draw the rectangle at</param>
			public static void FillRectangle(this SpriteBatch spriteBatch, Rectangle rect, Color color, float angle)
			{
				if (pixel == null)
				{
					CreateThePixel(spriteBatch);
				}

				spriteBatch.Draw(pixel, rect, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
			}


			/// <summary>
			/// Draws a filled rectangle
			/// </summary>
			/// <param name="spriteBatch">The destination drawing surface</param>
			/// <param name="location">Where to draw</param>
			/// <param name="size">The size of the rectangle</param>
			/// <param name="color">The color to draw the rectangle in</param>
			public static void FillRectangle(this SpriteBatch spriteBatch, Vector2 location, Vector2 size, Color color)
			{
				FillRectangle(spriteBatch, location, size, color, 0.0f);
			}


			/// <summary>
			/// Draws a filled rectangle
			/// </summary>
			/// <param name="spriteBatch">The destination drawing surface</param>
			/// <param name="location">Where to draw</param>
			/// <param name="size">The size of the rectangle</param>
			/// <param name="angle">The angle in radians to draw the rectangle at</param>
			/// <param name="color">The color to draw the rectangle in</param>
			public static void FillRectangle(this SpriteBatch spriteBatch, Vector2 location, Vector2 size, Color color, float angle)
			{
				if (pixel == null)
				{
					CreateThePixel(spriteBatch);
				}

				// stretch the pixel between the two vectors
				spriteBatch.Draw(pixel,
								 location,
								 null,
								 color,
								 angle,
								 Vector2.Zero,
								 size,
								 SpriteEffects.None,
								 0);
			}

			#endregion

		}
}
