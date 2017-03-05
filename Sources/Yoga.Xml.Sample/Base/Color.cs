namespace Yoga.Parser.Sample
{
	public struct Color
	{
		public Color(byte r, byte g, byte b)
		{
			this.R = r;
			this.G = g;
			this.B = b;
		}

		public byte R { get; set; }

		public byte G { get; set; }

		public byte B { get; set; }
	}
}
