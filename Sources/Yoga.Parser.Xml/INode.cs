﻿namespace Yoga.Parser
{
	using System.Collections.Generic;

	public interface INode
	{
		string Name { get; }

		IDictionary<string, object> Properties { get; }

		IEnumerable<INode> Children { get; }
	}
}
