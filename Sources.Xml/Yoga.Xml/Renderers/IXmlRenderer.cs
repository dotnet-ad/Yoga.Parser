namespace Yoga.Xml
{
	using System;
	using System.Xml.Linq;

	public interface IXmlRenderer
	{
		string Name { get; }

		Type Type { get; }

		object Render(XElement node);
	}
}