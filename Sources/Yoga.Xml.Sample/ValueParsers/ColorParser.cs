namespace Yoga.Parser.Sample
{
	using System;

	public class ColorParser : ValueParser<Color>
	{
		public override bool TryParse(string value, out Color output)
		{
			switch (value)
			{
				case "Gray":
					output = new Color(246, 247, 249);
					break;

				case "Green":
					output = new Color(151, 220, 207);
					break;

				case "Black":
					output = new Color(48, 56, 70);
					break;

				default:
					output = new Color(255, 255, 255);
					break;
			}

			return true;
		}
	}
}
