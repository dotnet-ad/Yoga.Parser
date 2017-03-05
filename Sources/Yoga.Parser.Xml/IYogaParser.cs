namespace Yoga.Parser
{
	using System;
	using System.IO;
	using Facebook.Yoga;

	public interface IYogaParser
	{
		YogaNode Parse(Stream stream);

		object ParseValue(string value, Type type);
	}

	public static class IYogaParserExtensions
	{
		public static T ParseValue<T>(this IYogaParser parser, string value) => (T)parser.ParseValue(value, typeof(T));
	}
}
