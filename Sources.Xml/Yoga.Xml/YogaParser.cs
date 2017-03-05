namespace Yoga.Xml
{
	using System.IO;
	using System.Xml.Linq;
	using System.Xml;
	using Facebook.Yoga;
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Reflection;

	public class YogaParser : IYogaParser
	{
		#region Constructors

		public YogaParser()
		{
			this.RegisterDefaultValueParsers();
		}

		#endregion

		#region Stream parsing

		public YogaNode Parse(Stream stream)
		{
			var root = XElement.Load(stream);
			return Parse(root as XElement);

		}

		#endregion

		#region Renderers

		private List<IXmlRenderer> renderers = new List<IXmlRenderer>();

		public void Register(IXmlRenderer renderer)
		{
			var existing = renderers.FindIndex(x => x.Name == renderer.Name);
			if (existing >= 0) renderers.RemoveAt(existing);
			this.renderers.Add(renderer);
		}

		public void Register<TView, TImpl>() where TImpl : TView
		{
			this.Register(new XmlRenderer<TView, TImpl>());
		}

		private IXmlRenderer GetRenderer(string name) => this.renderers.FirstOrDefault(x => x.Name == name);

		#endregion

		#region Property caching

		private Dictionary<string, PropertyInfo> nodePropertySetters = typeof(YogaNode).GetRuntimeProperties()
																					 .ToDictionary(x => x.Name, x => x);

		#endregion

		#region Value Parsing

		private Dictionary<Type, IValueParser> valueParsers = new Dictionary<Type, IValueParser>();

		private void RegisterDefaultValueParsers()
		{
			this.RegisterValueParser(new ConvertParser<float>());
			this.RegisterValueParser(new ConvertParser<int>());
			this.RegisterValueParser(new YogaValueParser());
			this.RegisterValueParser(new MarginParser());
			this.RegisterValueParser(new EnumParser<YogaUnit>());
			this.RegisterValueParser(new EnumParser<YogaEdge>());
			this.RegisterValueParser(new EnumParser<YogaWrap>());
			this.RegisterValueParser(new EnumParser<YogaAlign>());
			this.RegisterValueParser(new EnumParser<YogaJustify>());
			this.RegisterValueParser(new EnumParser<YogaOverflow>());
			this.RegisterValueParser(new EnumParser<YogaDisplay>());
			this.RegisterValueParser(new EnumParser<YogaDimension>());
			this.RegisterValueParser(new EnumParser<YogaDirection>());
			this.RegisterValueParser(new EnumParser<YogaPositionType>());
			this.RegisterValueParser(new EnumParser<YogaFlexDirection>());
		}

		public void RegisterValueParser<T>(IValueParser<T> parser)
		{
			this.valueParsers[typeof(T)] = parser;
		}

		private object ParseValue(XAttribute s, Type type)
		{
			object result;
			var parser = this.valueParsers[type];
			if (parser.TryParse(s.Value, out result))
			{
				return result;
			}

			throw new InvalidOperationException($"Failed to parse {type} value '{s.Value}' from attribute '{s.Name}'");
		}

		private T ParseValue<T>(XAttribute s)
		{
			T result;
			var parser = this.valueParsers[typeof(T)] as IValueParser<T>;
			if (parser.TryParse(s.Value, out result))
			{
				return result;
			}

			throw new InvalidOperationException($"Failed to parse {typeof(T)} value '{s.Value}' from attribute '{s.Name}'");
		}

		#endregion

		#region Attribute parsing

		private void ParseAttribute(YogaNode node, XAttribute attribute)
		{
			PropertyInfo property;
			if (attribute.Name.LocalName == nameof(node.Margin))
			{
				var margin = this.ParseValue<YogaValue[]>(attribute);
				node.MarginLeft = margin[0];
				node.MarginTop = margin[1];
				node.MarginRight = margin[2];
				node.MarginBottom = margin[3];
			}
			else if (attribute.Name.LocalName == nameof(node.Padding))
			{
				var padding = this.ParseValue<YogaValue[]>(attribute);
				node.PaddingLeft = padding[0];
				node.PaddingTop = padding[1];
				node.PaddingRight = padding[2];
				node.PaddingBottom = padding[3];
			}
			else if(nodePropertySetters.TryGetValue(attribute.Name.LocalName, out property))
			{
				var v = ParseValue(attribute, property.PropertyType);
				property.SetValue(node, v);
			}
		}

		#endregion

		#region Node parsing

		private YogaNode Parse(XElement node)
		{
			var result = new YogaNode();
			foreach (var attr in node.Attributes())
			{
				ParseAttribute(result, attr);
			}

			var renderer = this.renderers.FirstOrDefault(x => x.Name == node.Name.LocalName);

			if (renderers == null)
				throw new InvalidOperationException($"No renderer found for '{node.Name.LocalName}'");

			result.Data = renderer.Render(node);

			foreach (var item in node.Elements())
			{
				var child = Parse(item);
				result.Insert(result.Count, child);
			}

			return result;
		}

		#endregion
	}
}
