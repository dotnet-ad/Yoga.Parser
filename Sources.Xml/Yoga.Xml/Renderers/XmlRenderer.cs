namespace Yoga.Xml
{
	using System;
	using System.Xml.Linq;

	public abstract class XmlRenderer<T> : IXmlRenderer
	{
		public XmlRenderer(string name = null)
		{
			if (name == null)
			{
				var typename = typeof(T).Name;
				if (typename.StartsWith("I", StringComparison.Ordinal))
					typename = typename.Substring(1);
				name = typename;
			}

			this.Type = typeof(T);

			this.Name = name;

		}

		public string Name { get; }

		public Type Type { get; }

		public virtual T Render(XElement node)
		{
			return (T)Activator.CreateInstance(typeof(T));
		}

		object IXmlRenderer.Render(XElement node) => this.Render(node);
	}

	public class XmlRenderer<T, TImpl> : XmlRenderer<T> where TImpl : T
	{
		public override T Render(XElement node)
		{
			return (T)Activator.CreateInstance(typeof(TImpl));
		}
	}
}
