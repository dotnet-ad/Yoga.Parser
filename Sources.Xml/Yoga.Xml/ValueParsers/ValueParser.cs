namespace Yoga.Xml
{
	using System;

	public abstract class ValueParser<T> : IValueParser<T>
	{
		public Type Type => typeof(T);

		public bool TryParse(string value, out object output)
		{
			T t;
			var result = this.TryParse(value, out t);
			output = t;
			return result;
		}

		public abstract bool TryParse(string value, out T output);
	}
}
