﻿namespace Yoga.Parser
{
	using System.Collections.Generic;
	using System.Xml.Linq;
	using System.Linq;

	public class XmlNode : INode
	{
		public XmlNode(IYogaParser parser, INode node)
		{
			this.Name = node.Name;
			this.Properties = new Dictionary<string,object>(node.Properties);

			this.Xml = new XElement(this.Name);
			foreach (var prop in this.Properties)
			{
				var value = parser.ConvertValue<string>(prop.Value);
				this.Xml.SetAttributeValue(prop.Key, value);
			}

			var children = new List<INode>();
			foreach (var child in node.Children)
			{
				var childNode = new XmlNode(parser, child);
				children.Add(childNode);
				this.Xml.Add(childNode.Xml);
			}
			this.Children = children.ToArray();
		}

		public XmlNode(IYogaParser parser, XElement element)
		{
			this.Xml = element;
			this.parser = parser;
			this.Name = this.Xml.Name.LocalName;
			this.Properties = this.Xml.Attributes().ToDictionary(x => x.Name.LocalName, x => (object)x.Value);
			this.Children = this.Xml.Elements().Select(x => new XmlNode(parser, x));
		}

		public XElement Xml { get; }

		private IYogaParser parser;

		public string Name { get; }

		public IEnumerable<INode> Children { get; } 

		public IDictionary<string, object> Properties { get; }
	}
}
