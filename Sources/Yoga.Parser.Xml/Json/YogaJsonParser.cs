﻿namespace Yoga.Parser
{
	using System.IO;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;

	public class YogaJsonParser : YogaParser
	{
		protected override INode ReadNode(Stream stream)
		{
			using (var textReader = new StreamReader(stream))
			using (var reader = new JsonTextReader(textReader))
			{
				var root = JObject.Load(reader);
				return new JsonNode(this, root);
			}
		}

		public override void Write(Stream stream, INode node)
		{
			using (var textWriter = new StreamWriter(stream))
			using (var writer = new JsonTextWriter(textWriter))
			{
				var json = new JsonNode(this, node);
				json.Json.WriteTo(writer);
			}
		}
	}
}
