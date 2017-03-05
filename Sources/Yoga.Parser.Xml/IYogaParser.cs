namespace Yoga.Parser
{
	using System;
	using System.IO;
	using Facebook.Yoga;

	public interface IYogaParser
	{
		YogaNode Read(Stream stream);

		void Write(Stream stream, INode node);

		object ConvertValue(object value, Type type);

		object ConvertValue(object v, params Type[] destinations);

		void RegisterConverter<TSource,TDestination>(IValueConverter<TSource,TDestination> converter);

		IValueConverter<TSource, TDestination> GetConverter<TSource, TDestination>();

		void RegisterNodeRenderer(INodeRenderer renderer);
	}

	public static class IYogaParserExtensions
	{
		public static T ConvertValue<T>(this IYogaParser parser, object value) => (T)parser.ConvertValue(value, typeof(T));
	}
}
