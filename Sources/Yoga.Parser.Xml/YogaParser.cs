namespace Yoga.Parser
{
	using System.IO;
	using Facebook.Yoga;
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Reflection;

	public abstract class YogaParser : IYogaParser
	{
		#region Constructors

		public YogaParser(float density = 1)
		{
			this.nodeRenderer = new YogaNodeRenderer(this);

			// Renderers
			this.RegisterNodeRenderer(nodeRenderer);

			// Array converter
			this.RegisterConverter(ArrayConverters.Split());

			// Base value converter
			this.RegisterCastConverter<int, float>();
			this.RegisterCastConverter<float, int>();
			this.RegisterCastConverter<string, int>();
			this.RegisterCastConverter<string, float>();
			this.RegisterCastConverter<string, bool>();

			// Enum value converters
			this.RegisterEnumConverter<YogaUnit>();
			this.RegisterEnumConverter<YogaEdge>();
			this.RegisterEnumConverter<YogaWrap>();
			this.RegisterEnumConverter<YogaAlign>();
			this.RegisterEnumConverter<YogaJustify>();
			this.RegisterEnumConverter<YogaOverflow>();
			this.RegisterEnumConverter<YogaDisplay>();
			this.RegisterEnumConverter<YogaDimension>();
			this.RegisterEnumConverter<YogaPositionType>();
			this.RegisterEnumConverter<YogaFlexDirection>();

			// Yoga value converters
			this.RegisterConverter(YogaValueConverters.FromFloat(density));
			this.RegisterConverter(YogaValueConverters.FromInt(density));
			this.RegisterConverter(YogaValueConverters.FromString(density));
		}

		#endregion

		#region Stream parsing

		protected abstract INode ReadNode(Stream stream);

		public YogaNode Read(Stream stream)
		{
			var inode = ReadNode(stream);
			return ParseNode(inode);
		}

		public YogaNode ParseNode(INode node)
		{
			var result = nodeRenderer.Render(node);

			var dataRenderer = this.GetRenderer(node.Name);
			result.Data = dataRenderer.Render(node);

			foreach (var item in node.Children)
			{
				var child = ParseNode(item);
				result.Insert(result.Count, child);
			}

			return result;
		}

		#endregion

		#region Renderers

		protected YogaNodeRenderer nodeRenderer;

		private List<INodeRenderer> renderers = new List<INodeRenderer>();

		public void RegisterNodeRenderer(INodeRenderer renderer)
		{
			var existing = renderers.FindIndex(x => x.Name == renderer.Name);
			if (existing >= 0) renderers.RemoveAt(existing);
			this.renderers.Add(renderer);
		}

		public void Register<TView, TImpl>() where TImpl : TView
		{
			this.RegisterNodeRenderer(new NodeRenderer<TView, TImpl>(this));
		}

		protected INodeRenderer GetRenderer(string name) 
		{
			var result = this.renderers.FirstOrDefault(x => x.Name == name);
			if (result == null)
				throw new InvalidOperationException($"No renderer found for '{name}'");
			return result;
		}

		#endregion

		#region Value conversion

		private List<IValueConverter> valueConverters = new List<IValueConverter>();

		public void RegisterConverter<TSource,TDestination>(IValueConverter<TSource, TDestination> converter)
		{
			var existing = this.valueConverters.FindIndex(x => x.SourceType == typeof(TSource) && x.DestinationType == typeof(TDestination));
			if (existing >= 0) valueConverters.RemoveAt(existing);
			this.valueConverters.Add(converter);

			if(!typeof(TDestination).IsArray)
			{
				if(!this.valueConverters.Any(x => x.SourceType == typeof(TDestination) && x.DestinationType == typeof(TDestination[])))
				{
					this.valueConverters.Add(ArrayConverters.FromSingleItem<TDestination>());
				}
			}
		}


		private IValueConverter FindConverter(Type tsource, Type tdestination)
		{
			var destinations = this.valueConverters.Where(x => x.DestinationType == tdestination);
			if (!destinations.Any())
				return null;

			var converter = destinations.FirstOrDefault(x => x.SourceType == tsource);
			if (converter != null)
				return converter;

			foreach (var item in destinations)
			{
				converter = FindConverter(tsource, item.SourceType);
				if (converter != null)
					return new ChainValueConverter(converter, item);
			}

			return null;
		}

		public IValueConverter GetConverter(Type tsource, Type tdestination)
		{
			var result = FindConverter(tsource, tdestination);
			if (result == null)
				throw new InvalidOperationException($"No converter found from '{tsource}' to '{tdestination}'");
			return result;
		}

		public IValueConverter<TSource, TDestination> GetConverter<TSource,TDestination>()
		{
			return (IValueConverter<TSource, TDestination>)this.GetConverter(typeof(TSource), typeof(TDestination));
		}

		private (bool success, object result) TryConvertValue(object v, Type destination)
		{
			if (v == null)
			{
				return (true, (destination.GetTypeInfo().IsValueType) ? Activator.CreateInstance(destination) : null);
			}

			if (v.GetType() == destination)
				return (true, v);

			var converter = this.GetConverter(v.GetType(), destination);

			return converter.TryParse(v);
		}

		public object ConvertValue(object v, Type destination)
		{
			var (success, result) = TryConvertValue(v, destination);

			if (!success)
				throw new ArgumentException($"Conversion failed from '{v.GetType()}' to '{destination}' : invalid input {v}");

			return result;
		}

		public object ConvertValue(object v, params Type[] destinations)
		{
			foreach (var type in destinations)
			{
				var (success, result) = TryConvertValue(v, type);
				if (success)
					return result;
			}

			throw new ArgumentException($"Conversion failed from '{v.GetType()}' to one of '{string.Join<Type>(", ", destinations)}' : invalid input {v}");
		}

		public abstract void Write(Stream stream, INode node);

		#endregion
	}
}
