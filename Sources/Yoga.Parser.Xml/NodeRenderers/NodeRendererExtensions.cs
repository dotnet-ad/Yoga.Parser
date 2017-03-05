namespace Yoga.Parser
{
	using System.Linq;
	using System.Reflection;

	public static class NodeRendererExtensions
	{
		#region Type properties renderers

		public static void RegisterTypePropertyRenderer<T,TProperty>(this NodeRenderer<T> @this, string name, TProperty defaultValue = default(TProperty)) => @this.RegisterPropertyRenderer(new TypePropertyRenderer<T,TProperty>(name, defaultValue));

		public static void RegisterTypePropertyRenderers<T>(this NodeRenderer<T> @this, params string[] names)
		{
			foreach (var property in typeof(T).GetRuntimeProperties().Where(x => x.CanWrite && names.Contains(x.Name)))
			{
				@this.RegisterPropertyRenderer(new TypePropertyRenderer(property));
			}
		}

		public static void RegisterAllTypePropertyRenderers<T>(this NodeRenderer<T> @this, params string[] exclude)
		{
			foreach (var property in typeof(T).GetRuntimeProperties().Where(x => x.CanWrite && !exclude.Contains(x.Name)))
			{
				@this.RegisterPropertyRenderer(new TypePropertyRenderer(property));
			}
		}

		#endregion
	}
}
