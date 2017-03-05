namespace Yoga.Parser
{
	using System;
	using Facebook.Yoga;

	public static class YogaValueConverters
	{
		public static IValueConverter<string,YogaValue> FromString(float density) => new RelayValueConverter<string, YogaValue>((input) => 
		{
			var text = input.ToLower().Trim();

			if (text == "auto")
			{
				return (true, YogaValue.Auto());
			}

			if (text.EndsWith("%", StringComparison.Ordinal))
			{
				return (true, YogaValue.Percent(float.Parse(text.Substring(0, text.Length - 1))));
			}

			if (text.EndsWith("pt", StringComparison.Ordinal))
			{
				input = text.Substring(0, text.Length - 2);
			}

			float number;
			if (float.TryParse(text, out number))
			{	
				return (true, YogaValue.Point(number * density));
			}
				        
			return (false, YogaValue.Undefined());
		});

		public static IValueConverter<float, YogaValue> FromFloat(float density) => new RelayValueConverter<float, YogaValue>((input) =>
		{
			 return (true, YogaValue.Point(input * density));
		});

		public static IValueConverter<int, YogaValue> FromInt(float density) => new RelayValueConverter<int, YogaValue>((input) =>
		{
			return (true, YogaValue.Point(input * density));
		});
	}
}
