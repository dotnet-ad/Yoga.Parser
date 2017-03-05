namespace Yoga.Parser
{
	using System;

	public interface IPropertyRenderer
	{
		string Name { get; }

		Type Type { get; }

		void Render(object parent, object value);
	}
}
