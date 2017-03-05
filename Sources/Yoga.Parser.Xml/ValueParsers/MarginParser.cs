namespace Yoga.Parser
{
	using Facebook.Yoga;

	public class MarginParser : ArrayParser<YogaValue>
	{
		public MarginParser(): base(new YogaValueParser())
		{
		}

		public override bool TryParse(string value, out YogaValue[] output)
		{
			if(base.TryParse(value, out output))
			{
				switch (output.Length)
				{
					case 1:
						output = new[] { output[0], output[0], output[0], output[0] };
						break;
					case 2:
					case 3:
						output = new[] { output[0], output[1], output[0], output[1] };
						break;
					default:
						output = new[] { output[0], output[1], output[2], output[3] };
						break;
				}

				return true;
			}

			return false;
		}
	}
}
