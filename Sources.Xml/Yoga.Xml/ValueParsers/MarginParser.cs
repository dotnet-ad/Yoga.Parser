namespace Yoga.Xml
{
	using Facebook.Yoga;
	using System.Collections.Generic;

	public class MarginParser : ValueParser<YogaValue[]>
	{
		public MarginParser()
		{
			this.valueParser = new YogaValueParser();
		}

		private YogaValueParser valueParser;

		public override bool TryParse(string value, out YogaValue[] output)
		{
			var list = new List<YogaValue>();
			var splits = value.Split(',');
			foreach (var item in splits)
			{
				YogaValue v;
				if (!valueParser.TryParse(item, out v))
				{
					output = null;
					return false;
				}
				list.Add(v);
			}

			switch (list.Count)
			{
				case 1: 
					output = new[] { list[0], list[0], list[0], list[0] }; 
					break;
				case 2:
				case 3: 
					output = new[] { list[0], list[1], list[0], list[1] }; 
					break;
				default: 
					output = new[] { list[0], list[1], list[2], list[3] }; 
					break;
			}

			return true;
		}
	}
}
