namespace Yoga.Parser
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class EnumParser<TEnum> : ValueParser<TEnum>
	{
		static EnumParser()
		{
			values = Enum.GetNames(typeof(TEnum)).ToDictionary(x => x.ToLower(), x => (TEnum)Enum.Parse(typeof(TEnum), x), StringComparer.OrdinalIgnoreCase);
		}

		private static readonly Dictionary<string, TEnum> values;

		public override bool TryParse(string value, out TEnum output) => values.TryGetValue(value.ToLowerInvariant().Trim(), out output);
	}
}
