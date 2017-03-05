namespace Yoga.Parser
{
	using System;

	public abstract class ValueParser<T> : IValueParser<T>
	{
		public Type Type => typeof(T);

		public bool TryParse(string value, out object output)
		{
			if (string.IsNullOrEmpty(value))
			{
				output = default(T);
			}

			T t;
			var result = this.TryParse(value, out t);
			output = t;
			return result;
		}

		public abstract bool TryParse(string value, out T output);
	}
}
