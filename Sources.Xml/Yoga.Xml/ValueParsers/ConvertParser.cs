namespace Yoga.Xml
{
	using System;

	public class ConvertParser<T> : ValueParser<T>
	{
		public override bool TryParse(string value, out T output)
		{
			try
			{
				output = (T)Convert.ChangeType(value, typeof(T));
				return true;
			}
			catch (Exception ex)
			{
				output = default(T);
				return false;
			}
		}
	}
}
