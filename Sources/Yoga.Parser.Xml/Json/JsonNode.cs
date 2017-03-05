﻿namespace Yoga.Parser
{
	using System.Collections.Generic;
	using System.Linq;
	using Newtonsoft.Json.Linq;

	public class JsonNode : INode
	{
		public const string TypePropertyName = "Type";

		public const string ChildrenPropertyName = "Children";

		public const string Type = nameof(Type);

		public JsonNode(IYogaParser parser, INode node)
		{
			this.Name = node.Name;
			this.Properties = new Dictionary<string, object>(node.Properties);

			this.Json = new JObject();
			this.Json[TypePropertyName] = this.Name;

			foreach (var prop in this.Properties)
			{
				var value = parser.ConvertValue(prop.Value, typeof(bool), typeof(int), typeof(float), typeof(string));
				this.Json[prop.Key] = JToken.FromObject(value);
			}

			var children = new List<INode>();
			var jarray = new JArray();
			foreach (var child in node.Children)
			{
				var childNode = new JsonNode(parser, child);
				children.Add(childNode);
				jarray.Add(childNode.Json);
			}
			this.Children = children.ToArray();
			this.Json[ChildrenPropertyName] = jarray;
		}

		public JsonNode(IYogaParser parser, JObject element)
		{
			this.Json = element;
			this.parser = parser;
			this.Name = this.Json[TypePropertyName].Value<string>();
			this.Properties = this.Json.Properties()
										  .Where(x => x.Name != TypePropertyName && x.Name != ChildrenPropertyName)
										  .ToDictionary( x => x.Name, GetPropertyValue);
			var children = ((JArray)this.Json[ChildrenPropertyName]);
			this.Children = children?.Select(x => new JsonNode(this.parser, (JObject)x)) ?? (IEnumerable<INode>)new INode[0];
		}

		private IYogaParser parser;

		public JObject Json { get; }

		public string Name { get; }

		public IEnumerable<INode> Children { get; }

		public IDictionary<string, object> Properties { get; }

		private object GetPropertyValue(JProperty x)
		{
			var v = x.Value;
			switch (v.Type)
			{
				case JTokenType.Integer: return v.Value<int>();
				case JTokenType.Float: return v.Value<float>();
				case JTokenType.String: return v.Value<string>();
				case JTokenType.Boolean: return v.Value<bool>();
				default: return null;
			}
		}
	}
}
