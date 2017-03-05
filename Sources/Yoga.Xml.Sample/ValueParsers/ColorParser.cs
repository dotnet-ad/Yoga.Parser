namespace Yoga.Parser.Sample
{
	public static class ColorConverters
	{
		public static IValueConverter<string, Color> FromString() => new RelayValueConverter<string, Color>((input) =>
		 {
			 switch (input)
			 {
				 case "Gray":
					 return (true, new Color(246, 247, 249));

				 case "Green":
					 return (true, new Color(151, 220, 207));

				 case "Black":
					 return (true, new Color(48, 56, 70));

				 default:
					 return (true, new Color(255, 255, 255));
			 }
		 });
	}
}
