namespace Yoga.Xml
{
	using System.IO;
	using Facebook.Yoga;

	public static class YogaParserExtensions
	{
		public static YogaNode Parse(this YogaParser parser, string xml)
		{
			using(var stream = new MemoryStream())
			{
				using(StreamWriter writer = new StreamWriter(stream))
				{
					writer.Write(xml);
					writer.Flush();
					stream.Position = 0;
					return parser.Parse(stream);
				}
			}
		}
	}
}
