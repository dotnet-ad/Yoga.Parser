namespace Yoga.Parser
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	public abstract class Renderer<T> : IRenderer
	{
		public Renderer(string name = null)
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


		#region Reflection properties

		protected Dictionary<string, PropertyInfo> nodePropertySetters = new Dictionary<string, PropertyInfo>();

		protected void RegisterTypeProperty(string name)
		{
			nodePropertySetters[name] = this.Type.GetRuntimeProperty(name);
		}

		protected void RegisterTypeProperties(params string[] names)
		{
			foreach (var name in names)
			{
				RegisterTypeProperty(name);
			}
		}

		protected void RegisterAllTypeProperties(params string[] excludes)
		{
			var properties = this.Type.GetRuntimeProperties()
			                     .Where(x => x.CanWrite && !excludes.Contains(x.Name))
			                     .Select(x => x.Name);
			this.RegisterTypeProperties(properties.ToArray());
		}

		protected bool TryUpdateTypeProperty(T instance, string name, INode node)
		{
			PropertyInfo property;
			if (nodePropertySetters.TryGetValue(name, out property))
			{
				var v = node.Get(name, property.PropertyType);
				property.SetValue(instance, v);
				return true;
			}

			return false;
		}

		protected void UpdateTypeProperties(T instance, INode node)
		{
			foreach (var property in this.nodePropertySetters.Values)
			{
				if(node.Has(property.Name))
				{
					var v = node.Get(property.Name, property.PropertyType);
					property.SetValue(instance, v);
				}
			}
		}

		#endregion

		public string Name { get; }

		public Type Type { get; }

		protected virtual bool TryRenderProperty(T instance, string name, INode node) 
		{
			return TryUpdateTypeProperty(instance, name, node);
		}

		public virtual T Render(INode node) 
		{
			var result = (T)Activator.CreateInstance(typeof(T));

			foreach (var prop in node.Properties)
			{
				TryRenderProperty(result, prop, node);
			}

			return result;
		} 

		object IRenderer.Render(INode node) => this.Render(node);
	}

	public class XmlRenderer<T, TImpl> : Renderer<T> where TImpl : T
	{
		public override T Render(INode node)
		{
			var result = (T)Activator.CreateInstance(typeof(TImpl));

			foreach (var prop in node.Properties)
			{
				TryRenderProperty(result, prop, node);
			}

			return result;
		}
	}
}
