namespace Yoga.Parser.Sample
{
	public struct Rectangle
	{
		public Rectangle(float x, float y, float width, float height)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
		}

		public float X { get; set; }

		public float Y { get; set; }

		public float Width { get; set; }

		public float Height { get; set; }

		public static bool operator ==(Rectangle a, Rectangle b)
		{
			return ((a.X == b.X) && (a.Y == b.Y) && (a.Width == b.Width) && (a.Height == b.Height));
		}

		public static bool operator !=(Rectangle a, Rectangle b) => !(a == b);

		public bool Equals(Rectangle other) => this == other;

		public override int GetHashCode()
		{
			return ((int)this.X ^ (int)this.Y ^ (int)this.Width ^ (int)this.Height);
		}

		public override bool Equals(object obj)
		{
			return (obj is Rectangle) ? this == ((Rectangle)obj) : false;
		}
	}
}
