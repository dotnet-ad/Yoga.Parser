namespace Yoga.Parser
{
	using System;

	public interface IValueConverter
	{
		Type SourceType { get; }

		Type DestinationType { get; }

		(bool success, object output) TryParse(object value);
	}

	public interface IValueConverter<TSource,TDestination> : IValueConverter
	{
		(bool success, TDestination output) TryParse(TSource value);
	}
}
