namespace Yoga.Parser
{
	using System;
	using System.Reflection;

	public class TypePropertyRenderer : IPropertyRenderer
	{
		public TypePropertyRenderer(PropertyInfo info, object defaultValue = null)
		{
			this.info = info ?? throw new ArgumentNullException(nameof(info));

			var type = info.DeclaringType;

			if (defaultValue == null && type.GetTypeInfo().IsValueType)
				defaultValue = Activator.CreateInstance(type);

			this.defaultValue = defaultValue;
		}

		public TypePropertyRenderer(Type type, string name, object defaultValue = null) : this(type.GetRuntimeProperty(name), defaultValue) {}

		private object defaultValue;

		private PropertyInfo info;

		public string Name => info.Name;

		public Type Type => this.info.PropertyType;

		public virtual void Render(object parent, object value)
		{
			this.info.SetValue(parent, value);
		}
	}

	public class TypePropertyRenderer<T, TProperty> : TypePropertyRenderer
	{
		public TypePropertyRenderer(string name, TProperty defaultValue = default(TProperty)) : base(typeof(T), name, defaultValue) { }
	}
}
