namespace Yoga.Parser
{
	using System;
	using System.IO;
	using Facebook.Yoga;

	public static class YogaParserExtensions
	{
		public static YogaNode Parse(this IYogaParser parser, string xml)
		{
			using (var stream = new MemoryStream())
			{
				using (StreamWriter writer = new StreamWriter(stream))
				{
					writer.Write(xml);
					writer.Flush();
					stream.Position = 0;
					return parser.Read(stream);
				}
			}
		}

		public static void RegisterCastConverter<TSource, TDestination>(this IYogaParser parser)
		{
			parser.RegisterConverter(BaseConverters.FromCast<TSource, TDestination>());
		}

		public static void RegisterEnumConverter<TEnum>(this IYogaParser parser)
		{
			parser.RegisterRelayConverter<string, TEnum>(EnumConverters<TEnum>.FromString);
			parser.RegisterRelayConverter<int, TEnum>(EnumConverters<TEnum>.FromInt);
		}

		public static void RegisterRelayConverter<TSource,TDestination>(this IYogaParser parser, Func<TSource, (bool, TDestination)> conversion)
		{
			parser.RegisterConverter(new RelayValueConverter<TSource, TDestination>(conversion));

		}
	}
}