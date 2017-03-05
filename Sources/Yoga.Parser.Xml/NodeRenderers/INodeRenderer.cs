namespace Yoga.Parser
{
	using System;

	public interface INodeRenderer
	{
		string Name { get; }

		Type Type { get; }

		object Render(INode node);
	}
}