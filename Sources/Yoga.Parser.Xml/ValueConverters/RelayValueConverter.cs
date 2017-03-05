namespace Yoga.Parser
{
	using System;

	public class RelayValueConverter<TSource, TDestination> : ValueConverter<TSource, TDestination>
	{
		public RelayValueConverter(Func<TSource, (bool, TDestination)> conversion)
		{
			this.conversion = conversion;
		}

		private Func<TSource, (bool, TDestination)> conversion;

		public override (bool success, TDestination output) TryParse(TSource input) => conversion(input);
	}
}
