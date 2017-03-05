namespace Yoga.Parser
{
	using System.Collections.Generic;
	using System.Linq;
	using System;
	using System.IO;

	public class BinaryNode : INode
	{
		#region Separators

		public const int StartNodeSeparator = 50001;

		public const int EndNodeSeparator = 50002;

		public const int NodeNameSeparator = 50003;

		public const int ArrayValueTypeSeparator = 50004;

		public const int IntegerValueTypeSeparator = 50005;

		public const int FloatValueTypeSeparator = 50006;

		public const int ByteValueTypeSeparator = 50007;

		public const int BoolValueTypeSeparator = 50008;

		public const int StringValueTypeSeparator = 50009;

		#endregion

		public BinaryNode(INode node, BinaryWriter writer)
		{
			this.Name = node.Name;
			this.Properties = new Dictionary<string, object>(node.Properties);
			this.children = node.Children.ToList();
			this.Write(this, writer);

		}

		public BinaryNode(YogaBinaryParser parser, BinaryReader reader)
		{
			this.parser = parser;
			this.Read(reader);
		}

		#region Read

		private void Read(BinaryReader reader)
		{
			int current = reader.Read();

			if (current != StartNodeSeparator)
				throw new InvalidOperationException();

			while ((current = reader.PeekChar()) != -1)
			{
				if(current == StartNodeSeparator)
				{
					var child = new BinaryNode(parser, reader);
					this.children.Add(child);
				}
				else if (current == EndNodeSeparator)
				{
					reader.Read();
					return;
				}
				else if (current == NodeNameSeparator)
				{
					reader.Read();
					current = reader.Read();
					this.Name = this.parser.GetName(current);
				}
				else
				{
					current = reader.Read();
					var pname = this.parser.GetName(current);
					var pvaluetype = GetValueTypeFromSeparator(reader.Read());
					this.Properties[pname] = ReadValue(pvaluetype,reader);
				}
			}
		}

		private static readonly Dictionary<Type, int> valueTypeSeparators = new Dictionary<Type, int>
		{
			{ typeof(bool), BoolValueTypeSeparator },
			{ typeof(float), FloatValueTypeSeparator },
			{ typeof(byte), ByteValueTypeSeparator },
			{ typeof(string), StringValueTypeSeparator },
			{ typeof(int), IntegerValueTypeSeparator },
			{ typeof(Array), ArrayValueTypeSeparator },
		};

		private Type GetValueTypeFromSeparator(int pvaluetype)
		{
			var result = valueTypeSeparators.FirstOrDefault(x => x.Value == pvaluetype).Key;
			if(result == null)
					throw new InvalidDataException($"No value type found for byte : {pvaluetype}");
			return result;
		}

		private object ReadValue(Type t, BinaryReader reader)
		{
			if (t == typeof(bool))
				return reader.ReadBoolean();
			
			if (t == typeof(float))
				return reader.ReadSingle();

			if (t == typeof(byte))
				return reader.ReadByte();

			if (t == typeof(string))
				return reader.ReadString();

			if (t == typeof(int))
				return reader.ReadInt32();

			if(t == typeof(Array))
			{
				var arrayitemtype = GetValueTypeFromSeparator(reader.Read());

				if (arrayitemtype == typeof(Array))
					throw new InvalidDataException($"Only one dimension arrays are supported");

				var length = reader.Read();

				var array = Array.CreateInstance(arrayitemtype,length);

				for (int i = 0; i < length; i++)
				{
					var item = ReadValue(arrayitemtype, reader);
					array.SetValue(array, i);
				}

				return array;
			}

			throw new InvalidDataException($"Can't read value of type : {t}");
		}

		#endregion

		#region Write

		public void Write(INode node, BinaryWriter writer)
		{
			writer.Write(StartNodeSeparator);

			foreach (var p in node.Properties)
			{
				var v = parser.ConvertValue(p.Value, typeof(bool), typeof(int), typeof(float), typeof(string));
				var separator = GetValueSeparatorFromType(v.GetType());
				writer.Write(separator);
				WriteValue(v, writer);
			}

			foreach (var p in node.Children)
			{
				Write(p, writer);
			}

			writer.Write(EndNodeSeparator);
		}

		private int GetValueSeparatorFromType(Type t)
		{
			int r;
			if (valueTypeSeparators.TryGetValue(t, out r))
				return r;
			throw new InvalidDataException($"No value separator found for type : {t}");
		}

		private void WriteValue(object v, BinaryWriter writer)
		{
			if(v != null)
			{
				var t = v.GetType();

				if (t == typeof(bool))
					writer.Write((bool)v);
				else if (t == typeof(float))
					writer.Write((float)v);
				else if (t == typeof(byte))
					writer.Write((byte)v);
				else if (t == typeof(string))
					writer.Write((string)v);
				else if (t == typeof(int))
					writer.Write((int)v);
				else if (v != null && t.IsArray)
				{
					var array = v as Array;

					var itemseparator = GetValueSeparatorFromType(t.GetElementType());
					writer.Write(itemseparator);
					writer.Write(array.Length);

					for (int i = 0; i < array.Length; i++)
					{
						WriteValue(array.GetValue(i), writer);
					}
				}
			}
		}

		#endregion

		#region Fields

		private List<INode> children = new List<INode>();

		private YogaBinaryParser parser;

		#endregion

		#region Properties

		public string Name { get; private set; }

		public IEnumerable<INode> Children => this.children;

		public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();

		#endregion
	}
}
