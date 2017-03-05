namespace Yoga.Parser
{
	using System.IO;
	using System.Xml.Linq;
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
			// Renderers
			this.Register(nodeRenderer);

			// Value parsers
			this.RegisterValueParser(new ConvertParser<float>());
			this.RegisterValueParser(new ConvertParser<int>());
			this.RegisterValueParser(new ConvertParser<string>());
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

		#endregion

		#region Stream parsing

		public YogaNode Parse(Stream stream)
		{
			var root = XElement.Load(stream);
			return Parse(root as XElement);

		}

		#endregion

		#region Renderers

		private YogaNodeRenderer nodeRenderer = new YogaNodeRenderer();

		private List<IRenderer> renderers = new List<IRenderer>();

		public void Register(IRenderer renderer)
		{
			var existing = renderers.FindIndex(x => x.Name == renderer.Name);
			if (existing >= 0) renderers.RemoveAt(existing);
			this.renderers.Add(renderer);
		}

		public void Register<TView, TImpl>() where TImpl : TView
		{
			this.Register(new XmlRenderer<TView, TImpl>());
		}

		private IRenderer GetRenderer(string name) => this.renderers.FirstOrDefault(x => x.Name == name);

		#endregion

		#region Value Parsing

		private Dictionary<Type, IValueParser> valueParsers = new Dictionary<Type, IValueParser>();

		public void RegisterValueParser<T>(IValueParser<T> parser)
		{
			this.valueParsers[typeof(T)] = parser;
		}

		public object ParseValue(string value, Type type)
		{
			if(string.IsNullOrEmpty(value))
			{
				if (type.GetTypeInfo().IsValueType)
				{
					return Activator.CreateInstance(type);
				}
				return null;
			}

			object result;

			IValueParser parser;
			if (this.valueParsers.TryGetValue(type, out parser) && parser.TryParse(value, out result))
			{
				return result;
			}

			throw new InvalidOperationException($"Failed to parse {type} value '{value}'");
		}

		#endregion

		#region Node parsing

		private YogaNode Parse(XElement node)
		{
			var inode = new XmlNode(this, node);

			var result = nodeRenderer.Render(inode);

			if (renderers == null)
				throw new InvalidOperationException($"No renderer found for '{node.Name.LocalName}'");

			var dataRenderer = this.GetRenderer(node.Name.LocalName);
			result.Data = dataRenderer.Render(inode);

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
