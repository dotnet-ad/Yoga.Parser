namespace Yoga.Parser
{
	using System;
	using System.Reflection;

	public class ChainValueConverter : IValueConverter
	{
		public ChainValueConverter(IValueConverter first, IValueConverter second)
		{
			this.first = first;
			this.second = second;
		}

		private IValueConverter first;

		private IValueConverter second;

		public Type SourceType => first.SourceType;

		public Type DestinationType => second.DestinationType;

		public (bool success, object output) TryParse(object input)
		{
			var (success, inter) = first.TryParse(input);

			if (!success)
			{
				var result = this.DestinationType.GetTypeInfo().IsValueType ? Activator.CreateInstance(this.DestinationType) : null;
				return (false, result);
			}

			return second.TryParse(inter);
		}
	}
}
