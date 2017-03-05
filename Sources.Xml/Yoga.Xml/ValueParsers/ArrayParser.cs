namespace Yoga.Xml
{
	using System.Collections.Generic;

	public class ArrayParser<TItem> : ValueParser<TItem[]>
	{
		public ArrayParser(IValueParser<TItem> valueParser)
		{
			this.valueParser = valueParser;
		}

		private IValueParser<TItem> valueParser;

		public override bool TryParse(string value, out TItem[] output)
		{
			var list = new List<TItem>();
			var splits = value.Split(',');
			foreach (var item in splits)
			{
				TItem v;
				if (!valueParser.TryParse(item, out v))
				{
					output = null;
					return false;
				}
				list.Add(v);
			}

			output = list.ToArray();

			return true;
		}
	}
}
