namespace Yoga.Parser
{
	using System;

	public interface IRenderer
	{
		string Name { get; }

		Type Type { get; }

		object Render(INode node);
	}
}