namespace Yoga.Parser
{
	using System.Collections.Generic;
	using System.IO;

	public class YogaBinaryParser : YogaParser
	{
		protected override INode ReadNode(Stream stream)
		{
			using (var reader = new BinaryReader(stream))
				return new BinaryNode(this,reader);
		}

		public override void Write(Stream stream, INode node)
		{
			using (var writer = new BinaryWriter(stream))
				new BinaryNode(node, writer);
		}

		#region Names

		private Dictionary<int, string> names = new Dictionary<int, string>();

		public void RegisterName(int id, string name) => names[id] = name;

		public string GetName(int id) => names[id];

		#endregion
	}
}
