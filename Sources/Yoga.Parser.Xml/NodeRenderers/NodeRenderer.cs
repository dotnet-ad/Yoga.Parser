namespace Yoga.Parser
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	public class NodeRenderer<T> : INodeRenderer
	{
		public NodeRenderer(IYogaParser parser, string name = null)
		{
			this.parser = parser;

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

		private IYogaParser parser;

		#region Property renderers

		private List<IPropertyRenderer> propertyRenderers = new List<IPropertyRenderer>();

		public void RegisterPropertyRenderer(IPropertyRenderer renderer)
		{
			var existing = this.propertyRenderers.FindIndex(x => x.Name == renderer.Name);
			if (existing >= 0) propertyRenderers.RemoveAt(existing);
			propertyRenderers.Add(renderer);
		}

		protected void RenderProperty(object instance, string name, object value)
		{
			var renderer = this.propertyRenderers.FirstOrDefault(x => x.Name == name);
			if(renderer != null)
			{
				var v = parser.ConvertValue(value, renderer.Type);
				renderer.Render(instance, v);
			}
		}

		#endregion

		public string Name { get; }

		public Type Type { get; }

		#region Rendering

		public virtual T Render(INode node)
		{
			var result = (T)Activator.CreateInstance(typeof(T));

			foreach (var prop in node.Properties)
			{
				this.RenderProperty(result, prop.Key, prop.Value);	
			}

			return result;
		}

		object INodeRenderer.Render(INode node) => this.Render(node);

		#endregion
	}

	public class NodeRenderer<T, TImpl> : NodeRenderer<T> where TImpl : T
	{
		public NodeRenderer(IYogaParser parser, string name = null) : base(parser, name)
		{
		}

		public override T Render(INode node)
		{
			var result = (T)Activator.CreateInstance(typeof(TImpl));

			foreach (var prop in node.Properties)
			{
				this.RenderProperty(result, prop.Key, prop.Value);
			}

			return result;
		}
	}
}
