namespace Yoga.Parser
{
	using System;

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

		public string Name { get; }

		public Type Type { get; }

		public virtual T Render(INode node) => (T)Activator.CreateInstance(typeof(T));

		object IRenderer.Render(INode node) => this.Render(node);
	}

	public class XmlRenderer<T, TImpl> : Renderer<T> where TImpl : T
	{
		public override T Render(INode node)
		{
			return (T)Activator.CreateInstance(typeof(TImpl));
		}
	}
}
