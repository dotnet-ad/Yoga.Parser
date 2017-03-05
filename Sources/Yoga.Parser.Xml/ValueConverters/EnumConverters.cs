namespace Yoga.Parser
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public static class EnumConverters<TEnum>
	{
		static EnumConverters()
		{
			values = Enum.GetNames(typeof(TEnum)).ToDictionary(x => x.ToLower(), x => (TEnum)Enum.Parse(typeof(TEnum), x), StringComparer.OrdinalIgnoreCase);
		}

		private static readonly Dictionary<string, TEnum> values;

		public static (bool success, TEnum output) FromString(string input)
		{
			TEnum result;
			return (values.TryGetValue(input.ToLowerInvariant().Trim(), out result), result);
		}

		public static (bool success, TEnum output) FromInt(int input)
		{
			return (true, (TEnum)Convert.ChangeType(input, typeof(TEnum)));
		}
	}
}
