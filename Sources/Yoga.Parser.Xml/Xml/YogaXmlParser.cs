﻿namespace Yoga.Parser
{
	using System.IO;
	using System.Xml.Linq;

	public class YogaXmlParser : YogaParser
	{
		public override void Write(Stream stream, INode node)
		{
			var xml = new XmlNode(this,node);
			xml.Xml.Save(stream);
		}

		protected override INode ReadNode(Stream stream)
		{
			var root = XElement.Load(stream);
			return new XmlNode(this, root);
		}
	}
}
