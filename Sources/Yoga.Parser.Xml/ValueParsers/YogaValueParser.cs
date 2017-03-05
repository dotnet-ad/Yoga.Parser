namespace Yoga.Parser
{
	using System;
	using Facebook.Yoga;

	public class YogaValueParser : ValueParser<YogaValue>
	{
		public YogaValueParser(float density = 1)
		{
			this.Density = density;
		}

		public float Density { get; }

		public override bool TryParse(string value, out YogaValue output)
		{
			value = value.ToLower().Trim();

			if (value == "auto")
			{
				output = YogaValue.Auto();
				return true;
			}

			if (value.EndsWith("%", StringComparison.Ordinal))
			{
				output = YogaValue.Percent(float.Parse(value.Substring(0, value.Length - 1)));
				return true;
			}

			if (value.EndsWith("pt", StringComparison.Ordinal))
			{
				value = value.Substring(0, value.Length - 2);
			}

			float number;
			if (float.TryParse(value, out number))
			{
				output = YogaValue.Point(number * this.Density);
				return true;
			}

			output = YogaValue.Undefined();
			return false;
		}
	}
}
