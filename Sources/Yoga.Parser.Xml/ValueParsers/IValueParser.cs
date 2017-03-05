using System;
namespace Yoga.Parser
{
	public interface IValueParser
	{
		Type Type { get; }

		bool TryParse(string value, out object output);
	}

	public interface IValueParser<T> : IValueParser
	{
		bool TryParse(string value, out T output);
	}
}
