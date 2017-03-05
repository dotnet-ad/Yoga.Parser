namespace Yoga.Parser
{
	using System.Collections.Generic;
	using System.Xml.Linq;
	using System.Linq;
	using System;

	public class XmlNode : INode
	{
		public XmlNode(IYogaParser parser, XElement element)
		{
			this.element = element;
			this.parser = parser;
		}

		private IYogaParser parser;

		private XElement element;

		public string Name => this.element.Name.LocalName;

		public IEnumerable<string> Properties => this.element.Attributes().Select(x => x.Name.LocalName);

		public object Get(string name, Type type) => parser.ParseValue(this.element.Attribute(name)?.Value, type);

		public bool Has(string name) => this.element.Attributes().Any(x => x.Name.LocalName == name);
	}
}
