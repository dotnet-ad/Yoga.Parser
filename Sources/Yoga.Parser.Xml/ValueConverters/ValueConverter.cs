namespace Yoga.Parser
{
	using System;

	public abstract class ValueConverter<TSource,TDestination> : IValueConverter<TSource, TDestination>
	{
		public Type SourceType => typeof(TSource);

		public Type DestinationType => typeof(TDestination);

		public (bool success, object output) TryParse(object value)
		{
			if(value is TDestination)
			{
				return (true, value);
			}

			if (value == null)
			{
				return (true, default(TDestination));
			}

			return this.TryParse((TSource)value);
		}

		public abstract (bool success, TDestination output) TryParse(TSource input);
	}
}
